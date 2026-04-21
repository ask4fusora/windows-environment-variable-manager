# Shell Session Integration

## Status

Planned.

## Goal

Update the current shell session after `set` and `delete` commands.

## Constraint

The CLI runs as a child process and cannot directly mutate the parent shell's
environment block.

## Current State

- OS-level `User` and `Machine` environment variables are updated correctly.
- The tool also updates its own process environment for internal consistency.
- Existing shell sessions are not updated automatically.

## Intended Direction

Add shell integration so the current shell session can apply the same change
immediately after the CLI succeeds.

## Proposed Approach

- add an `--emit-shell` mode that outputs shell-specific patch commands
- add thin shell wrappers that invoke the CLI and evaluate the emitted patch on
  success

## Likely Shell Targets

### Nushell

- set: `load-env { FOO: "Bar" }`
- delete: `hide-env FOO`

### PowerShell

- set: `$env:FOO = "Bar"`
- delete: `Remove-Item Env:FOO`

### cmd.exe

- set: `set FOO=Bar`
- delete: `set FOO=`

## Requirements

- do not claim to update the parent shell from the standalone executable
- make shell integration explicit and opt-in
- keep the base CLI useful even without shell integration
- preserve exact scope validation and existing mutation behavior

## Non-Goals

- mutating arbitrary already-running processes
- forcing all applications to reload environment variables
- rewriting the parent shell environment directly from the standalone CLI
