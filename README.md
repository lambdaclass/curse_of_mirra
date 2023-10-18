# Curse of Myrra

## Requirements
- Rust:
    - https://www.rust-lang.org/tools/install
- Elixir and Erlang:
    - https://thinkingelixir.com/install-elixir-using-asdf/
    - Erlang/OTP 26
    - Elixir 1.15.4
- Unity
- Docker

## Setup project
Make sure Docker is running.
```
git clone https://github.com/lambdaclass/curse_of_myrra
cd curse_of_myrra/server
make db
make setup
```

## Run backend
Make sure Docker is running.
```
make start
```

## Useful commands
```
make tests
```
Will run elixir and rust tests

```
make format
```
Will format elixir and rust code.`
```
make prepush
```
Will format you code, runs credo check and tests.

## Documentation
You can find our documentation [here](https://docs.curseofmyrra.com/) or run it locally.

For that you have to install:
```
cargo install mdbook
cargo install mdbook-mermaid
```

Then run:
```
make docs
```
Open:
[http://localhost:3000/](http://localhost:3000/ios_builds.html)

Some useful links
- [Backend architecture](https://docs.curseofmyrra.com/backend_architecture.html)
- [Message protocol](https://docs.curseofmyrra.com/message_protocol.html)
- [Android build](https://docs.curseofmyrra.com/android_builds.html)
- [IOs builds](https://docs.curseofmyrra.com/ios_builds.html)
