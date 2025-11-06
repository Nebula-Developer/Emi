#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
BGFX_PATH="$ROOT/submodules/bgfx"

if [ ! -d "$BGFX_PATH" ]; then
  echo "submodules/bgfx not found. Run 'git submodule update --init --recursive' first." >&2
  exit 1
fi

UNAME="$(uname)"
case "$UNAME" in
  Darwin)
    TARGET="osx-arm64-release"
    ;;
  Linux)
    if command -v clang >/dev/null 2>&1; then
      TARGET="linux-clang-release64"
    else
      TARGET="linux-gcc-release64"
    fi
    ;;
  MINGW*|MSYS*|CYGWIN*|Windows_NT)
    TARGET="windows-release"
    ;;
  *)
    echo "Unsupported OS: $UNAME" >&2
    exit 1
    ;;
esac

echo "Building bgfx with make target: $TARGET"
pushd "$BGFX_PATH" >/dev/null

make "$TARGET"
make "idl"

popd >/dev/null
echo "bgfx build complete."
