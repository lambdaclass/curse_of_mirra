.PHONY: setup run elixir-tests rust-tests db shell

setup:
	mix deps.get
	mix deps.compile
	mix setup

db:
	docker compose up -d

run:
	mix assets.build
	iex -S mix phx.server

tests: elixir-tests rust-tests

elixir-tests:
	mix test

rust-tests:
	cargo test --manifest-path native/gamestate/Cargo.toml

shell:
	iex -S mix run --no-start --no-halt

generate-protobuf:
	protoc \
		--elixir_out=transform_module=DarkWorldsServer.Communication.ProtoTransform:./lib/dark_worlds_server/communication/ \
		--elixir_opt=package_prefix=dark_worlds_server.communication.proto \
		messages.proto
