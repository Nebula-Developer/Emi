#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
BGFX_PATH="$ROOT/submodules/bgfx"
TOOLS_PATH="$ROOT/tools"
NATIVE_PATH="$ROOT/runtimes"

UNAME="$(uname)"
case "$UNAME" in
  Darwin)
    OS_NAME="osx"
    BUILD_OUTPUT_DIR="osx-arm64"
    ;;
  Linux)
    OS_NAME="linux-x64"
    BUILD_OUTPUT_DIR="linux64_gcc"
    ;;
  MINGW*|MSYS*|CYGWIN*|Windows_NT)
    OS_NAME="win"
    BUILD_OUTPUT_DIR="win64_vs2019"
    ;;
  *)
    echo "Unsupported OS: $UNAME" >&2
    exit 1
    ;;
esac

BUILD_BIN_DIR="$BGFX_PATH/.build/$BUILD_OUTPUT_DIR/bin"

mkdir -p "$TOOLS_PATH/$OS_NAME"
mkdir -p "$NATIVE_PATH/$OS_NAME/native"

declare -A TOOLS=(
  ["geometrycRelease"]="geometryc"
  ["geometryvRelease"]="geometryv"
  ["shadercRelease"]="shaderc"
  ["texturecRelease"]="texturec"
  ["texturevRelease"]="texturev"
)

declare -A NATIVES=(
  ["libbgfx-shared-libRelease.dylib"]="libbgfx.dylib"
  ["libbgfx-shared-libRelease.so"]="libbgfx.so"
  ["bgfx.dll"]="bgfx.dll"
)

# Copy tools
for src in "${!TOOLS[@]}"; do
  dest="${TOOLS[$src]}"
  SRC_PATH="$BUILD_BIN_DIR/$src"
  if [ ! -f "$SRC_PATH" ]; then
    SRC_PATH=$(find "$BGFX_PATH/.build" -type f -name "$src*" | head -n1 || true)
  fi
  if [ -f "$SRC_PATH" ]; then
    cp -f "$SRC_PATH" "$TOOLS_PATH/$OS_NAME/$dest"
    chmod +x "$TOOLS_PATH/$OS_NAME/$dest" || true
    echo "Copied tool: $SRC_PATH -> $TOOLS_PATH/$OS_NAME/$dest"
  else
    echo "Warning: tool $src not found" >&2
  fi
done

# Copy native libraries
for src in "${!NATIVES[@]}"; do
  dest="${NATIVES[$src]}"
  SRC_PATH="$BUILD_BIN_DIR/$src"
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
