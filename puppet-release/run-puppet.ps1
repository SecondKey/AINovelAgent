$ErrorActionPreference = 'Stop'

# Resolve puppet CLI path (prefer absolute; fallback to PATH)
$puppetExe = Join-Path $HOME ".dotnet\tools\puppet.exe"
if (-not (Test-Path $puppetExe)) {
    $puppetExe = "puppet"
}

# Optional: predefined answers for interactive prompts
$inputsPath = Join-Path $PSScriptRoot 'inputs.txt'
if (-not (Test-Path $inputsPath)) {
    $inputsPath = $null
}

# Temp files for redirection
$guid = [Guid]::NewGuid().ToString('N')
$outFile = Join-Path $env:TEMP ("puppet_out_" + $guid + ".log")
$errFile = Join-Path $env:TEMP ("puppet_err_" + $guid + ".log")

try {
    $startParams = @{
        FilePath = $puppetExe
        RedirectStandardOutput = $outFile
        RedirectStandardError  = $errFile
        NoNewWindow = $true
        PassThru = $true
    }
    if ($inputsPath) {
        $startParams['RedirectStandardInput'] = $inputsPath
    }

    $proc = Start-Process @startParams
    $proc.WaitForExit()
    $exitCode = $proc.ExitCode

    $output = ''
    if (Test-Path $outFile) { $output = Get-Content $outFile -Raw }
    $errorOut = ''
    if (Test-Path $errFile) { $errorOut = Get-Content $errFile -Raw }

    # Echo outputs for callers
    if ($output) { Write-Output $output }
    if ($errorOut) { Write-Output $errorOut }

    # Parse and surface result line
    $resultLine = ($output -split "`r?`n") | Where-Object { $_ -match '^\s*result\b' } | Select-Object -First 1
    if (-not $resultLine) {
        $resultLine = "result: " + ($(if ($exitCode -eq 0) { 'success' } else { 'failed' }))
    }
    Write-Output $resultLine

    exit $exitCode
}
catch {
    Write-Error $_
    Write-Output 'result: failed'
    exit 1
}
finally {
    if (Test-Path $outFile) { Remove-Item $outFile -Force -ErrorAction SilentlyContinue }
    if (Test-Path $errFile) { Remove-Item $errFile -Force -ErrorAction SilentlyContinue }
}
