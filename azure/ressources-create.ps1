$scriptRoot = $PSScriptRoot
. $scriptRoot\ressources-functions.ps1

az login -u "hyper_silverstar@hotmail.fr"

# ARRANGE
$settings = get-content -path "$($scriptRoot)\ressources.json" -Raw | ConvertFrom-Json
# project ressources
$account = az account show -n "subscription-pay-as-you-go" | ConvertFrom-Json

write-host "set active account to ""$($account.name)""" -ForegroundColor Yellow
az account set --subscription $account.name

# ACT
foreach ($ressource in $settings.ressources) {
    switch ($ressource.kind) {
        "ressource group" { 
            Write-Host "create ressource group $($ressource.name)" -ForegroundColor Yellow
            $ressource | Set-RessourceGroup -location $settings.location -tags $settings.tags | Out-Null
        }
        "appservice plan" { 
            Write-Host "create appservice plan $($ressource.name)" -ForegroundColor Yellow
            $ressource | Set-AppService-Plan -location $settings.location -tags $settings.tags | Out-Null
        
        }
        "webapp" { 
            Write-Host "create webapp $($ressource.name)" -ForegroundColor Yellow
            $ressource | Set-WebApp -location $settings.location -tags $settings.tags | Out-Null
        }
        Default {}
    }
}

# ASSERT
foreach ($ressource in $settings.ressources) {
    switch ($ressource.kind) {
        "ressource group" { 
            Write-Host "store ressource group $($ressource.name) settings" -ForegroundColor Green
            $ressource | Get-RessourceGroup `
            | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json" 
        }
        "appservice plan" { 
            Write-Host "store appservice plan $($ressource.name) settings" -ForegroundColor Green
            $ressource | Get-AppService-Plan `
            | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json" `
        
        }
        "webapp" { 
            Write-Host "store webapp $($ressource.name) settings" -ForegroundColor Green
            $ressource | Get-WebApp `
            | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json"
        }
        Default {}
    }
}

Write-Host "ressources created"