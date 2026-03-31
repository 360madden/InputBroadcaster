# InputBroadcaster Architecture

## Purpose
InputBroadcaster is a Windows 11 WPF application that captures keyboard input from one designated leader window and rebroadcasts allowed keys to one designated follower window.

v0.1.0 is intentionally narrow:
- 1 leader -> 1 follower
- keyboard only
- allowlist only
- no modifiers
- no mouse input

## Architectural Principles
- Reliability first
- Deterministic routing
- Strong separation of concerns
- No game-specific internals
- No hidden automation
- No runtime policy hardcoding outside the policy module

## Mandatory Pipeline
Capture -> Normalize -> Policy -> Route -> Send -> Log

## Modules

### 1. InputBroadcaster.App
Purpose:
- WPF shell
- dependency injection composition root
- view models
- application startup and shutdown

Must not contain:
- routing logic
- policy logic
- Win32 input logic

### 2. InputBroadcaster.Core
Purpose:
- shared contracts
- enums
- immutable models
- common result types
- key event abstractions

Example ownership:
- BroadcastKeyEvent
- WindowDescriptor
- BroadcastDecision
- BroadcastPolicy
- service interfaces

### 3. InputBroadcaster.Input
Purpose:
- input capture from leader window
- key state tracking
- normalization into internal key events

Responsibilities:
- detect key down / key up
- reject events from non-leader windows
- track active modifier state
- expose normalized events to the pipeline

### 4. InputBroadcaster.Routing
Purpose:
- policy evaluation
- routing decisions
- fan-out-safe design for future expansion

Responsibilities:
- check whether key is allowed
- reject input when modifiers are active
- route to enabled target windows
- return decision objects for logging

### 5. InputBroadcaster.Windows
Purpose:
- window discovery
- window metadata
- leader/follower selection support
- target validation

Responsibilities:
- enumerate candidate windows
- store HWND plus identifying metadata
- validate target still exists

### 6. InputBroadcaster.Sending
Purpose:
- abstracted key delivery to target window

Responsibilities:
- send key down / key up in correct order
- keep sender strategy isolated behind interfaces
- support future alternate send strategies without redesign

### 7. InputBroadcaster.Configuration
Purpose:
- strongly typed JSON configuration
- load/save defaults
- schema-safe settings evolution

### 8. InputBroadcaster.Diagnostics
Purpose:
- structured logs
- broadcast decisions
- warning/error output
- troubleshooting support

### 9. InputBroadcaster.Tests
Purpose:
- policy tests
- configuration tests
- normalization tests
- routing tests

## v0.1 Policy Model
Default allowlist:
- 1 2 3 4 5 6 7 8 9 0 - =

Rules:
- keyboard only
- no mouse
- no modifiers
- all non-allowlisted keys ignored
- all modified keys ignored

## Input State Rules
The system must track:
- key down
- key up
- currently active keys
- currently active modifiers
- stop/reset cleanup to prevent stuck keys

## Window Model
v0.1 window model:
- one leader window
- one follower window
- manual selection only

Future scaling requirement:
- routing must be list-capable internally even though v0.1 uses one follower
- no redesign should be required to support multiple follower windows later

## Configuration Model
Configuration must include:
- leader window descriptor
- follower window descriptors
- allowed keys
- broadcast enabled flag
- diagnostics settings as needed

Configuration values must not be hardcoded in unrelated modules.

## UI Boundaries
UI responsibilities:
- select leader window
- select follower window
- start broadcasting
- stop broadcasting
- show logs/state

UI must remain thin.
All business logic belongs in services outside the UI layer.

## Anti-Drift Rules
- Do not broadcast all keys then filter broadly
- Do not hardcode policy into sender/capture/UI
- Do not introduce modifier support in v0.1
- Do not add mouse features in v0.1
- Do not couple logic to RIFT internals
- Do not mix window enumeration with routing logic

## Initial Project Layout
```text
InputBroadcaster/
  AI_CONTEXT.md
  README.md
  ARCHITECTURE.md
  ROADMAP.md
  CHANGELOG.md
  CONTRIBUTING.md
  TESTING.md
  docs/
    project-brief.md
    current-scope.md
    out-of-scope.md
    module-contracts.md
  src/
    InputBroadcaster.App/
    InputBroadcaster.Core/
    InputBroadcaster.Input/
    InputBroadcaster.Routing/
    InputBroadcaster.Windows/
    InputBroadcaster.Sending/
    InputBroadcaster.Configuration/
    InputBroadcaster.Diagnostics/
  tests/
    InputBroadcaster.Tests/
```

## Current Status
This architecture file defines the intended v0.1 baseline. Implementation must follow this structure and must update this file if architectural boundaries change.
