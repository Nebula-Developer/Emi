#!/usr/bin/env bash
set -euo pipefail

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

echo "=== Step 1: Build bgfx ==="
"$SCRIPT_DIR/bgfx-compile.sh"

echo "=== Step 2: Copy tools and native libraries ==="
"$SCRIPT_DIR/bgfx-copy.sh"

echo "All steps finished."
