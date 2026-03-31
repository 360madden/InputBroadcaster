# Testing

## v0.1 Manual Validation Matrix

### 1. Leader Window Selection
- select a valid leader window
- verify non-leader windows do not trigger broadcast

### 2. Follower Window Selection
- select a valid follower window
- verify invalid or closed follower windows are detected

### 3. Allowed Keys
Verify broadcast for:
- 1 2 3 4 5 6 7 8 9 0 - =

### 4. Disallowed Keys
Verify no broadcast for keys outside the allowlist.

### 5. Modifiers
Verify no broadcast occurs when any modifier is active:
- Shift
- Ctrl
- Alt

### 6. Start / Stop
- start broadcasting
- verify allowed key dispatch works
- stop broadcasting
- verify no more dispatch occurs

### 7. Key State Reset
- stop while keys are pressed
- verify no stuck key state remains

### 8. Logging
Verify logs record:
- leader/follower selection
- start/stop transitions
- allowed broadcast decisions
- rejected key decisions
- invalid target conditions

## Automation Targets
As implementation matures, add automated tests for:
- policy evaluation
- config load/save
- normalization
- routing
- key state handling
