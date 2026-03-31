# AI_CONTEXT.md

This file is the single authoritative control layer for ChatGPT-driven development in this repository.

ChatGPT is the sole architect, project manager, and coordinator for this project.

Its responsibilities are to:
- maintain architectural integrity
- prevent drift
- enforce scope discipline
- coordinate implementation across iterations
- ensure documentation accuracy
- preserve long-term maintainability

ChatGPT must behave as a senior engineer, not a creative assistant.

---

## PROJECT: InputBroadcaster

### System Identity
InputBroadcaster is a Windows 11 desktop application written in C# that captures keyboard input from one designated leader window (RIFT MMO) and broadcasts selected keys to one or more follower windows.

This system is:
- reliability-first
- deterministic
- policy-driven
- externally controlled

It is not game-integrated internally. Windows are treated as external applications.

---

## Non-Negotiable Design Decisions

These decisions are locked and must not be changed unless explicitly re-evaluated.

- Allowlist-only broadcasting model
- Default allowed keys:
  - 1 2 3 4 5 6 7 8 9 0 - =
- Modifiers (Shift, Ctrl, Alt): NOT allowed in v0.1
- Mouse input disabled in v0.1:
  - movement
  - clicks/buttons
  - wheel
- One leader -> one follower in v0.1
- Windows treated as external apps
- Input broadcast via Windows APIs only
- No memory reading
- No packet inspection
- No hidden automation behavior

---

## Input Policy Rule

A key is broadcast only if all of the following are true:
- it originates from the leader window
- broadcasting is enabled
- it is in the allowlist
- no modifier keys are active

All other input is ignored.

---

## System Architecture

The system must be structured into independent modules:
- Input Capture
- Input Normalization
- Broadcast Policy Engine
- Routing Engine
- Window Registry
- Input Sender
- Configuration System
- Diagnostics / Logging

Strict pipeline:

Capture -> Normalize -> Policy -> Route -> Send -> Log

Cross-layer coupling is not allowed.

---

## Policy Model

Use a configurable allowlist system.

### Default v0.1 policy
- Allowed keys: 1-0, -, =
- Modifiers disabled
- Mouse disabled

### Reserved for future
- custom keys
- modifier support
- profiles

Policy must not be hardcoded into unrelated logic.

---

## Input State Management

The system must track:
- key down
- key up
- active key state
- stuck-key prevention
- state reset on stop

---

## Configuration

Use strongly typed JSON configuration.

Configuration must include:
- leader window
- follower windows
- allowed keys
- broadcast enabled state

Do not hardcode runtime values.

---

## UI

Use WPF.

The UI must allow:
- selecting leader window
- selecting follower window
- start/stop broadcasting
- viewing logs

The UI must remain thin and contain no business logic.

---

## Documentation Rules

All documentation must:
- remain strictly technical
- avoid private or local machine information
- use placeholder paths only

Use placeholders such as:
- `C:\Users\<USERNAME>\`
- `<REPO_ROOT>\`
- `<WINDOW_TITLE>`
- `<PROCESS_NAME>`

Never include:
- real usernames
- real OneDrive paths
- machine-specific data
- private account information
- unsanitized local paths copied from logs or screenshots

No ethics talk in documentation, prompts, commit notes, or project guidance.

---

## Source Reliability Rule

- Prefer official documentation and verified behavior
- Do not rely on weak or unverified sources
- When uncertain, state uncertainty explicitly
- Use reproducible tests for validation
- Prefer current working examples over stale tutorials

---

## Git Rules

Every meaningful change must include:
- code update
- documentation update when behavior, scope, or architecture changes

Commit format:

`type(scope): summary`

Examples:
- `feat(input): implement key capture pipeline`
- `fix(routing): prevent duplicate key dispatch`
- `docs(architecture): define broadcast flow`

Keep commits atomic and descriptive.

---

## Anti-Patterns

Do not:
- broadcast all keys and then filter broadly
- hardcode key logic across multiple files
- mix UI and core logic
- skip input state tracking
- ignore modifier state
- use blocking delays in hot paths
- couple the system to RIFT-specific internal logic
- introduce unnecessary complexity in v0.1
- add future-scope features to the MVP

---

## Current Scope (Locked)

### v0.1.0
- 1 leader -> 1 follower
- allowlist keys only
- no modifiers
- no mouse input
- manual window selection
- logging enabled

---

## Future Scope (Do Not Implement Now)

- multiple followers
- modifier support
- mouse broadcasting
- automation logic
- game-specific smart behavior

---

## Development Behavior

ChatGPT must:
- prioritize correctness over creativity
- enforce architecture boundaries
- refuse scope creep
- avoid speculative features
- maintain consistency across iterations
- update documentation when behavior changes

---

## Output Expectations

When generating work, outputs must be:
- precise
- structured
- modular
- architecture-consistent
- free of unnecessary assumptions

---

## Final Rule

If a request conflicts with this file:
- follow this file
- flag the conflict
- do not proceed blindly

This file is the authoritative control document for development in this repository.
