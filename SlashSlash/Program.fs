open Pinicola.FSharp.SpectreConsole
open SlashSlash
open TextCopy

let clipboardContent = ClipboardService.GetText()

AnsiConsole.markupLineInterpolated $"Clipboard content: [yellow]{clipboardContent}[/]"

let options = OptionsFactory.get clipboardContent

let maxLen = options |> Map.keys |> Seq.map _.Length |> Seq.max

let rec loop () =
    let choice =
        SelectionPrompt.init ()
        |> SelectionPrompt.withTitle (Raw "Select a transformation")
        |> SelectionPrompt.addChoices options
        |> SelectionPrompt.useConverter (fun kvp -> SpectreConsoleString.fromInterpolated $"[yellow]{kvp.Key.PadRight(maxLen, '.')}[/]: {kvp.Value |> Markup.escape}")
        |> AnsiConsole.prompt
        |> _.Value

    ClipboardService.SetText(choice)

    AnsiConsole.markupLineInterpolated $"New clipboard content: [yellow]{choice}[/]"

    loop ()

loop ()
