open Pinicola.FSharp.SpectreConsole
open SlashSlash
open TextCopy

let clipboardContent = ClipboardService.GetText()

AnsiConsole.markupLineInterpolated $"Clipboard content: [yellow]{clipboardContent}[/]"

let options = OptionsFactory.get clipboardContent

let maxLen = options |> List.map (fst >> String.length) |> List.max

let rec loop () =
    let choice =
        SelectionPrompt.init ()
        |> SelectionPrompt.withTitle (Raw "Select a transformation")
        |> SelectionPrompt.addChoices options
        |> SelectionPrompt.useConverter (fun (label, value) -> SpectreConsoleString.fromInterpolated $"[yellow]{label.PadRight(maxLen, '.')}[/]: {value |> Markup.escape}")
        |> AnsiConsole.prompt
        |> snd

    ClipboardService.SetText(choice)

    AnsiConsole.markupLineInterpolated $"New clipboard content: [yellow]{choice}[/]"

    loop ()

loop ()
