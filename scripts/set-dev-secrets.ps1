using namespace System.IO;

param (
    [FileInfo] $secretsFile,
    [switch] $clear,
    [switch] $show
)

$env = "development"
$setDevSecretRoot = $PSScriptRoot
$ErrorActionPreference = "Stop"
$projectRoot = ([DirectoryInfo](Join-Path $PSScriptRoot "..")).FullName

$jsonDepth = 20
function Set-Secret-File-Template([FileInfo] $template) {
    $items = @{
        AI                = @{
            Gemini_ApiKey = "<your-api-key>"
        }
        ConnectionStrings = @{
            accountstorage = "<your-connection-string>"
        }
        Serilog           = @{
            WriteTo = @{
                AzureLogAnalytics = @{
                    Args = @{
                        credentials = @{
                            endpoint     = "<your-endpoint>"
                            immutableId  = "<your-immutable-id>"
                            tenantId     = "<your-tenant-id>"
                            clientId     = "<your-client-id>"
                            clientSecret = "<your-client-secret>"
                        }
                    }
                }
            }
        }
    }

    $content = @{
        production  = $items
        development = $items
    }

    $file = [FileInfo]"$setDevSecretRoot/.secrets/env-variables.json"
    if ($null -ne $template) { 
        $file = $template; 
        $file.Refresh() 
    }
    if (-not $file.Directory.Exists) { $file.Directory.Create() }

    $content | ConvertTo-Json -Depth $jsonDepth | Out-File -FilePath $file.FullName -Encoding utf8
    Write-Host "template created at: $($file.FullName)"

    return $file
}


# Liste des clés de secrets requises
if ($null -eq $secretsFile -or -not $secretsFile.Exists) {
    Write-Host "Secrets file not found: '$secretsFile'" -ForegroundColor Yellow
    $template = Set-Secret-File-Template $secretsFile
    Write-Host "fill the template then add parameter to execute" -ForegroundColor Yellow
    Write-Host "  -secretFile ""$($template.FullName)""" -ForegroundColor Yellow
    exit
}

# Récupération des secrets
$secrets = Get-Content -Path $secretsFile.FullName | ConvertFrom-Json
$secrets = $secrets.$env

# Récupération du chemin du projet
$projectPath = [DirectoryInfo](Join-Path $projectRoot "src\back\TendersITAssistant.Presentation.API")
if (-not $projectPath.Exists) {
    Write-Host "Project path not found: $projectPath"
    exit
}

$location = Get-Location
try {
    Set-Location $projectPath

    # Initialize user-secrets if not already done
    [xml]$csprojContent = Get-Content "$($projectPath)/TendersITAssistant.Presentation.API.csproj"
    if (-not $csprojContent.Project.PropertyGroup.UserSecretsId) {
        Write-Host "prepare project '$($projectPath.FullName)' to receive secrets" -ForegroundColor Yellow
        dotnet user-secrets init
        Write-Host
    }
    else {
        Write-Host "project '$($projectPath.FullName)' is already set up to use secrets" -ForegroundColor Yellow
        if ($show) {
            Write-Host "list of before update:"
            dotnet user-secrets list
            Write-Host
        }
    }

    # Add secrets
    if ($clear) {
        Write-Host 'clear existing secrets'
        dotnet user-secrets clear
        Write-Host
    }
    
    Write-Host 'set secrets'
    $secrets | ConvertTo-Json -Depth $jsonDepth | dotnet user-secrets set
    Write-Host

    if ($show) {
        # display current secrets
        Write-Host "list of current secrets:" -ForegroundColor Yellow
        dotnet user-secrets list
        Write-Host
    }
}
finally {
    Set-Location $location
}
