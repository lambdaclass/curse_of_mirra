# Dark Worlds Server

Elixir + Rust backend for Dark Worlds.

## Requirements

- [Elixir](https://elixir-lang.org/)
- [Rust](https://www.rust-lang.org/tools/install)

## Running locally

Install dependencies and compile the project with

```
make setup
```

Start the Docker database with

```
make db
```

Then run the server with

```
make run
```
This will setup a server listening through a websocket on port `4000`, on the `/matchmaking` path. This server handles the game state, consisting of a number of players scattered on a grid. By default there are three players on a `100x100` grid.

## Tests

Run tests with

```
make tests
```
