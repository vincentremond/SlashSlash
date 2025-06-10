$ErrorActionPreference = "Stop"

dotnet tool restore
dotnet build

AddToPath .\SlashSlash\bin\Debug\
