@echo off

dotnet paket update
dotnet tool restore
dotnet build

AddToPath .\SlashSlash\bin\Debug\
