.PHONY: gen-client-protobuf gen-protobuf docs

gen-protobuf: gen-client-protobuf

gen-client-protobuf:
	protoc --csharp_out=./  communication/messages.proto
	mv Messages.cs client/Assets/Scripts/Messages.pb.cs

docs: 
	cd docs && mdbook serve --open
