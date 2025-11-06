# Stop on errors
$ErrorActionPreference = "Stop"

$root = Get-Location
$bgfxPath = Join-Path $root "submodules/bgfx"
$toolsPath = Join-Path $root "tools\win-x64"
$nativePath = Join-Path $root "runtimes\win-x64\native"
$buildOutput = Join-Path $bgfxPath ".build\win64_vs2022\bin"

# Define mappings
$toolsTargets = @{
    "geometrycRelease.exe"       = "geometryc.exe"
    "geometryvRelease.exe"       = "geometryv.exe"
    "shadercRelease.exe"         = "shaderc.exe"
    "texturecRelease.exe"        = "texturec.exe"
    "texturevRelease.exe"        = "texturev.exe"
}

$nativeTargets = @{
    "bgfx-shared-libRelease.dll" = "bgfx.dll"
}

# Create output directories
New-Item -ItemType Directory -Force -Path $toolsPath | Out-Null
New-Item -ItemType Directory -Force -Path $nativePath | Out-Null

# Copy tools
foreach ($srcName in $toolsTargets.Keys) {
    $src = Join-Path $buildOutput $srcName
    $dest = Join-Path $toolsPath $toolsTargets[$srcName]
    if (Test-Path $src) {
        Copy-Item $src $dest -Force
        Write-Host "Copied tool: $srcName -> $($toolsTargets[$srcName])"
    } else {
        Write-Warning "Missing tool: $srcName"
    }
}

# Copy native libraries
foreach ($srcName in $nativeTargets.Keys) {
    $src = Join-Path $buildOutput $srcName
    $dest = Join-Path $nativePath $nativeTargets[$srcName]
    if (Test-Path $src) {
        Copy-Item $src $dest -Force
        Write-Host "Copied native lib: $srcName -> $($nativeTargets[$srcName])"
    } else {
        Write-Warning "Missing native lib: $srcName"
    }
}

Write-Host "`nCopy complete. Tools: $toolsPath, Native libs: $nativePath"
