SHELL := /bin/sh

include ../../config.mk

.PHONY : dotnet-examples clean

dotnet-examples:
	dotnet build Example.csproj -o $(OUT_DIR)

clean:
	rm -rf $(OUT_DIR)/example*