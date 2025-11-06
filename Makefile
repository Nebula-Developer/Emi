all: init build

init:
	git submodule update --init --recursive

update:
	git submodule update --remote --merge

build:
	./scripts/build-bgfx.sh
