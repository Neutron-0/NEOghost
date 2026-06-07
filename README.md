# Mouse Jiggler

A focused Windows desktop application that prevents idle detection by gently simulating mouse movement. This repository is intentionally stripped of packaging, deployment, and distribution tooling so it remains clean and centered on the application logic.

## Project Context

`MouseJiggler` is a Windows Forms application targeting .NET 10 (`net10.0-windows10.0.17763.0`). It is built to run as a single instance, with both a user interface and command-line support.

The primary application behavior is implemented in the `MouseJiggler/` folder, while supporting runtime settings and persistence are handled through the `Properties` folder.

### What this repository currently contains

- `MouseJiggler/Program.cs`: startup flow, command line parsing, single-instance enforcement, and console attachment for help output.
- `MouseJiggler/MainForm.cs`: settings UI, tray icon integration, hotkey registration, and jiggling state management.
- `MouseJiggler/JiggleMode.cs`: available jiggle modes.
- `MouseJiggler/JigglePatterns.cs`: jiggle motion algorithms.
- `MouseJiggler/Helpers.cs`: shared helpers and platform interop.
- `MouseJiggler/Properties/Settings.settings`: persisted user preferences.
- `MouseJiggler/MouseJiggler.csproj`: project file for the Windows Forms application.

## Application Behavior

`MouseJiggler` works by periodically generating mouse input to prevent idle detection and screen saver activation.

Key runtime behaviors:

- Ensures only one instance is active using a named mutex.
- Optionally starts with jiggling enabled.
- Supports a tray icon with live status text showing mode, interval, and distance.
- Allows `Ctrl+Shift+J` as a global toggle hotkey.
- Supports optional random timing variation.
- Respects lock state settings when configured.
- Offers four movement modes: `Normal`, `Zen`, `Circle`, and `Linear`.

## Command-Line Usage

The application supports these options:

- `-j`, `--jiggle` — start in jiggle mode.
- `-m`, `--minimized` — start minimized.
- `-o`, `--mode <Circle|Linear|Normal|Zen>` — choose the jiggle mode.
- `-r`, `--random` — enable random variation.
- `-s`, `--seconds <seconds>` — set the jiggle interval.
- `-d`, `--distance <distance>` — set the movement distance multiplier.
- `-g`, `--settings` — show the settings panel on startup.

Example:

```powershell
MouseJiggler.exe --jiggle --mode Zen --seconds 30 --distance 2 --minimized
```

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

## Project Audit: Past Author References

A scan of the active source and project file identified legacy attribution references from the original project.

### Files containing past author metadata

- `MouseJiggler/Program.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `MouseJiggler/MainForm.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `MouseJiggler/Helpers.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `MouseJiggler/AboutBox.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `MouseJiggler/MouseJiggler.csproj`
  - `<Authors>Alistair J. R. Young, Dimitris Panokostas</Authors>`
  - `<Company>Arkane Systems</Company>`
  - `<Copyright>Copyright © Alistair J. R. Young 2007-2026</Copyright>`

### Notes on the audit

- No active packaging build hooks were found in the current `MouseJiggler.csproj` file.
- Legacy author references were present in source headers and project metadata but have now been rewritten for this repository.
- The repository no longer contains Chocolatey or WinGet distribution artifacts.

## Removed Artifacts

The following packaging and distribution files were deleted from the repository:

- `MouseJiggler/choco/mousejiggler.nuspec`
- `MouseJiggler/choco/tools/chocolateyinstall.ps1`
- `MouseJiggler/choco/tools/chocolateyuninstall.ps1`
- `MouseJiggler/choco/tools/VERIFICATION.txt`
- `MouseJiggler/winget/ArkaneSystems.MouseJiggler.yaml`
- `MouseJiggler/winget/ArkaneSystems.MouseJiggler.locale.en-US.yaml`
- `MouseJiggler/winget/ArkaneSystems.MouseJiggler.installer.yaml`

## Repository Standards

This repository now includes the common files expected by modern open source projects:

- `CONTRIBUTING.md` — contribution guidelines and workflow expectations.
- `CODE_OF_CONDUCT.md` — community standards for collaboration.
- `SECURITY.md` — secure reporting guidance for vulnerabilities.
- `LICENSE` — the current source license for this project.

### Current License

This repository is currently licensed under the Microsoft Public License (Ms-PL), as defined in the `LICENSE` file. It is not currently licensed under the MIT license. If you want to adopt the MIT license instead, the existing `LICENSE` file should be replaced with MIT text and the README updated accordingly.

## Notes

- This repository is now focused strictly on application logic and Windows Forms behavior.
- The README was updated to reflect the current state of the project and the cleanup work completed.

## License

This project is licensed under the terms of the `LICENSE` file in the repository root.
