.PHONY: docs gen-server-protobuf gen-client-protobuf gen-load-test-protobuf

docs:
	cd docs && mdbook serve --open

gen-protobuf: gen-server-protobuf gen-client-protobuf gen-load-test-protobuf

gen-server-protobuf:
	protoc \
		--elixir_out=transform_module=DarkWorldsServer.Communication.ProtoTransform:./server/lib/dark_worlds_server/ \
		--elixir_opt=package_prefix=dark_worlds_server.communication.proto \
		communication/messages.proto

	protoc \
		--elixir_out=transform_module=DarkWorldsServer.Communication.ProtoTransform:./server/lib/dark_worlds_server/ \
		--elixir_opt=package_prefix=dark_worlds_server.communication.proto \
		communication/config.proto

	protoc \
		--elixir_out=transform_module=DarkWorldsServer.Communication.ProtoTransform:./server/lib/dark_worlds_server/ \
		--elixir_opt=package_prefix=dark_worlds_server.communication.proto \
		communication/action.proto

	
# Elixir's protobuf lib does not add a new line nor formats the output file
# so we do it here with a format:
	mix format "./server/lib/dark_worlds_server/communication/messages.pb.ex"

gen-client-protobuf:
	protoc --csharp_out=./  communication/messages.proto
	mv Messages.cs client/Assets/Scripts/Messages.pb.cs

	protoc --csharp_out=./  communication/config.proto
	mv Config.cs client/Assets/Scripts/Config.pb.cs

	protoc --csharp_out=./  communication/action.proto
	mv Action.cs client/Assets/Scripts/Action.pb.cs


gen-load-test-protobuf:
	protoc \
		--elixir_out=./server/load_test/lib/load_test/communication/ \
		--elixir_opt=package_prefix=load_test.communication.proto \
		communication/messages.proto
# Elixir's protobuf lib does not add a new line nor formats the output file
# so we do it here with a format:
	mix format "./server/load_test/**/*.{ex,exs}"
