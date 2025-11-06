# Stop on errors
$ErrorActionPreference = "Stop"

$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path

Write-Host "=== Step 1: Compile bgfx ==="
& "$scriptDir\bgfx-compile.ps1"

Write-Host "`n=== Step 2: Copy tools and native libraries ==="
& "$scriptDir\bgfx-copy.ps1"

Write-Host "`nAll steps finished."
