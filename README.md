# Mouse Jiggler

A focused Windows desktop application that prevents idle detection by periodically moving the mouse pointer. This repository contains only the application logic and UI implementation, with all package manager distribution tooling removed.

## Project Overview

`MouseJiggler` is implemented as a Windows Forms app targeting .NET 10 (`net10.0-windows10.0.17763.0`). The main executable entrypoint is `MouseJiggler/Program.cs`, and the UI is implemented in `MouseJiggler/MainForm.cs`.

The app is designed to run as a single instance and supports both interactive GUI and command-line launch modes.

## Key Features

- Four jiggle modes:
  - `Normal` — visible diagonal pointer movement.
  - `Zen` — virtual pointer movement that avoids visible cursor motion.
  - `Circle` — circular pointer motion.
  - `Linear` — side-to-side pointer motion.
- Configurable jiggle interval in seconds.
- Adjustable distance multiplier from `1` to `120`.
- Optional random timer variation.
- Optional start minimized.
- Optional start with jiggling enabled.
- Optional startup display of the settings panel.
- Tray icon status text that summarizes the current jiggling state.
- Global hotkey toggle support: `Ctrl+Shift+J`.
- Respect locked session behavior when enabled.

## Command-Line Usage

The application supports the following command-line options:

- `-j`, `--jiggle` — start with jiggling enabled.
- `-m`, `--minimized` — start minimized.
- `-o`, `--mode <Circle|Linear|Normal|Zen>` — select jiggle mode.
- `-r`, `--random` — enable random variation.
- `-s`, `--seconds <seconds>` — set the jiggle interval.
- `-d`, `--distance <distance>` — set the distance multiplier.
- `-g`, `--settings` — open the settings panel on startup.

Example:

```powershell
MouseJiggler.exe --jiggle --mode Zen --seconds 30 --distance 2 --minimized
```

## Code Structure

- `MouseJiggler/Program.cs` — CLI parsing, single-instance enforcement, and application startup flow.
- `MouseJiggler/MainForm.cs` — main settings UI, tray integration, hotkey handling, and jiggling state.
- `MouseJiggler/JiggleMode.cs` — enum for available jiggle modes.
- `MouseJiggler/JigglePatterns.cs` — algorithmic jiggle pattern implementation.
- `MouseJiggler/Helpers.cs` — shared helpers used by the application.
- `MouseJiggler/Properties/Settings.settings` — persisted application settings.
- `MouseJiggler/MouseJiggler.csproj` — project file for the WinForms application.

## Build Instructions

### Prerequisites

- .NET 10 SDK
- Visual Studio 2022 or later with Windows Forms workload (recommended)

### Build from the command line

```powershell
cd "C:\Users\Lakshya\Documents\Downloads\Mouse-jiggler"
dotnet build MouseJiggler\MouseJiggler.csproj -c Release
```

### Open in Visual Studio

Open `MouseJiggler\MouseJiggler.sln` and build the solution.

## Repository Cleanup Notes

This repository no longer contains any Chocolatey or WinGet packaging artifacts. The following files were removed as part of the cleanup:

- `MouseJiggler/choco/mousejiggler.nuspec`
- `MouseJiggler/choco/tools/chocolateyinstall.ps1`
- `MouseJiggler/choco/tools/chocolateyuninstall.ps1`
- `MouseJiggler/choco/tools/VERIFICATION.txt`
- `MouseJiggler/winget/ArkaneSystems.MouseJiggler.yaml`
- `MouseJiggler/winget/ArkaneSystems.MouseJiggler.locale.en-US.yaml`
- `MouseJiggler/winget/ArkaneSystems.MouseJiggler.installer.yaml`

The project file was also cleaned to remove package-related metadata entries that are not relevant to the application runtime.

## Notes

- The app is intentionally kept focused on application logic and Windows Forms behavior.
- No packaging automation, `choco pack`, or `wingetcreate` build hooks are present in the active project file.

## License

This project is licensed under the terms of the `LICENSE` file in the repository root.
