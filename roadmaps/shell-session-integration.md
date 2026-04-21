# Shell Session Integration

## Goal

Update the current shell session after `set` and `delete` commands.

## Constraint

The CLI runs as a child process and cannot directly mutate the parent shell's
environment block.

## Current State

- OS-level `User` and `Machine` environment variables are updated correctly.
- The CLI also updates its own process environment for internal consistency.
- Existing shell sessions are not updated automatically.

## Intended Direction

Add shell integration so the current shell session can apply the same change
immediately after the CLI succeeds.

## Proposed Approach

- Add an `--emit-shell` mode that outputs shell-specific patch commands.
- Add thin shell wrappers that invoke the CLI and evaluate the emitted patch on
  success.

## Likely Shell Targets

### Nushell

- Set: `load-env { FOO: "Bar" }`
- Delete: `hide-env FOO`

### PowerShell

- Set: `$env:FOO = "Bar"`
- Delete: `Remove-Item Env:FOO`

### cmd.exe

- Set: `set FOO=Bar`
- Delete: `set FOO=`

## Non-Goals

- Mutating arbitrary already-running processes.
- Forcing all applications to reload environment variables.
- Rewriting the parent shell environment directly from the standalone CLI.
