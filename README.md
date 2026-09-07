# SDBBGuiHelper

simple C# CLI tool to ingest a spreadsheet and spit out the DeluxeMenus YAML file required for our game.

## Build for Linux

You can do the following on Linux or Windows:

```
dotnet publish -r linux-x64 src/sdbb-gui.csproj
```

The resulting executable will be located at `build/publish/sdbb-gui`

On Linux, I'd reccomend adding `--self-contained`. This will drastically increase the file size, but it becomes a portable executable.


## Build for Windows

You can do the following on Linux or Windows:

```
dotnet publish -r win-x64 src/sdbb-gui.csproj
```

The resulting executable will be located at `build/publish/sdbb-gui.exe`
