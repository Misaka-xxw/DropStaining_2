[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$workspaceRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$gitDirectory = Join-Path $workspaceRoot '.git'

if (-not (Test-Path -LiteralPath $gitDirectory)) {
    throw "Git workspace was not found at $workspaceRoot."
}

$tracked = @(& git -C $workspaceRoot ls-files -- 'data/logs/*.jsonl' 'data/machine-executor.lock')
if ($LASTEXITCODE -ne 0) {
    throw 'git ls-files failed.'
}

if ($tracked.Count -gt 0) {
    throw "Runtime files are still tracked: $($tracked -join ', ')"
}

$ignoreProbe = 'data/logs/runtime-git-policy-probe.jsonl'
$ignored = @(& git -C $workspaceRoot check-ignore --no-index -- $ignoreProbe 'data/machine-executor.lock')
if ($LASTEXITCODE -ne 0 -or $ignored.Count -ne 2) {
    throw 'Runtime log or lease ignore rule is missing.'
}

$status = @(& git -C $workspaceRoot status --porcelain --untracked-files=all -- 'data/logs' 'data/machine-executor.lock')
if ($LASTEXITCODE -ne 0) {
    throw 'git status failed.'
}

$worktreeChanges = @($status | Where-Object {
    $_.StartsWith('??', [StringComparison]::Ordinal) -or ($_.Length -ge 2 -and $_[1] -ne ' ')
})
if ($worktreeChanges.Count -gt 0) {
    throw "Runtime files still pollute the working tree: $($worktreeChanges -join ', ')"
}

Write-Output 'PASS runtime files are local, ignored, and untracked.'
