# Builds and runs the probe against a supplied 0Harmony.dll (for example BepInEx 5.4.23.5's BepInEx/core/0Harmony.dll).
# Requires a .NET SDK able to build net472 and a Windows .NET Framework host. Works on Windows PowerShell 5.1 and PowerShell 7.
param([Parameter(Mandatory = $true)][string]$HarmonyPath)
$ErrorActionPreference = 'Stop'
$out = Join-Path ([IO.Path]::GetTempPath()) ('harmonyx-prefix-args-' + [Guid]::NewGuid().ToString('N'))
try {
    Write-Output ('0Harmony.dll SHA256 ' + (Get-FileHash -LiteralPath $HarmonyPath -Algorithm SHA256).Hash)
    & dotnet build (Join-Path $PSScriptRoot 'Probe.csproj') -c Release -o $out "-p:HarmonyPath=$HarmonyPath" "-p:BaseIntermediateOutputPath=$out\obj\" --nologo -v q
    if ($LASTEXITCODE -ne 0) { throw 'Probe build failed.' }
    & (Join-Path $out 'Probe.exe')
    if ($LASTEXITCODE -ne 0) { throw 'Probe failed.' }
}
finally { Remove-Item -LiteralPath $out -Recurse -Force -ErrorAction SilentlyContinue }
