.PHONY: generate-protos docs

generate-protos: 
	protoc --csharp_out=./ messages.proto
	mv Messages.cs client/Assets/Scripts/Messages.pb.cs

docs: 
	cd docs && mdbook serve --open
