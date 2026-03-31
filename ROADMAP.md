# InputBroadcaster Roadmap

## v0.1.0 - MVP
Goal: reliable one-leader to one-follower keyboard broadcasting.

### Scope
- WPF desktop app
- manual leader window selection
- manual follower window selection
- allowlist-only broadcasting
- default allowed keys: 1 2 3 4 5 6 7 8 9 0 - =
- modifiers blocked
- mouse blocked
- start/stop broadcasting
- diagnostics/logging
- strongly typed JSON configuration

### Success Criteria
- allowed keys broadcast correctly
- disallowed keys ignored
- modified keys ignored
- no stuck keys after stop
- routing decisions logged
- leader/follower windows validated

## v0.1.1
Goal: harden the MVP.

### Focus
- improve logging clarity
- configuration validation
- sender reliability improvements
- UI polish without architecture changes
- test coverage expansion

## v0.2.0
Goal: controlled expansion without redesign.

### Planned
- multiple follower windows
- custom allowlist editing
- saved profiles
- better window rebinding support

## Deferred Beyond v0.2.0
Not for current implementation.

- modifier support
- mouse broadcasting
- automation logic
- game-specific smart behavior
- macro systems
- memory reading
- packet inspection

## Roadmap Rules
- Do not pull future-scope items into v0.1.x
- Update this file when scope changes materially
- Keep entries technical and implementation-focused
