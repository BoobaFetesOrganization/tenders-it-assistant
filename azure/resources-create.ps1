param (
    [switch]$noLogin,
    [switch]$noLogout
)

$scriptRoot = $PSScriptRoot
. $scriptRoot\lib\resources-lib-commands.ps1


try {    
    Write-Host "============================================================" -ForegroundColor Green
    Write-Host "    INITIALIZE                                              " -ForegroundColor Green
    Write-Host "============================================================" -ForegroundColor Green

    #clean error file
    $ErrorFile = "$($scriptRoot)\error.log"
    $ErrorFile | Clear-Error-File

    $settings = Get-Settings
    if ($noLogin) {
        $subscription = $settings.subscription | Get-Subscription
        Write-Host ($subscription | ConvertTo-Json | Format-Json)
    }
    else { $subscription = Login }
      
    if ($null -eq $subscription) {
        Write-Error "No subscription found"
        exit
    }
    # if current subscription is not the one involved, switch to it
    if ($subscription.name -ne $settings.subscription) {
        Write-Host "set active account to ""$($settings.subscription)""" -ForegroundColor Yellow
        $settings.subscription | Set-Subscription
        
        # find subscription and print it in the console
        $subscription = $settings.subscription | Get-Subscription
        Write-Host "active account : `n$($subscription | ConvertTo-Json | Format-Json)"
    }
    $subscription | New-Resource-File
    
    #clean all resources files
    Set-Resources-Folders -subs $subscription.id
    Clear-Resources-Files
    
    # ACT
    $references = [hashtable]@{
        subscription = $subscription
    }
    
    Write-Host "============================================================" -ForegroundColor Green
    Write-Host "    CREATE RESOURCES                                        " -ForegroundColor Green
    Write-Host "============================================================" -ForegroundColor Green

    foreach ($resource in $settings.resources) {
        switch ($resource.kind) {
            "resource group" { 
                $resource | Set-RessourceGroup -references $references -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "appservice plan" { 
                $resource | Set-AppService-Plan -references $references -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "webapp" { 
                $resource | Set-WebApp -references $references -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }       
            "storage account" { 
                $resource | Set-Storage-Account -references $references -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "storage table" { 
                $resource | Set-Storage-Table -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "log analytics workspace" { 
                $resource | Set-Log-Analytics-Workspace -references $references -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "monitor data-collection endpoint" {
                $resource | Set-Monitor-DataCollection-Endpoint -references $references -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            }
            "log analytics workspace table" { 
                $resource | Set-Log-Analytics-Workspace-Table -references $references -ErrorFile $ErrorFile | Out-Null
            }
            "monitor data-collection rule" {
                $resource | Set-Monitor-DataCollection-Rule -references $references -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null                
            }
            Default {}
        }
    }
    


    Write-Host "============================================================" -ForegroundColor Green
    Write-Host "    CREATE SERVICE PRINCIPALS                               " -ForegroundColor Green
    Write-Host "============================================================" -ForegroundColor Green

    foreach ($servicePrincipal in $settings.servicePrincipals) {
        $servicePrincipal | Set-ServicePrincipal -references $references -ErrorFile $ErrorFile | Out-Null
    }
}
catch {
    Write-Error $Error[0]
}
finally {   
    if (-not $noLogout) { az logout }
}