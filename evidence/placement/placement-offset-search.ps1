$ErrorActionPreference = 'Stop'
function Find-ClearPosition([int]$Origin) {
    foreach ($distance in 1..20) {
        if ($Origin + $distance -ge 10) { return $Origin + $distance }
    }
    throw 'No valid candidate.'
}
$baseline = Find-ClearPosition 0
$shiftedSearch = Find-ClearPosition 4
$translatedCandidate = $baseline + 4
if ($baseline -ne 10) { throw 'Baseline did not select the clearance boundary.' }
if ($shiftedSearch -ne $baseline) { throw 'Probe did not reproduce offset cancellation.' }
if ($translatedCandidate - $baseline -ne 4 -or $translatedCandidate -lt 10) { throw 'Candidate translation lost displacement or clearance.' }
'3 checks passed: baseline 10, translated-origin search 10, translated candidate 14.'
