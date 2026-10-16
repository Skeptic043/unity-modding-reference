param(
    [Parameter(Mandatory)][string]$HarmonyDll,
    [Parameter(Mandatory)][string]$CecilDll
)

$ErrorActionPreference = 'Stop'
foreach ($path in @($HarmonyDll, $CecilDll)) {
    if (-not (Test-Path -LiteralPath $path -PathType Leaf)) {
        throw "Assembly not found: $path"
    }
}
Add-Type -Path $CecilDll
$assembly = [Mono.Cecil.AssemblyDefinition]::ReadAssembly($HarmonyDll)
try {
    "HARMONY_SHA256 $((Get-FileHash -LiteralPath $HarmonyDll -Algorithm SHA256).Hash)"
    $type = $assembly.MainModule.Types | Where-Object FullName -eq 'HarmonyLib.PatchClassProcessor'
    if ($null -eq $type) { throw 'HarmonyLib.PatchClassProcessor not found.' }
    foreach ($methodName in @('Patch', 'GetBulkMethods')) {
        $method = $type.Methods | Where-Object { $_.Name -eq $methodName -and $_.HasBody } |
            Select-Object -First 1
        if ($null -eq $method) { throw "PatchClassProcessor.$methodName body not found." }
        "METHOD $($method.FullName)"
        $instructions = @($method.Body.Instructions)
        $indexes = [Collections.Generic.HashSet[int]]::new()
        for ($i = 0; $i -lt $instructions.Count; $i++) {
            $operand = [string]$instructions[$i].Operand
            if ($methodName -eq 'Patch' -and $i -lt 34 -or
                $operand -match '::RunMethod<HarmonyLib.HarmonyTargetMethod,' -or
                $operand -match '::GetBulkMethods\(\)') {
                for ($j = [Math]::Max(0, $i - 3); $j -le [Math]::Min($instructions.Count - 1, $i + 3); $j++) {
                    [void]$indexes.Add($j)
                }
            }
        }
        if ($indexes.Count -eq 0) { throw "Expected hook names absent from $methodName." }
        foreach ($index in ($indexes | Sort-Object)) { $instructions[$index].ToString() }
    }
} finally {
    $assembly.Dispose()
}
