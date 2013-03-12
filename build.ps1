$ErrorActionPreference = "Stop"

Write-Verbose "Getting MSBuild location."

$_msBuild = Join-Path `
                ([System.Runtime.InteropServices.RuntimeEnvironment]::GetRuntimeDirectory()) `
                "MSBuild.exe"

if ($_msBuild -like "*Framework64*") {
    Write-Verbose "You have a 64-bit .NET Framework. Windows Phone 8 hates you."
    $_msBuild = $_msBuild.Replace("Framework64", "Framework")
}

if (!(Test-Path $_msBuild)) {
    throw "Could not locate MSBuild!"
}

Push-Location $PSScriptRoot
try {
    & $_msBuild .\build.proj
}
finally {
    Pop-Location
}