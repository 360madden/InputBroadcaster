# InputBroadcaster

Starter project scaffold for InputBroadcaster with GitHub connection.

## Purpose
- Provide a minimal reproducible project
- Demonstrate correct handling for `clearMarks` behavior (Performance API vs MGT)

## Usage
1. Install dependencies: `npm install`
2. Run: `npm start`

## Notes
If you see `mgt.clearMarks is not a function`, avoid calling it directly on MGT instance; use `performance.clearMarks` instead, or use object-safe guard.

