$scriptRoot = $PSScriptRoot
. $scriptRoot\ressources-functions.ps1

try {
    az login --use-device-code
    if (-not $?) {
        throw "Login failed"
    }

    # ARRANGE
    $settings = get-content -path "$($scriptRoot)\ressources.json" -Raw | ConvertFrom-Json
    # project ressources
    $account = az account show -n $settings.subscription | ConvertFrom-Json

    write-host "set active account to ""$($account.name)""" -ForegroundColor Yellow
    az account set --subscription $account.name

    # ACT
    foreach ($ressource in $settings.ressources) {
        if ($ressource.disabled) { continue }
        switch ($ressource.kind) {
            "ressource group" { 
                Write-Host "create ressource group '$($ressource.name)'" -ForegroundColor Yellow
                $ressource | Set-RessourceGroup -location $settings.location -tags $settings.tags | Out-Null
                Write-Host "$($ressource.name) ($($settings.location)) exists" -ForegroundColor Green
            }
            "appservice plan" { 
                Write-Host "create appservice plan '$($ressource.name)'" -ForegroundColor Yellow
                $ressource | Set-AppService-Plan -location $settings.location -tags $settings.tags | Out-Null
                Write-Host "$($ressource.name) ($($settings.location)) exists" -ForegroundColor Green
            }
            "webapp" { 
                Write-Host "create webapp '$($ressource.name)'" -ForegroundColor Yellow
                $ressource | Set-WebApp -location $settings.location -tags $settings.tags | Out-Null
                Write-Host "$($ressource.name) ($($settings.location)) exists" -ForegroundColor Green
            }       
            "storage account" { 
                Write-Host "create storage account '$($ressource.name)'" -ForegroundColor Yellow
                $ressource | Set-Storage-Account -tags $settings.tags | Out-Null
                Write-Host "$($ressource.name) ($($settings.location)) exists" -ForegroundColor Green                
            }        
            "storage table" { 
                Write-Host "create table storage '$($ressource.name)'" -ForegroundColor Yellow
                $ressource | Set-Storage-Table -location $settings.location -tags $settings.tags | Out-Null
                Write-Host "$($ressource.name) ($($settings.location)) exists" -ForegroundColor Green
            }
            Default {}
        }
    }

    # ASSERT
    foreach ($ressource in $settings.ressources) {
        if ($ressource.disabled) { continue }
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
            "storage account" { 
                Write-Host "storage account $($ressource.name) settings" -ForegroundColor Green
                $ressource | Get-Storage-Account `
                | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json"
            }
            Default {}
        }
    }
}
catch {
    Write-Error $Error[0]
}
finally {
    az logout
}