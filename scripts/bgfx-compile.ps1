# Stop on errors
$ErrorActionPreference = "Stop"

Write-Host "=== Compiling bgfx (VS2022 Release x64) ===`n"

# Detect Visual Studio installation
$vswhere = "${env:ProgramFiles(x86)}\Microsoft Visual Studio\Installer\vswhere.exe"
if (-not (Test-Path $vswhere)) {
    Write-Error "vswhere.exe not found. Please install Visual Studio 2022."
    exit 1
}

$vsPath = & $vswhere -latest -products * -property installationPath
$vsCmd = Join-Path $vsPath "Common7\Tools\VsDevCmd.bat"

if (-not (Test-Path $vsCmd)) {
    Write-Error "VsDevCmd.bat not found at $vsCmd"
    exit 1
}

$root = Get-Location
$bgfxPath = Join-Path $root "submodules/bgfx"

if (-not (Test-Path $bgfxPath)) {
    Write-Error "submodules/bgfx not found. Run 'git submodule update --init --recursive' first."
    exit 1
}

$cmdFile = Join-Path $env:TEMP "bgfx_build.cmd"
$cmdContent = @"
call "$vsCmd"
cd /d "$bgfxPath"
make vs2022-release64
"@

Set-Content -Path $cmdFile -Value $cmdContent -Encoding ASCII

Write-Host "Running build..."
cmd /c "`"$cmdFile`""

if ($LASTEXITCODE -ne 0) {
    Write-Error "bgfx build failed."
    exit 1
}

Write-Host "bgfx compile complete."
