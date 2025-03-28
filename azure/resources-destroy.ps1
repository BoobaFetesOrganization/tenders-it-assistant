param (
    [Parameter(Mandatory = $true)]
    [string] $name,
    [switch]$noLogin,
    [switch]$noLogout
)

$scriptRoot = $PSScriptRoot
. $scriptRoot\lib\resources-lib-commands.ps1

try {
    if (-not $noLogin) { Login }

    Write-Host "============================================================" -ForegroundColor Green
    Write-Host "    DESTROY RESOURCES  - $name                              " -ForegroundColor Green
    Write-Host "============================================================" -ForegroundColor Green

    # ARRANGE
    $settings = Get-Settings -name $name
    
    $settings.subscription | Set-Subscription
    $subscription = $settings.subscription | Get-Subscription
    Write-Host "subscription : `n$($subscription | ConvertTo-Json | Format-Json)"

    if ($null -eq $subscription) {
        Write-Error "No subscription found"
        exit
    }
    #clean error file
    $ErrorFile = "$($scriptRoot)\error.log"
    $ErrorFile | Clear-Error-File    
    #clean all resources files
    Set-Resources-Folders -subs $name

    $resourceGroup = $settings.resources | Where-Object { $_.kind -eq "resource group" } | Get-RessourceGroup
    if ($null -eq $resourceGroup) {    
        Write-Host "resource group not found." -ForegroundColor Red
        exit 1
    }
    for ($index = $settings.resources.Count - 1 ; $index -ge 0; $index--) {        
        $resource = $settings.resources[$index]
        Write-Host "destroy resource '$($resource.name)' of kind '$($resource.kind)'" -ForegroundColor Green
        switch ($resource.kind) {
            "service principal" {
                $resource | Remove-ServicePrincipal | Out-Null
            }
            "resource group" { 
                # do nothing because devoteam sandboxes doesn't allow to create resource
            }
            "appservice plan" { 
                $resource | Remove-AppService-Plan | Out-Null
            }
            "webapp" { 
                $resource | Remove-WebApp | Out-Null
            }       
            "storage account" { 
                $resource | Remove-Storage-Account | Out-Null
            }
            "storage table" { 
                $resource | Remove-Storage-Table | Out-Null
            }
            "log analytics workspace" { 
                $resource | Remove-Log-Analytics-Workspace | Out-Null
            }
            "monitor data-collection endpoint" {
                $resource | Remove-Monitor-DataCollection-Endpoint | Out-Null
            }
            "log analytics workspace table" { 
                $resource | Remove-Log-Analytics-Workspace-Table | Out-Null
            }
            "monitor data-collection rule" {
                $resource | Remove-Monitor-DataCollection-Rule | Out-Null                
            }
            Default {}
        }
    }
    Clear-Resources-Files 
}
catch {
    Write-Error $Error[0]
}
finally {
    if (-not $noLogout) { az logout }
}