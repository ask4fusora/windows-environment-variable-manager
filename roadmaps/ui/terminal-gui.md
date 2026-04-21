# Terminal GUI

## Status

Planned.

## Goal

Provide a resilient TUI for environment variable management with efficient
navigation, clear structure, and no dependency on fragile box-drawing layouts.

## UX Direction

- Terminal.Gui-based
- Vim-like motion where it improves navigation speed
- flat design
- no Unicode box-drawing borders
- good color separation without trying to imitate a desktop GUI frame system

## Design Constraints

- avoid decorative Unicode border characters
- prefer clean spacing, contrast, and focus states
- stay readable in typical Windows terminal environments
- keep the TUI robust rather than flashy

## Functional Direction

- browse variables by scope
- inspect and edit values
- support future PATH-specific workflows
- support future history/rollback flows backed by local SQLite storage

## Requirements

- align with the existing CLI/domain model
- remain useful in terminal-first workflows
- keep interactions efficient for heavy keyboard users

## Non-Goals

- pixel-perfect imitation of native Windows GUI tools
- decorative border-heavy layouts
- requiring mouse-first interaction
