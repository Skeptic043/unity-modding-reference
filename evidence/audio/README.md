# Reproduce the standalone Media Foundation comparison

[Spike.cs](Spike.cs) and [ComSpike.cs](ComSpike.cs) provide the manual-vtable and `ComImport` paths. A comparison needs:

- an independently installed Unity 2019.1.10f1 MonoBleedingEdge runtime and `mcs`
- FFmpeg
- Windows Media Foundation
- a new scratch directory

After assigning absolute `$mono`, `$mcs` and `$ffmpeg` paths, run this from the evidence folder:

```powershell
$probeOutput = Join-Path $env:TEMP 'unity-mf-probe'
New-Item -ItemType Directory -Path $probeOutput -Force -ErrorAction Stop | Out-Null

$fixture = Join-Path $probeOutput 'aac-stereo.m4a'
& $ffmpeg -hide_banner -loglevel error -f lavfi -i 'sine=frequency=330:sample_rate=44100:duration=3' -ac 2 -c:a aac -b:a 128k -map_metadata -1 $fixture
if ($LASTEXITCODE -ne 0) { throw 'Fixture generation failed' }

& $mono $mcs "-out:$probeOutput\Spike.exe" '.\Spike.cs'
if ($LASTEXITCODE -ne 0) { throw 'Spike compile failed' }

& $mono $mcs "-out:$probeOutput\ComSpike.exe" '.\ComSpike.cs'
if ($LASTEXITCODE -ne 0) { throw 'ComSpike compile failed' }

& $mono "$probeOutput\Spike.exe" $fixture
$manualExit = $LASTEXITCODE
& $mono "$probeOutput\ComSpike.exe" $fixture
$comImportExit = $LASTEXITCODE

"manual-vtable exit=$manualExit"
"ComImport exit=$comImportExit"
```

This creates a new fixture. Preserve stdout, stderr, exit codes, input hashes and the actual process architecture. A zero exit code is not enough because both sources can print caught errors.

The two programs differ in COM setup and output configuration as well as invocation mechanism. Even if a fresh run matches the historical result, it does not isolate the crash cause.
