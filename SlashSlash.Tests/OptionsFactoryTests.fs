namespace SlashSlash.Tests

open NUnit.Framework
open AwesomeAssertions
open SlashSlash

[<TestFixture>]
module OptionsFactoryTests =

    [<TestCase("feat(ui): add new button", "feat/add-new-button--ui")>]
    [<TestCase("fix: resolve memory leak", "fix/resolve-memory-leak")>]
    let ``get with conventional commit returns Branch name`` input expected =
        let result = OptionsFactory.get input
        result |> Map.find "Branch name" |> _.Should().Be(expected) |> ignore

    [<TestCase("#123 [core] update dependencies", "feat/update-dependencies--123", "feat: update dependencies #123")>]
    [<TestCase("456 [My Super Scope] Deploy “Premium” on Todo", "feat/Deploy-Premium-on-Todo--456", "feat: Deploy “Premium” on Todo #456")>]
    let ``get with Work Item pattern returns Branch name and Commit message`` input expectedBranchName expectedCommitMsg =
        let result = OptionsFactory.get input

        let branchName = result |> Map.find "Branch name"
        let commitMsg = result |> Map.find "Commit message"

        branchName.Should().Be(expectedBranchName) |> ignore
        commitMsg.Should().Be(expectedCommitMsg) |> ignore

    [<TestCase("javascript:(function(){alert('hello%20world');})();", "alert('hello world');")>]
    let ``get with bookmarklet returns Decoded Bookmarklet`` input expected =
        let result = OptionsFactory.get input
        result |> Map.find "Bookmarklet" |> _.Should().Be(expected) |> ignore

    [<TestCase("plain text", "\"plain text\"")>]
    let ``get with plain text returns default options including Json`` input expected =
        let result = OptionsFactory.get input
        result |> Map.containsKey "Json" |> _.Should().BeTrue() |> ignore
        result |> Map.find "Json" |> _.Should().Be(expected) |> ignore
