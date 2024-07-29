open System
open System.Diagnostics
open System.Text
open System.Text.RegularExpressions
open System.Web
open Newtonsoft.Json
open Pinicola.FSharp.SpectreConsole
open Spectre.Console
open TextCopy

[<RequireQualifiedAccess>]
module List =
    let apply v l = l |> List.map (fun f -> f v)

[<RequireQualifiedAccess>]
module String =
    let trimStart (start: string) (s: string) =
        if s.StartsWith(start) then s.Substring(start.Length) else s

    let trimEnd (end_: string) (s: string) =
        if s.EndsWith(end_) then
            s.Substring(0, s.Length - end_.Length)
        else
            s

    let replace (oldValue: string) (newValue: string) (s: string) = s.Replace(oldValue, newValue)

    let replaceDiacritics (s: string) =
        s.Normalize(NormalizationForm.FormD)
        |> (fun s -> Regex.Replace(s, "[^a-zA-z0-9 ]", ""))
        |> replace " " "-"

let json (s: string) : string = JsonConvert.SerializeObject(s)

let clipboard = Clipboard()
let clipboardContent = clipboard.GetText()

AnsiConsole.markupLineInterpolated $"Clipboard content: [yellow]{clipboardContent}[/]"

let conventionalCommitRegex =
    Regex(@"^(?<type>(fix|feat))(?:\((?<scope>[\w]+)\))?: (?<description>.+)$", RegexOptions.Compiled)

let options =
    if conventionalCommitRegex.IsMatch(clipboardContent) then
        let m = conventionalCommitRegex.Match(clipboardContent)

        let description =
            m.Groups.["description"].Value
            |> String.replaceDiacritics
            |> String.replace " " "-"

        let scope =
            if m.Groups.["scope"].Success then
                "--" + m.Groups.["scope"].Value
            else
                ""

        [ $"""{m.Groups.["type"].Value}/{description}{scope}""" ]
    elif clipboardContent.StartsWith("javascript:") then
        [
            clipboardContent
            |> HttpUtility.UrlDecode
            |> String.trimStart "javascript:(function(){"
            |> String.trimEnd "})();"
        ]
    else
        [
            json
            (sprintf "\"%s\"") >> json >> (fun c -> c.Substring(1, c.Length - 2))
            json >> json
            Regex.Escape
            Regex.Escape >> json
        ]
        |> List.apply clipboardContent

let choice =
    SelectionPrompt.init ()
    |> SelectionPrompt.setTitle "Select a transformation"
    |> SelectionPrompt.addChoices options
    |> AnsiConsole.prompt

clipboard.SetText(choice)

AnsiConsole.markupLineInterpolated $"New clipboard content: [yellow]{choice}[/]"

if not (Environment.GetCommandLineArgs().Length > 1) then
    AnsiConsole.Confirm("Press any key to exit...") |> ignore
