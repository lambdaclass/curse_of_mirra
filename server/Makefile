.PHONY: setup run tests elixir-tests rust-tests db shell credo

setup: dependencies
	mix deps.compile
	mix setup

dependencies:
	mix deps.get

db:
	docker compose up -d

stop:
	docker compose down

start: db dependencies run

run:
	mix assets.build
	iex -S mix phx.server

tests: elixir-tests

elixir-tests:
	mix test

shell:
	iex -S mix run --no-start --no-halt

credo:
	mix credo --strict

format:
	mix format
	cd load_test && mix format

prepush: format credo tests
