.PHONY: docs gen-server-protobuf gen-client-protobuf gen-load-test-protobuf

docs:
	cd docs && mdbook serve --open

gen-protobuf: gen-server-protobuf gen-client-protobuf gen-load-test-protobuf

gen-server-protobuf:
	protoc \
		--elixir_out=transform_module=DarkWorldsServer.Communication.ProtoTransform:./server/lib/dark_worlds_server/communication/ \
		--elixir_opt=package_prefix=dark_worlds_server.communication.proto \
		messages.proto

gen-client-protobuf:
	protogen --csharp_out=./  messages.proto
	mv messages.cs client/Assets/Scripts/Messages.pb.cs

gen-load-test-protobuf:
	protoc \
		--elixir_out=./server/load_test/lib/load_test/communication/ \
		--elixir_opt=package_prefix=load_test.communication.proto \
		messages.proto
