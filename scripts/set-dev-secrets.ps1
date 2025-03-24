using namespace System.IO;

param (
    [hashtable]$secrets,
    [switch]$clear
)

$ErrorActionPreference = "Stop"
$projectRoot = ([DirectoryInfo](Join-Path $PSScriptRoot "..")).FullName

# Liste des clés de secrets requises
$requiredSecrets = @(
    "AZURE_STORAGE_CONNECTIONSTRING",
    "GOOGLE_GEMINI_APIKEY",
    "AZURE_TENANT_ID",
    "AZURE_WEBAPP_LOG_ID",
    "AZURE_WEBAPP_LOG_SECRET"
)
# Fonction pour vérifier la validité d'un secret
function Test-SecretValidity {
    param (
        [string]$secretName,
        [string]$secretValue
    )

    if ($null -eq $secretValue) {
        Write-Error "Le secret '$secretName' est invalide : valeur nulle."
        return $false
    }
    elseif ("" -eq $secretValue) {
        Write-Error "Le secret '$secretName' est invalide : valeur vide."
        return $false
    }
    elseif ($secretValue -match '\s') {
        Write-Error "Le secret '$secretName' est invalide : valeur ne contenant que des espaces ou tabulations."
        return $false
    }
    
    return $true
}

# affichage du paramètre attendu
Write-Host 'pour rappel, le parametre $secrets est un objet qui est defini comme ci-dessous' -ForegroundColor Yellow
Write-Host "(le paramètre 'secret' n'est pas obligé de contenir toutes les propriétés listées)" -ForegroundColor Yellow
Write-Host '$secrets = @{' -ForegroundColor Yellow
$requiredSecrets | ForEach-Object { Write-Host "  $_ = ""the secret""" -ForegroundColor Yellow }
Write-Host '}' -ForegroundColor Yellow
Write-Host './set-dev-secrets.ps1' -ForegroundColor Yellow
Write-Host

if ($null -eq $secrets) {
    Write-Error "Le paramètre 'secrets' est nul. Veuillez le définir."
    exit
}
elseif ($secrets.GetType().Name -ne "Hashtable") {
    Write-Error "Le paramètre 'secrets' n'est pas un Hashtable. Veuillez le définir."
    exit
}
$allSecretsValid = $true
foreach ($secretName in $requiredSecrets) {
    if ($secrets.ContainsKey($secretName)) {
        $secretValue = $secrets[$secretName]
        if (-not (Test-SecretValidity -secretName $secretName -secretValue $secretValue)) {
            $allSecretsValid = $false
        }
    }
}
if ($allSecretsValid -and $secrets.Count -eq 0) {
    Write-Error "Aucune propriété n'est définie dans \$secrets."
    $allSecretsValid = $false
}
# Affichage d'un message si tous les secrets sont valides
if ($allSecretsValid) {
    Write-Host "Tous les secrets saisis sont valides."
}
else {
    Write-Error "Certains secrets sont invalides. Veuillez les corriger."
    exit
}

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
    }
    else {
        Write-Host "project '$($projectPath.FullName)' is already set up to use secrets" -ForegroundColor Yellow
        Write-Host "list of before update:"
        dotnet user-secrets list
    }
    Write-Host

    # Add secrets
    if ($clear) {
        Write-Host 'clear existing secrets'
        dotnet user-secrets clear
        Write-Host
    }
    
    write-host 'set new secrets'
    if ($secrets.ContainsKey("GOOGLE_GEMINI_APIKEY")) {
        dotnet user-secrets set "AI:Gemini_ApiKey" "$($secrets.GOOGLE_GEMINI_APIKEY)"    
    }
    if ($secrets.ContainsKey("AZURE_STORAGE_CONNECTIONSTRING")) {
        dotnet user-secrets set "ConnectionStrings:accountstorage" "$($secrets.AZURE_STORAGE_CONNECTIONSTRING)"
    }
    if ($secrets.ContainsKey("AZURE_DCE_ENDPOINT")) {
        dotnet user-secrets set "Serilog:WriteTo:AzureLogAnalytics:Args:credentials:endpoint" "$($secrets.AZURE_DCE_ENDPOINT)"
    }
    if ($secrets.ContainsKey("AZURE_DCR_ID")) {
        dotnet user-secrets set "Serilog:WriteTo:AzureLogAnalytics:Args:credentials:immutableId" "$($secrets.AZURE_DCR_ID)"
    }
    if ($secrets.ContainsKey("AZURE_TENANT_ID")) {
        dotnet user-secrets set "Serilog:WriteTo:AzureLogAnalytics:Args:credentials:tenantId" "$($secrets.AZURE_TENANT_ID)"
    }
    if ($secrets.ContainsKey("AZURE_WEBAPP_LOG_ID")) {
        dotnet user-secrets set "Serilog:WriteTo:AzureLogAnalytics:Args:credentials:clientId" "$($secrets.AZURE_WEBAPP_LOG_ID)"
    }
    if ($secrets.ContainsKey("AZURE_WEBAPP_LOG_SECRET")) {
        dotnet user-secrets set "Serilog:WriteTo:AzureLogAnalytics:Args:credentials:clientSecret" "$($secrets.AZURE_WEBAPP_LOG_SECRET)"
    }
    Write-Host

    # display current secrets
    Write-Host "list of current secrets:" -ForegroundColor Yellow
    dotnet user-secrets list
}
finally {
    Set-Location $location
}
