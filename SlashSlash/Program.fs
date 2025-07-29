open System
open System.Diagnostics
open System.Text
open System.Text.RegularExpressions
open System.Web
open Newtonsoft.Json
open Pinicola.FSharp.SpectreConsole
open Pinicola.FSharp.RegularExpressions
open Spectre.Console
open TextCopy

[<RequireQualifiedAccess>]
module List =
    let apply value list = list |> List.map (fun f -> f value)

    let applySnd value list =
        list |> List.map (fun (a, f) -> (a, f value))

[<RequireQualifiedAccess>]
module String =
    let trimStart (start: string) (s: string) =
        if s.StartsWith(start) then s.Substring(start.Length) else s

    let trimEnd (end_: string) (s: string) =
        if s.EndsWith(end_) then
            s.Substring(0, s.Length - end_.Length)
        else
            s

    let trimChar (c: char) (s: string) = s.Trim(c)

    let trimSingleChar (c: char) (s: string) =
        if s.Length > 2 && s.StartsWith(c) && s.EndsWith(c) then
            s.Substring(1, s.Length - 2)
        else
            s

    let trimString (toTrim: string) = (trimStart toTrim >> trimEnd toTrim)

    let replace (oldValue: string) (newValue: string) (s: string) = s.Replace(oldValue, newValue)

    let replaceDiacritics (s: string) =
        s.Normalize(NormalizationForm.FormD)
        |> (fun s -> Regex.Replace(s, "[^a-zA-z0-9 ]", ""))
        |> replace " " "-"

let json (s: string) : string = JsonConvert.SerializeObject(s)
let unitTestPatternA = Regex(@"[\s']")
let unitTestA (s: string) : string = unitTestPatternA.Replace(s, "_")

let clipboard = Clipboard()
let clipboardContent = clipboard.GetText()

AnsiConsole.markupLineInterpolated $"Clipboard content: [yellow]{clipboardContent}[/]"

let conventionalCommitRegex =
    Regex(@"^(?<type>(fix|feat))(?:\((?<scope>.+)\))?: (?<description>.+)$", RegexOptions.Compiled)

let options: (string * string) list =
    if conventionalCommitRegex.IsMatch(clipboardContent) then
        let m = conventionalCommitRegex.Match(clipboardContent)

        let conventionalCommitDescription =
            m.Groups.["description"].Value
            |> String.replaceDiacritics
            |> String.replace " " "-"

        let scope =
            if m.Groups.["scope"].Success then
                let scope = m.Groups.["scope"].Value |> Regex.replace @"[^\w]" ""
                "--" + scope
            else
                ""

        [ "Branch name", $"""{m.Groups.["type"].Value}/{conventionalCommitDescription}{scope}""" ]
    elif clipboardContent.StartsWith("javascript:") then
        [
            "Bookmarklet",
            clipboardContent
            |> HttpUtility.UrlDecode
            |> String.trimStart "javascript:(function(){"
            |> String.trimEnd "})();"
        ]
    else
        [
            "Json", json
            "Json (double quote trimmed)", ((String.trimChar '"') >> json)
            "Json ( ?!? )", ((String.trimChar '"') >> json >> (String.trimSingleChar '"'))

            "Json ( ?!? )", ((sprintf "\"%s\"") >> json >> (fun c -> c.Substring(1, c.Length - 2)))
            "json²", (json >> json)
            "Regex escape", Regex.Escape
            "Regex escape + json", Regex.Escape >> json
            "Unit test A", unitTestA
        ]
        |> List.applySnd clipboardContent

let maxLen = options |> List.map (fst >> String.length) |> List.max

let choice =
    SelectionPrompt.init ()
    |> SelectionPrompt.withTitle "Select a transformation"
    |> SelectionPrompt.addChoices options
    |> SelectionPrompt.useConverter (fun (label, value) ->
        $"[yellow]{label.PadRight(maxLen, '.')}[/]: {value |> Markup.escape}"
    )
    |> AnsiConsole.prompt
    |> snd

clipboard.SetText(choice)

AnsiConsole.markupLineInterpolated $"New clipboard content: [yellow]{choice}[/]"

if not (Environment.GetCommandLineArgs().Length > 1) then
    AnsiConsole.Confirm("Press any key to exit...") |> ignore
