# Gemini Context: SlashSlash

## Project Overview
**SlashSlash** is a command-line utility designed to transform clipboard content through various operations, such as generating conventional commit branch names, decoding bookmarklets, and performing JSON/Regex transformations.

- **Main Technologies:** F#, .NET 10.0
- **Key Libraries:**
  - [Spectre.Console](https://spectreconsole.net/) - Interactive console UI.
  - [Newtonsoft.Json](https://www.newtonsoft.com/json) - JSON processing.
  - [TextCopy](https://github.com/SimonCropp/TextCopy) - Clipboard integration.
  - **Pinicola.FSharp** - A local library/submodule providing common F# utilities and Spectre.Console wrappers.

## Architecture
The project is a simple interactive CLI application (`SlashSlash` project) that uses `Pinicola.FSharp` for shared logic and UI abstractions. It operates in a loop, presenting transformations to the user and updating the clipboard with the result.

## Building and Running

### Prerequisites
- .NET 10.0 SDK
- [Paket](https://fsprojects.github.io/Paket/) (restored via `dotnet tool restore`)

### Commands
- **Restore Tools:** `dotnet tool restore`
- **Build:** `dotnet build` or run `.\build.ps1`
- **Run:** `dotnet run --project SlashSlash/SlashSlash.fsproj`
- **Test:** `dotnet test` (Runs tests in `Pinicola.FSharp.Tests` and `SlashSlash.Tests`)

## Development Conventions

- **Language:** F# is the primary language. Adhere to F# idiomatic patterns (e.g., pipeline operators, pattern matching).
- **Dependency Management:** Uses [Paket](https://fsprojects.github.io/Paket/). Dependencies are defined in `paket.dependencies` and integrated via `Paket.Restore.targets`.
- **UI:** Interactive CLI using `Spectre.Console`. Use `Pinicola.FSharp.SpectreConsole` for higher-level UI abstractions.
- **Testing:**
  - Framework: [NUnit](https://nunit.org/)
  - Assertions: [AwesomeAssertions](https://awesomeassertions.org/) (a fork of FluentAssertions)
- **Modularization:** Shared logic should be placed in the `Pinicola.FSharp` project when applicable.

## Key Files
- `SlashSlash/OptionsFactory.fs`: Logic for generating transformation options.
- `SlashSlash/Program.fs`: Main entry point and CLI loop.
- `SlashSlash.Tests/`: Unit tests for `SlashSlash` project.
- `SlashSlash/SlashSlash.fsproj`: Main project file.
- `Pinicola.FSharp/`: Library containing utility functions for Strings, Regex, and Spectre.Console.
- `build.ps1`: Simple build script.
- `paket.dependencies`: Root dependency definitions.
