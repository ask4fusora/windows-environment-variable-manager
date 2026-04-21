# Windows Environment Variable Manager (wem)

Windows Environment Variable Manager is a Windows-first tool for managing
persistent `User` and `Machine` environment variables through the registry,
with a CLI-first workflow today and safer, more structured workflows planned.

## Why This Exists

Windows persistent environment variables are not just shell-local text files.
They are registry-backed OS settings that affect how new processes inherit
their environment. That creates a Windows-specific problem:

- programs are often launched from GUI surfaces like Explorer, shortcuts, and
  shells with different parent contexts
- work is spread across PowerShell, `cmd.exe`, Git Bash, Nushell, terminal
  multiplexers, IDE terminals, and AI-driven agents
- long values such as `PATH` are easy to break and hard to reason about

This is different from the common Linux workflow where shell startup files and
single-shell process trees are often enough for day-to-day environment
management.

This project exists to make Windows environment variable management feel more
deliberate, inspectable, and eventually reversible.

## What The Tool Does Today

- lists persistent environment variable names
- gets persistent variable values
- sets persistent variable values
- deletes persistent variable values
- supports `user` and `machine` scopes
- uses the Windows-backed .NET environment variable APIs
- provides CLI-first workflows with a future TUI direction

## What The Tool Does Not Do Yet

- it does not update the current shell session automatically
- it does not retroactively update already-running processes
- it does not yet provide PATH-specific editing workflows
- it does not yet provide local version history or rollback
- the TUI entrypoint exists, but the TUI is not feature-complete yet

## Current CLI Surface

Examples:

```powershell
wem --help
wem list
wem list --scope machine
wem get PATH
wem get PATH --scope machine
wem set FOO Bar
wem set FOO Bar --scope machine
wem delete FOO
wem delete FOO --scope machine
wem tui
```

Scope rules:

- accepted values are `user` and `machine`
- omitted scope defaults to `user`
- `machine` mutation requires an elevated Administrator shell

## Important Behavior Notes

- persisted `User` and `Machine` values are updated correctly
- the current shell session is not automatically refreshed
- already-running processes keep their existing environment blocks
- new processes may observe updated values only when launched from an updated
  parent context

## Storage Direction

Planned local configuration and version-history data will live under
`%LOCALAPPDATA%\wem\`.

## Roadmaps

Detailed mini-specs live under [roadmaps/](roadmaps/README.md):

- [Roadmap Index](roadmaps/README.md)
- [PATH Operations](roadmaps/cli/path-operations.md)
- [Shell Session Integration](roadmaps/integration/shell-session-integration.md)
- [Local Version History](roadmaps/storage/local-version-history.md)
- [Terminal GUI](roadmaps/ui/terminal-gui.md)

## Status

This project is early-stage and CLI-first.

The current focus is establishing correct Windows behavior first, then adding
safer PATH editing, local version history, shell-session integration, and a
resilient TUI.
