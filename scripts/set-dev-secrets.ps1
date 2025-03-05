using namespace System.IO;

param (
    [hashtable]$secrets,
    [switch]$clear
)

$ErrorActionPreference = "Stop"
$projectRoot = ([DirectoryInfo](Join-Path $PSScriptRoot "..")).FullName

if (-not $secrets) {
    Write-Host "Usage: set-dev-secrets.ps1 -secrets @{ connectionstring= 'your_connection_string'; apikey = 'your_api_key' }"
    exit
}

$projectPath = [DirectoryInfo](Join-Path $projectRoot "src\back\GenAIChat\GenAIChat.Presentation.API")
if (-not $projectPath.Exists) {
    Write-Host "Project path not found: $projectPath"
    exit
}



# Initialize user-secrets if not already done
[xml]$csprojContent = Get-Content "$($projectPath)/GenAIChat.Presentation.API.csproj"
if (-not $csprojContent.Project.PropertyGroup.UserSecretsId) {
    Write-Host "prepare project '$($projectPath.FullName)' to receive secrets" -ForegroundColor Yellow
    dotnet user-secrets init --project $projectPath
}
else {
    Write-Host "project '$($projectPath.FullName)' is already set up to use secrets" -ForegroundColor Yellow
    Write-Host "list of current secrets" -ForegroundColor Yellow
    dotnet user-secrets list --project $projectPath
    Write-Host "end of the current secrets" -ForegroundColor Yellow

    if ($clear) {
        Write-Host "clear current secrets" -ForegroundColor Red
        dotnet user-secrets clear --project $projectPath
    }
}

# Add secrets
dotnet user-secrets set "ConnectionStrings:accountstorage" $secrets.connectionstring --project $projectPath
dotnet user-secrets set "AI:Gemini_ApiKey" $secrets.apikey --project $projectPath

# display current secrets
Write-Host "list of new secrets" -ForegroundColor Yellow
dotnet user-secrets list --project $projectPath
Write-Host "end of the new secrets" -ForegroundColor Yellow

