$scriptRoot = $PSScriptRoot
. $scriptRoot\ressources-functions.ps1

$ErrorFile = "$($scriptRoot)\error.log"
if (Test-Path $ErrorFile) {
    Remove-Item $ErrorFile
}
try {
    # disable the subscription selector feature once logged in
    az config set core.login_experience_v2=off
    # login with device code
    $currentSubscription = az login --use-device-code | ConvertFrom-Json | Select-Object -First 1
    if (-not $currentSubscription) { throw "Login failed" }
    if ($currentSubscription -is [array]) { $currentSubscription = $currentSubscription[0] }
    Write-Host "login success" -ForegroundColor Yellow
      
    # ARRANGE
    $settings = get-content -path "$($scriptRoot)\ressources.json" -Raw | ConvertFrom-Json

    # if current subscription is not the one involved, switch to it
    if ($currentSubscription.name -ne $subscription.name) {
        Write-Host "set active account to ""$($subscription.name)""" -ForegroundColor Yellow
        az account set --subscription $subscription.name
        
        # find subscription and print it in the console
        $currentSubscription = az account show -n $settings.subscription | ConvertFrom-Json
        Write-Host "active account : `n$($subscription | ConvertTo-Json)"
    }
    Set-Content -Path "$($scriptRoot)\ressources\subscription.json" -Value $($currentSubscription | ConvertTo-Json)

    # ACT
    foreach ($ressource in $settings.ressources) {
        if ($ressource.disabled) { continue }
        switch ($ressource.kind) {
            "ressource group" { 
                $ressource | Set-RessourceGroup -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "appservice plan" { 
                $ressource | Set-AppService-Plan -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "webapp" { 
                $ressource | Set-WebApp -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }       
            "storage account" { 
                $ressource | Set-Storage-Account -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "storage table" { 
                $ressource | Set-Storage-Table -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "log analytics workspace" { 
                $ressource | Set-Log-Analytics-Workspace -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            # ne fonctionne pas encore => "log analytics workspace table" { 
            # ne fonctionne pas encore =>     $ressource | Set-Log-Analytics-Workspace-Table -ErrorFile $ErrorFile | Out-Null
            # ne fonctionne pas encore => }
            Default {}
        }
    }

    # ASSERT
    Write-Host "store all ressources in files" -ForegroundColor Yellow

    foreach ($ressource in $settings.ressources) {
        if ($ressource.disabled) { continue }
        switch ($ressource.kind) {
            "ressource group" { 
                $ressource | Get-RessourceGroup | ConvertTo-Json `
                | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json" 
                Write-Host "store ressource group '$($ressource.name)' settings"
            }
            "appservice plan" { 
                $ressource | Get-AppService-Plan  | ConvertTo-Json `
                | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json" 
                Write-Host "appservice plan '$($ressource.name)' settings stored"            
            }
            "webapp" { 
                $ressource | Get-WebApp  | ConvertTo-Json `
                | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json"
                Write-Host "webapp '$($ressource.name)' settings stored"
            }
            "storage account" { 
                $ressource | Get-Storage-Account  | ConvertTo-Json `
                | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json"
                Write-Host "storage account '$($ressource.name)' settings stored"
            }
            "log analytics workspace" { 
                $ressource | Get-Log-Analytics-Workspace | ConvertTo-Json `
                | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json"
                Write-Host "log analytics workspace '$($ressource.name)' settings stored"
            }
            # ne fonctionne pas encore => "log analytics workspace table" { 
            # ne fonctionne pas encore =>     $ressource | Get-Log-Analytics-Workspace-Table | ConvertTo-Json `
            # ne fonctionne pas encore =>     | Set-Content -Path "$($scriptRoot)\ressources\$($ressource.name).json"
            # ne fonctionne pas encore =>     Write-Host "log analytics workspace table '$($ressource.name)' settings stored"
            # ne fonctionne pas encore => }
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