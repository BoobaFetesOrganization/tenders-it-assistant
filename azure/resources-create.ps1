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
    $references = [hashtable]@{
        subscription = $subscription
        endpoint     = $null;
        workspace    = $null 
    }
    foreach ($resource in $settings.resources) {
        if ($resource.disabled) { continue }
        switch ($resource.kind) {
            # debug "resource group" { 
            # debug     $resource | Set-RessourceGroup -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # debug }
            # debug "appservice plan" { 
            # debug     $resource | Set-AppService-Plan -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # debug }
            # debug "webapp" { 
            # debug     $resource | Set-WebApp -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # debug }       
            # debug "storage account" { 
            # debug     $resource | Set-Storage-Account -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # debug }
            # debug "storage table" { 
            # debug     $resource | Set-Storage-Table -tags $settings.tags -ErrorFile $ErrorFile | Out-Null
            # debug }
            "log analytics workspace" { 
                $references.workspace = $resource | Set-Log-Analytics-Workspace -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile
            }
            "monitor data-collection endpoint" {
                $references.endpoint = $resource | Set-Monitor-DataCollection-Endpoint -location $settings.location -tags $settings.tags -ErrorFile $ErrorFile
            }
            "log analytics workspace table" { 
                $resource | Set-Log-Analytics-Workspace-Table -ErrorFile $ErrorFile | Out-Null
            }
            "monitor data-collection rule" {
                $resource | Set-Monitor-DataCollection-Rule -location $settings.location -references $references -tags $settings.tags -ErrorFile $ErrorFile | Out-Null                
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