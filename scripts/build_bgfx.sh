#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
BGFX_PATH="$ROOT/submodules/bgfx"
TOOLS_PATH="$ROOT/tools"

if [ ! -d "$BGFX_PATH" ]; then
  echo "submodules/bgfx not found. Run 'git submodule update --init --recursive' first." >&2
  exit 1
fi

echo "Running apply_patch.sh..."
"$SCRIPT_DIR/apply_patch.sh"

UNAME="$(uname)"
case "$UNAME" in
  Darwin)
    OS_NAME="darwin"
    ARCH="$(uname -m)"
    if [ "$ARCH" = "arm64" ]; then
      TARGET="osx-arm64-release"
    else
      echo "Warning: macOS architecture '$ARCH' not specifically supported by makefile; attempting osx-arm64-release." >&2
      TARGET="osx-arm64-release"
    fi
    ;;
  Linux)
    OS_NAME="linux"

    if command -v clang >/dev/null 2>&1; then
      TARGET="linux-clang-release64"
    else
      TARGET="linux-gcc-release64"
    fi
    ;;
  *)
    echo "Unsupported OS: $UNAME" >&2
    exit 1
    ;;
esac

echo "Building bgfx with make target: $TARGET"
pushd "$BGFX_PATH" >/dev/null

if ! make "$TARGET"; then
  echo "bgfx make target failed: $TARGET" >&2
  popd >/dev/null
  exit 1
fi

if [ "$UNAME" = "Darwin" ]; then
  BUILD_OUTPUT_DIR="osx-arm64"
elif [ "$UNAME" = "Linux" ]; then
  BUILD_OUTPUT_DIR="linux64_gcc"
else
  BUILD_OUTPUT_DIR=""
fi

BUILD_BIN_DIR="$BGFX_PATH/.build/$BUILD_OUTPUT_DIR/bin"

mkdir -p "$TOOLS_PATH"

mappings=(
  "geometrycRelease:geometryc"
  "geometryvRelease:geometryv"
  "shadercRelease:shaderc"
  "texturecRelease:texturec"
  "texturevRelease:texturev"
  "libbgfx-shared-libRelease.dylib:bgfx.dll"
  "libbgfx-shared-libRelease.so:bgfx.dll"
)

copied_any=0

for mapping in "${mappings[@]}"; do
  srcname="${mapping%%:*}"
  destname="${mapping#*:}"
  srcpath="$BUILD_BIN_DIR/$srcname"

  if [ -f "$srcpath" ]; then
    cp -f "$srcpath" "$TOOLS_PATH/$destname"
    chmod +x "$TOOLS_PATH/$destname" || true
    echo "Copied: $srcpath -> $TOOLS_PATH/$destname"
    copied_any=1
    continue
  fi

  FOUND=$(find "$BGFX_PATH/.build" -type f -name "$srcname*" 2>/dev/null | head -n 1 || true)
  if [ -n "$FOUND" ]; then
    cp -f "$FOUND" "$TOOLS_PATH/$destname"
    chmod +x "$TOOLS_PATH/$destname" || true
    echo "Copied found: $FOUND -> $TOOLS_PATH/$destname"
    copied_any=1
    continue
  fi

  echo "Warning: built artifact for '$srcname' not found under $BUILD_BIN_DIR" >&2
done

if [ "$copied_any" -eq 0 ]; then
  echo "No built tools were copied. You may need to inspect the build output under $BGFX_PATH/.build" >&2
fi

popd >/dev/null

echo "bgfx build finished. Tools copied to: $TOOLS_PATH"
