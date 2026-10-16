# Builds the same Target.Check source twice (with and without an unrelated earlier member)
# and compares its raw IL bytes with opcode/resolved-operand sequences.
# Requires a .NET SDK able to build net472. Works on Windows PowerShell 5.1 and PowerShell 7.
$ErrorActionPreference = 'Stop'
$root = $PSScriptRoot
$out = Join-Path ([IO.Path]::GetTempPath()) ('il-token-probe-' + [Guid]::NewGuid().ToString('N'))
try {
    & dotnet build (Join-Path $root 'lib\Lib.csproj') -c Release -o (Join-Path $out 'a') "-p:BaseIntermediateOutputPath=$out\obj-a\" --nologo -v q
    if ($LASTEXITCODE -ne 0) { throw 'Build A failed.' }
    & dotnet build (Join-Path $root 'lib\Lib.csproj') -c Release -o (Join-Path $out 'b') '-p:DefineConstants=EXTRA' "-p:BaseIntermediateOutputPath=$out\obj-b\" --nologo -v q
    if ($LASTEXITCODE -ne 0) { throw 'Build B failed.' }
    & dotnet build (Join-Path $root 'compare\Compare.csproj') -c Release -o (Join-Path $out 'cmp') "-p:BaseIntermediateOutputPath=$out\obj-cmp\" --nologo -v q
    if ($LASTEXITCODE -ne 0) { throw 'Compare build failed.' }
    & (Join-Path $out 'cmp\Compare.exe') (Join-Path $out 'a\Lib.dll') (Join-Path $out 'b\Lib.dll')
    if ($LASTEXITCODE -ne 0) { throw 'Compare failed.' }
}
finally { Remove-Item -LiteralPath $out -Recurse -Force -ErrorAction SilentlyContinue }
