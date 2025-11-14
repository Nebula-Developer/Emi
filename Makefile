all: init build

init:
	git submodule update --init --recursive

update:
	git submodule update --remote --merge

ifeq ($(OS),Windows_NT)
build:
	powershell.exe -File ./scripts/build-bgfx.ps1
else
build:
	./scripts/build-bgfx.sh
endif