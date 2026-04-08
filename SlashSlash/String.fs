namespace SlashSlash

open System.Text
open System.Text.RegularExpressions

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
