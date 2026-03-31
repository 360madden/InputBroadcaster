# InputBroadcaster

Windows 11 WPF MVP for **one leader window -> one follower window** keyboard broadcasting.

## Current State

This repository is no longer a Node starter. The active codebase is a .NET 9 WPF solution with focused MVP scope:

- one leader window
- one follower window
- keyboard only
- allowlist only
- default keys: `1 2 3 4 5 6 7 8 9 0 - =`
- modifiers blocked in v0.1
- mouse blocked in v0.1

Implemented so far:

- core broadcast models and contracts
- allowlist policy evaluator
- raw keyboard normalization
- keyboard state tracker
- window enumeration
- in-memory window registry
- Win32 `PostMessage` sender for supported keys
- baseline xUnit coverage
- functional WPF shell for selecting leader/follower windows and managing visible broadcast state

Not implemented yet:

- live keyboard capture from the selected leader window
- end-to-end routing pipeline wiring from capture -> policy -> send
- persisted UI configuration
- multi-follower fan-out
- modifier support
- mouse broadcasting

## Solution Layout

```text
src/
  InputBroadcaster.App
  InputBroadcaster.Core
  InputBroadcaster.Input
  InputBroadcaster.Routing
  InputBroadcaster.Windows
  InputBroadcaster.Sending
  InputBroadcaster.Configuration
  InputBroadcaster.Diagnostics

tests/
  InputBroadcaster.Tests
```

## Build

Requirements:

- Windows 11
- .NET 9 SDK
- Visual Studio 2022 or newer, or the `dotnet` CLI

Restore and build:

```powershell
dotnet restore .\InputBroadcaster.sln
dotnet build .\InputBroadcaster.sln
```

Run tests:

```powershell
dotnet test .\InputBroadcaster.sln
```

Run the app:

```powershell
dotnet run --project .\src\InputBroadcaster.App\InputBroadcaster.App.csproj
```

## Practical Notes

- The WPF shell exposes honest state only. Start/Stop currently changes application state but does **not** yet mean leader-window capture is wired.
- Sender behavior is intentionally narrow and only maps the MVP key allowlist.
- The repo should be treated as an MVP under active construction, not a finished broadcaster.

## Next Development Slice

1. wire actual leader-window keyboard capture
2. feed normalized events into policy evaluation
3. send allowed events to the selected follower
4. prevent stuck keys on stop/reset
5. persist leader/follower selection and broadcast settings
