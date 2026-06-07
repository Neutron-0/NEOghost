# Contributing to Mouse Jiggler

Thank you for taking an interest in contributing to Mouse Jiggler.

## How to contribute

- Open an issue for any bug, enhancement, or question before beginning work.
- Prefer small, focused pull requests that address a single concern.
- Use the `main` branch as the stable development base.
- If you want to contribute a feature, fork the repository and create a branch with a descriptive name.

## Coding conventions

- Keep the user interface and business logic separated where practical.
- Preserve the existing Windows Forms patterns used in the project.
- Keep changes localized and avoid broad refactors unless required for the fix.
- Use meaningful commit messages that explain the intent of the change.

## Testing

- Build the project using `dotnet build MouseJiggler\MouseJiggler.csproj -c Release`.
- Test the UI paths manually if your change affects settings, hotkeys, or tray behavior.
- Verify that command-line options still parse correctly.

## Pull requests

- Rebase or merge frequently to keep your branch up to date with `main`.
- Include a clear description of the problem and your proposed solution.
- Mention any manual testing you performed.
- Do not include packaging or distribution artifacts unless explicitly requested.
