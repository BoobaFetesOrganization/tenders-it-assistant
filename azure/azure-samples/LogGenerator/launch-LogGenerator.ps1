$scriptRoot = $PSScriptRoot
. $scriptRoot\..\..\lib\resources-lib-file.ps1

# set the output directory
$outputDir = "$scriptRoot\output"

# get the latest log file to transform
$logDir = "$scriptRoot\..\..\..\src\back\TendersITAssistant.Presentation.API\Logs"
$latestLogFile = Get-ChildItem -Path $logDir | Sort-Object Name | Select-Object -Last 1
$latestLogFileName = $latestLogFile.FullName

# get the service principal file and set the login information
$settings = Get-Settings
$logAnalyticsWrksp = $settings.resources | Where-Object { $_.kind -eq "log analytics workspace" } | Select-Object -First 1
$spFile = $logAnalyticsWrksp | Get-Service-Principal-FileName-From-Settings
if (-not (Test-Path $spFile.full)) {
    throw "Service principal file not found. Please run resources-create.ps1 first."
}
$content = Get-Content -Path $spFile.full | ConvertFrom-Json
$login = @{
    tenantId  = $content.tenant
    appId     = $content.appId
    appSecret = $content.password
}

# convert the log file to a JSON file
write-host "Generating sample data..."
write-host " > login:  $($login | ConvertTo-Json | Format-Json)"
& "$scriptRoot\LogGenerator.ps1" -login $login -Log $latestLogFileName -Output "$outputDir\$($latestLogFile.BaseName).json"