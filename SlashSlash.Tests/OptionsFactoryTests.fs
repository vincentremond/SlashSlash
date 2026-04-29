namespace SlashSlash.Tests

open NUnit.Framework
open AwesomeAssertions
open SlashSlash

[<TestFixture>]
module OptionsFactoryTests =

    let shouldContainKey key expectedValue (result: Map<string, string>) =
        match result |> Map.tryFind key with
        | Some value -> value.Should().Be(expectedValue) |> ignore
        | None -> Assert.Fail($"Expected key '{key}' not found in result.")

    [<TestCase("feat(ui): add new button", "feat/add-new-button--ui")>]
    [<TestCase("fix: resolve memory leak", "fix/resolve-memory-leak")>]
    let ``get with conventional commit returns Branch name`` input expected =
        let result = OptionsFactory.get input
        result |> shouldContainKey "Branch name" expected

    [<TestCase("#123 [core] update dependencies", "feat/update-dependencies--123", "feat: update dependencies #123")>]
    [<TestCase("#123 🎯[core] update dependencies", "feat/update-dependencies--123", "feat: update dependencies #123")>]
    [<TestCase("456 [My Super Scope] Deploy “Premium” on Todo", "feat/Deploy-Premium-on-Todo--456", "feat: Deploy “Premium” on Todo #456")>]
    let ``get with Work Item pattern returns Branch name and Commit message`` input expectedBranchName expectedCommitMsg =
        let result = OptionsFactory.get input

        result |> shouldContainKey "Branch name" expectedBranchName
        result |> shouldContainKey "Commit message" expectedCommitMsg

    [<TestCase("javascript:(function(){alert('hello%20world');})();", "alert('hello world');")>]
    let ``get with bookmarklet returns Decoded Bookmarklet`` input expected =
        let result = OptionsFactory.get input
        result |> shouldContainKey "Bookmarklet" expected

    [<TestCase("plain text", "\"plain text\"")>]
    let ``get with plain text returns default options including Json`` input expected =
        let result = OptionsFactory.get input
        result |> shouldContainKey "Json" expected

    [<TestCase(@"C:\Users\Jean\Downloads\", "/mnt/c/Users/Jean/Downloads/")>]
    let ``get with Windows patch returns Unix path`` input expected =
        let result = OptionsFactory.get input
        result |> shouldContainKey "Unix path" expected
