namespace SlashSlash

[<RequireQualifiedAccess>]
module List =
    let apply value list = list |> List.map (fun f -> f value)

    let applySnd value list =
        list |> List.map (fun (a, f) -> (a, f value))
