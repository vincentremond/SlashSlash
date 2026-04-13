namespace SlashSlash.Tests

open NUnit.Framework
open AwesomeAssertions
open SlashSlash

[<TestFixture>]
type OptionsFactoryTests() =

    [<Test>]
    member _.``get with conventional commit (feat) returns Branch name``() =
        let input = "feat(ui): add new button"
        let result = OptionsFactory.get input

        let branchName = result |> List.find (fun (name, _) -> name = "Branch name") |> snd
        branchName.Should().Be("feat/add-new-button--ui") |> ignore

    [<Test>]
    member _.``get with conventional commit (fix) returns Branch name``() =
        let input = "fix: resolve memory leak"
        let result = OptionsFactory.get input

        let branchName = result |> List.find (fun (name, _) -> name = "Branch name") |> snd
        branchName.Should().Be("fix/resolve-memory-leak") |> ignore

    [<Test>]
    member _.``get with Work Item pattern returns Branch name and Commit message``() =
        let input = "#123 [core] update dependencies"
        let result = OptionsFactory.get input

        let branchName = result |> List.find (fun (name, _) -> name = "Branch name") |> snd

        let commitMsg =
            result |> List.find (fun (name, _) -> name = "Commit message") |> snd

        branchName.Should().Be("feat/update-dependencies--123") |> ignore
        commitMsg.Should().Be("feat: update dependencies #123") |> ignore

    [<Test>]
    member _.``get with bookmarklet returns Decoded Bookmarklet``() =
        let input = "javascript:(function(){alert('hello%20world');})();"
        let result = OptionsFactory.get input

        let bookmarklet = result |> List.find (fun (name, _) -> name = "Bookmarklet") |> snd
        bookmarklet.Should().Be("alert('hello world');") |> ignore

    [<Test>]
    member _.``get with plain text returns default options including Json``() =
        let input = "plain text"
        let result = OptionsFactory.get input

        result
        |> List.exists (fun (name, _) -> name = "Json")
        |> _.Should().BeTrue()
        |> ignore

        let jsonVal = result |> List.find (fun (name, _) -> name = "Json") |> snd
        jsonVal.Should().Be("\"plain text\"") |> ignore
