#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
BGFX_PATH="$ROOT/submodules/bgfx"
TOOLS_PATH="$ROOT/tools"
NATIVE_PATH="$ROOT/runtimes"

UNAME="$(uname)"
ARCH="$(uname -m)"

case "$UNAME" in
  Darwin)
    OS_NAME="osx-${ARCH}"
    BUILD_OUTPUT_DIR="osx-${ARCH}"
    ;;
  Linux)
    OS_NAME="linux-x64"
    BUILD_OUTPUT_DIR="linux64_gcc"
    ;;
  MINGW*|MSYS*|CYGWIN*|Windows_NT)
    OS_NAME="win-x64"
    BUILD_OUTPUT_DIR="win64_vs2019"
    ;;
  *)
    echo "Unsupported OS: $UNAME" >&2
    exit 1
    ;;
esac

# Correct the BUILD_BIN_DIR path
BUILD_BIN_DIR="$BGFX_PATH/.build/$BUILD_OUTPUT_DIR"

mkdir -p "$TOOLS_PATH/$OS_NAME"
mkdir -p "$NATIVE_PATH/$OS_NAME/native"

# Replace associative arrays with plain arrays
TOOLS_KEYS=("geometrycRelease" "geometryvRelease" "shadercRelease" "texturecRelease" "texturevRelease")
TOOLS_VALUES=("geometryc" "geometryv" "shaderc" "texturec" "texturev")

NATIVES_KEYS=("libbgfx-shared-libRelease.dylib" "libbgfx-shared-libRelease.so" "bgfx.dll")
NATIVES_VALUES=("libbgfx.dylib" "libbgfx.so" "bgfx.dll")

# Debugging output
echo "Debugging Information:"
echo "BUILD_BIN_DIR: $BUILD_BIN_DIR"
echo "TOOLS_PATH: $TOOLS_PATH"
echo "NATIVE_PATH: $NATIVE_PATH"
echo "TOOLS_KEYS: ${TOOLS_KEYS[*]}"
echo "NATIVES_KEYS: ${NATIVES_KEYS[*]}"

# Copy tools
for i in "${!TOOLS_KEYS[@]}"; do
  src="${TOOLS_KEYS[$i]}"
  dest="${TOOLS_VALUES[$i]}"
  SRC_PATH="$BUILD_BIN_DIR/bin/$src"
  echo "Processing tool: $src -> $dest"
  if [ ! -f "$SRC_PATH" ]; then
    SRC_PATH=$(find "$BGFX_PATH/.build" -type f -name "$src*" | head -n1 2>/dev/null || true)
  fi
  if [ -n "$SRC_PATH" ] && [ -f "$SRC_PATH" ]; then
    cp -f "$SRC_PATH" "$TOOLS_PATH/$OS_NAME/$dest"
    chmod +x "$TOOLS_PATH/$OS_NAME/$dest" || true
    echo "Copied tool: $SRC_PATH -> $TOOLS_PATH/$OS_NAME/$dest"
  else
    echo "Warning: tool $src not found" >&2
  fi
done

# Copy native libraries
for i in "${!NATIVES_KEYS[@]}"; do
  src="${NATIVES_KEYS[$i]}"
  dest="${NATIVES_VALUES[$i]}"
  SRC_PATH="$BUILD_BIN_DIR/bin/$src"
  echo "Processing native lib: $src -> $dest"
  if [ ! -f "$SRC_PATH" ]; then
    SRC_PATH=$(find "$BGFX_PATH/.build" -type f -name "$src*" | head -n1 || true)
  fi
  if [ -f "$SRC_PATH" ]; then
    cp -f "$SRC_PATH" "$NATIVE_PATH/$OS_NAME/native/$dest"
    chmod +x "$NATIVE_PATH/$OS_NAME/native/$dest" || true
    echo "Copied native lib: $SRC_PATH -> $NATIVE_PATH/$OS_NAME/native/$dest"
  else
    echo "Warning: native lib $src not found" >&2
  fi
done

echo "bgfx copy finished. Tools: $TOOLS_PATH/$OS_NAME, Native libs: $NATIVE_PATH/$OS_NAME"
