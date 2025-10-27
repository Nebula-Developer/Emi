$ErrorActionPreference = "Stop"

$root = Get-Location
$submodulePath = Join-Path $root "submodules/bgfx"
$patchFile = Join-Path $root "scripts/bgfx.patch"

if (-not (Test-Path $submodulePath)) {
    Write-Error "Submodule path '$submodulePath' not found."
    exit 1
}

if (-not (Test-Path $patchFile)) {
    Write-Error "Patch file '$patchFile' not found."
    exit 1
}

Push-Location $submodulePath

try {
    git apply --ignore-whitespace $patchFile
}
finally {
    Pop-Location
}
