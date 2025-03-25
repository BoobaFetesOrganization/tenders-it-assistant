using namespace System.IO;

$ErrorActionPreference = "Stop"
$root = ([DirectoryInfo](Join-Path $PSScriptRoot "..")).FullName

$location = Get-Location
try {
    Set-Location $projectPath

    # arrange
    $projectPath = [DirectoryInfo](Join-Path $root "src\back\TendersITAssistant.Presentation.API")    
    $secretsFile = [FileInfo](Join-Path $root ".secrets/env-variables-dev.json")
   
    # check constraints
    if (-not $projectPath.Exists) { throw "Project path not found: $projectPath" }
    if (-not $secretsFile.Directory.Exists) { $secretsFile.Directory.Create() }
    
    # Create secrets file as a template if needed then stop the script
    if ($secretsFile.Exists) { 
        Write-Host "Creating secrets file template..." -ForegroundColor Yellow

        $items = @{
            AI      = @{ Gemini_ApiKey = "<your-api-key>" }
            Serilog = @{ WriteTo = @{ AzureLogAnalytics = @{ Args = @{ credentials = @{
                                endpoint     = "<your-endpoint>"; 
                                immutableId  = "<your-immutable-id>";
                                tenantId     = "<your-tenant-id>"; 
                                clientId     = "<your-client-id>";
                                clientSecret = "<your-client-secret>"; 
                            }                         
                        }                     
                    }                 
                } 
            }
        }    

        $content = @{ production = $items; development = $items } 
        $content | ConvertTo-Json -Depth 20 | Out-File -FilePath $secretsFile.FullName -Encoding utf8

        Write-Host " > created at: $($secretsFile.FullName)"
        Write-Host " > fill the template then execute once again" -ForegroundColor Yellow

        exit 0 
    }

    # secrets file exists, so set secrets
    write-host "Setting secrets from '$($secretsFile.FullName)'" -ForegroundColor Yellow

    # Initialize user-secrets if not already done
    [xml]$csprojContent = Get-Content "$($projectPath)/TendersITAssistant.Presentation.API.csproj"
    if (-not $csprojContent.Project.PropertyGroup.UserSecretsId) {
        dotnet user-secrets init
    }
    
    # set secrets
    $secrets = Get-Content -Path $secretsFile.FullName -Raw
    $secrets | dotnet user-secrets set
}
finally {
    Set-Location $location
}
