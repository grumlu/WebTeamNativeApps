Set-Location $PSScriptRoot

$inputFileName = 'Package.appxmanifest'
$outputFileName = $PSScriptRoot + '/Package.appxmanifest';

$baseDate = Get-Date -Date "2000-01-01 00:00:00Z"
$now = Get-Date
$difference = $now - $baseDate
$versionMinor = $now.Year
$versionBuild = $now.DayOfYear
$versionRevision = ($now.Hour * 60) + $now.Minute

$content = (gc $inputFileName) -join "`r`n"

$callback = {
  param($match)
    [string]$versionMajor = $match.Groups[2].Value
    #[int]$intNum = [convert]::ToInt32($versionMajor, 10)
    $match.Groups[1].Value + 'Version="' + $versionMajor + '.' + $versionMinor + '.' + $versionBuild + '.' + $versionRevision + '"'
}

$identityRegex = [regex]'(\<Identity[^\>]*)Version=\"([0-9])+\.([0-9]+)\.([0-9]+)\.([0-9]+)\.*\"'
$newContent = $identityRegex.Replace($content, $callback)
#Write-Host $newContent
[io.file]::WriteAllText($outputFileName, $newContent)