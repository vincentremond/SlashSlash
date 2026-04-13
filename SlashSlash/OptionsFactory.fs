namespace SlashSlash

open System.Text.RegularExpressions
open System.Web
open Newtonsoft.Json
open Pinicola.FSharp
open Pinicola.FSharp.RegularExpressions
open SlashSlash

[<RequireQualifiedAccess>]
module OptionsFactory =

    let private unitTestPatternA = Regex(@"[\s']")
    let private unitTestA (s: string) : string = unitTestPatternA.Replace(s, "_")
    let json (s: string) : string = JsonConvert.SerializeObject(s)

    let get clipboardContent =

        let items =
            match clipboardContent with
            | MatchRegex (Regex @"^(?<type>(fix|feat))(?:\((?<scope>.+)\))?: (?<description>.+)$") m ->
                let conventionalCommitDescription =
                    m.Groups["description"].Value
                    |> String.replaceDiacritics
                    |> String.replace " " "-"

                let scope =
                    if m.Groups["scope"].Success then
                        let scope = m.Groups["scope"].Value |> Regex.replacePattern @"[^\w]" ""
                        "--" + scope
                    else
                        ""

                [ "Branch name", $"""{m.Groups["type"].Value}/{conventionalCommitDescription}{scope}""" ]
            | MatchRegex (Regex @"^#?(?<Id>\d+) \[(?<Scope>(\w|\s)+)\] (?<Subject>.+)$") m ->
                let id = m.Groups["Id"].Value

                let subject = m.Groups["Subject"].Value
                let branchSubject = subject |> String.replaceDiacritics |> String.replace " " "-"

                [
                    "Branch name", $"""feat/{branchSubject}--{id}"""
                    "Commit message", $"""feat: {subject} #{id}"""
                ]

            | StartsWithICIC "javascript:" -> [
                "Bookmarklet",
                clipboardContent
                |> HttpUtility.UrlDecode
                |> String.trimStart "javascript:(function(){"
                |> String.trimEnd "})();"
              ]
            | _ ->
                [
                    "Json", json
                    "Json (double quote trimmed)", ((String.trimChar '"') >> json)
                    "Json ( ?!? )", ((String.trimChar '"') >> json >> (String.trimSingleChar '"'))

                    "Json ( !?! )", ((sprintf "\"%s\"") >> json >> (fun c -> c.Substring(1, c.Length - 2)))
                    "json²", (json >> json)
                    "Regex escape", Regex.Escape
                    "Regex escape + json", Regex.Escape >> json
                    "Unit test A", unitTestA
                ]
                |> List.applySnd clipboardContent

        items |> Map.safeOfSeq
