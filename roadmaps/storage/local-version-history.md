# Local Version History

## Status

Planned.

## Goal

Add local, SQLite-backed history so environment variable changes can be
inspected, compared, and reverted safely.

## Motivation

Environment variables are easy to break and hard to recover manually,
especially:

- long values such as `PATH`
- shared machine-wide values
- changes performed over time from different shells and tools

Version history should make these changes auditable and reversible.

## Storage Location

Planned local storage root:

`%LOCALAPPDATA%\wem\`

Expected contents later:

- SQLite database for change history
- optional future config files
- optional future export/import artifacts

## Intended Capabilities

- record mutations performed by the tool
- inspect prior values
- compare current value with previous snapshots
- revert a variable to a prior state
- make PATH edits easier to reason about over time

## Data Shape Direction

Likely fields per change record:

- variable name
- scope
- operation type
- previous value
- new value
- timestamp
- optional machine/user metadata relevant to the local session

## Requirements

- local-only by default
- no cloud dependency
- safe for long string values
- future-friendly for diff and rollback operations

## Non-Goals

- multi-machine synchronization
- secrets management platform behavior
- replacing source control for configuration files in general
