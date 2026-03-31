# Contributing

## Workflow Rules
- Keep changes scoped and atomic
- Update documentation when behavior, scope, or architecture changes
- Follow AI_CONTEXT.md
- Do not add future-scope features to v0.1

## Commit Format
Use:

`type(scope): summary`

Examples:
- `feat(input): add normalized key event model`
- `fix(sending): prevent duplicate key up dispatch`
- `docs(testing): add modifier rejection cases`

## Required Before Push
- project builds
- tests pass
- docs updated if needed
- no private local paths or usernames in committed files

## Privacy Rules
Use placeholder values only.
Do not commit real local machine paths, usernames, or unsanitized logs.

## Scope Rules
v0.1 allows only:
- one leader window
- one follower window
- keyboard allowlist broadcasting
- no modifiers
- no mouse input

Anything outside this requires explicit roadmap advancement.
