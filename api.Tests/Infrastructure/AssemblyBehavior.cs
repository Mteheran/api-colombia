using Xunit;

// Test classes run one at a time across the whole assembly.
//
// The suite shares a single seeded host (see ApiColombiaFactory), which made it fast enough that
// the MCP Streamable HTTP handshake started failing intermittently — `tools/list` would come back
// with no reply — whenever the MCP class ran alongside the rest of the suite. It is not the shared
// database (it reproduces with a dedicated factory) and not response compression (it reproduces
// with compression off); it disappears completely when classes run serially.
//
// Serial execution costs a few seconds against a suite that is still faster than it was before the
// shared host, which is a good trade for a CI that does not flake.
[assembly: CollectionBehavior(DisableTestParallelization = true)]
