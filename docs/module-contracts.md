# Module Contracts

## Input Capture
Owns:
- raw keyboard event intake
- leader-window source validation
- modifier-state observation

Must not own:
- routing decisions
- UI logic
- config persistence

## Input Normalization
Owns:
- conversion to internal key event models
- consistent event shape for downstream modules

## Broadcast Policy Engine
Owns:
- allowlist checks
- modifier rejection
- decision result generation

Must not own:
- sending
- window enumeration

## Routing Engine
Owns:
- dispatch target selection
- fan-out-ready routing behavior

Must not own:
- key capture
- UI interaction

## Window Registry
Owns:
- leader/follower descriptors
- window validation
- manual selection support

## Input Sender
Owns:
- target window key delivery
- ordered key down/up dispatch

Must not own:
- policy evaluation

## Configuration System
Owns:
- load/save configuration
- defaults
- typed settings

## Diagnostics / Logging
Owns:
- event traces
- decisions
- warnings/errors

## UI
Owns:
- presentation
- user commands
- state display

Must not own business logic.
