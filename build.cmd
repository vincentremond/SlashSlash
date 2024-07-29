@echo off

dotnet paket update
dotnet tool restore
dotnet build

add-to-path SlashSlash\bin\Debug\
