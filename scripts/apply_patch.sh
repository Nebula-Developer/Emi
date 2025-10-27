#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"
SUBMODULE_PATH="$ROOT/submodules/bgfx"
PATCH_FILE="$SCRIPT_DIR/bgfx.patch"

if [ ! -d "$SUBMODULE_PATH" ]; then
  echo "Submodule path '$SUBMODULE_PATH' not found." >&2
  exit 1
fi

if [ ! -f "$PATCH_FILE" ]; then
  echo "Patch file '$PATCH_FILE' not found." >&2
  exit 1
fi

echo "Applying patch: $PATCH_FILE -> $SUBMODULE_PATH"
pushd "$SUBMODULE_PATH" >/dev/null

if ! git apply --ignore-whitespace "$PATCH_FILE"; then
    echo "Warning: Patch failed to apply, continuing..." >&2
fi

popd >/dev/null

echo "Patch applied."
