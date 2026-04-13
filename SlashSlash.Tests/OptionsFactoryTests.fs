namespace SlashSlash.Tests

open NUnit.Framework
open AwesomeAssertions
open SlashSlash

[<TestFixture>]
module OptionsFactoryTests =

    [<Test>]
    let ``get with conventional commit (feat) returns Branch name`` () =
        let input = "feat(ui): add new button"
        let result = OptionsFactory.get input

        let branchName = result |> Map.find "Branch name"
        branchName.Should().Be("feat/add-new-button--ui") |> ignore

    [<Test>]
    let ``get with conventional commit (fix) returns Branch name`` () =
        let input = "fix: resolve memory leak"
        let result = OptionsFactory.get input

        let branchName = result |> Map.find "Branch name"
        branchName.Should().Be("fix/resolve-memory-leak") |> ignore

    [<Test>]
    [<TestCase("#123 [core] update dependencies", "feat/update-dependencies--123", "feat: update dependencies #123")>]
    [<TestCase("456 [My Super Scope] Deploy “Premium” on Todo", "feat/Deploy-Premium-on-Todo--456", "feat: Deploy “Premium” on Todo #456")>]
    let ``get with Work Item pattern returns Branch name and Commit message`` input expectedBranchName expectedCommitMsg =
        let result = OptionsFactory.get input

        let branchName = result |> Map.find "Branch name"

        let commitMsg = result |> Map.find "Commit message"

        branchName.Should().Be(expectedBranchName) |> ignore
        commitMsg.Should().Be(expectedCommitMsg) |> ignore

    [<Test>]
    let ``get with bookmarklet returns Decoded Bookmarklet`` () =
        let input = "javascript:(function(){alert('hello%20world');})();"
        let result = OptionsFactory.get input

        let bookmarklet = result |> Map.find "Bookmarklet"
        bookmarklet.Should().Be("alert('hello world');") |> ignore

    [<Test>]
    let ``get with plain text returns default options including Json`` () =
        let input = "plain text"
        let result = OptionsFactory.get input

        result |> Map.containsKey "Json" |> _.Should().BeTrue() |> ignore

        let jsonVal = result |> Map.find "Json"
        jsonVal.Should().Be("\"plain text\"") |> ignore
