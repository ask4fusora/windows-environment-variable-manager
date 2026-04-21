# PATH Operations

## Status

Planned.

## Goal

Provide safe, explicit workflows for editing `PATH` as a list of segments
instead of treating it as an opaque string.

## Problem

`PATH` is one of the hardest environment variables to manage manually on
Windows:

- it is long
- it is easy to duplicate entries
- it is easy to leave stale entries behind
- it is difficult to visually diff and reason about as one giant string

## Scope

PATH-specific workflows should build on top of the existing registry-backed
environment variable operations.

These commands should target the `PATH` variable only and treat it as an ordered
list of path segments.

## Terminology

Recommended user-facing term: `PATH segment`

Recommended operations:

- `path add <segment>`
- `path remove <segment>`
- `path list`
- `path prune`

Placement modifiers:

- `--append`
- `--prepend`

## Terminology Decisions

### Add

`add` should be the main verb, with `--append` and `--prepend` controlling
placement.

### Remove

`remove` should mean exact removal of a specific PATH segment.

### Prune

`prune` should be reserved for cleanup-oriented workflows such as:

- removing duplicate segments
- removing empty segments
- optionally removing nonexistent directories

`prune` should not be used as the primary exact-delete verb.

## Intended UX

Examples:

```powershell
wem path list
wem path add "C:\Tools\Bin" --append
wem path add "C:\Tools\Bin" --prepend
wem path remove "C:\Tools\Bin"
wem path prune
```

## Requirements

- preserve segment order unless explicitly changed
- avoid accidental duplication when adding
- expose exact removal, not fuzzy removal
- preserve scope support with `user` default and optional `machine`
- integrate with future local version history so PATH edits can be reviewed and
  reverted

## Non-Goals

- guessing which segment the user meant
- auto-repairing the entire environment silently
- treating arbitrary environment variables as PATH-like by default
