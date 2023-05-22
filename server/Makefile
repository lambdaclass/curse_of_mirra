.PHONY: setup run tests elixir-tests rust-tests db shell credo

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
	mix rust_tests

shell:
	iex -S mix run --no-start --no-halt

credo:
	mix credo --strict
