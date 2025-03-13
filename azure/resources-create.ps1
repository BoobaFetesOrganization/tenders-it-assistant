$scriptRoot = $PSScriptRoot
. $scriptRoot\resources-lib-commands.ps1


#clean error file
$ErrorFile = "$($scriptRoot)\error.log"
$ErrorFile | Clear-Error-File

#clean all resources files
Clear-Resources-Files

try {
    $subscription = Login
      
    # ARRANGE
    $settings = Get-Settings

    # if current subscription is not the one involved, switch to it
    if ($subscription.name -ne $settings.subscription) {
        Write-Host "set active account to ""$($settings.subscription)""" -ForegroundColor Yellow
        $settings.subscription | Set-Subscription
        
        # find subscription and print it in the console
        $subscription = $settings.subscription | Get-Subscription
        Write-Host "active account : `n$($subscription | ConvertTo-Json)"
    }
    $subscription | New-Resource-File

    # ACT
    foreach ($resource in $settings.resources) {
        if ($resource.disabled) { continue }
        switch ($resource.kind) {
            "resource group" { 
                $resource | Set-RessourceGroup -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "appservice plan" { 
                $resource | Set-AppService-Plan -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "webapp" { 
                $resource | Set-WebApp -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }       
            "storage account" { 
                $resource | Set-Storage-Account -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "storage table" { 
                $resource | Set-Storage-Table -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "log analytics workspace" { 
                $resource | Set-Log-Analytics-Workspace -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            # ne fonctionne pas encore =>  "monitor data-collection endpoint" {
            # ne fonctionne pas encore =>      $resource | Set-Monitor-DataCollection-Endpoint -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # ne fonctionne pas encore =>  }
            # ne fonctionne pas encore =>  "monitor data-collection rule" {
            # ne fonctionne pas encore =>      $resource | Set-Monitor-DataCollection-Rule -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # ne fonctionne pas encore =>  }
            # ne fonctionne pas encore => "log analytics workspace table" { 
            # ne fonctionne pas encore =>     $resource | Set-Log-Analytics-Workspace-Table -ErrorFile $ErrorFile | Out-Null
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