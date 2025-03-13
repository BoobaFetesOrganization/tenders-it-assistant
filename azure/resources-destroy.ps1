$scriptRoot = $PSScriptRoot
. $scriptRoot\resources-lib-commands.ps1

try {
    Login | Out-Null

    # ARRANGE
    $settings = Get-Settings
    
    $settings.subscription | Set-Subscription
    Write-Host "subscription : `n$($settings.subscription | Get-Subscription | ConvertTo-Json)"

    $resource = $settings.resources | Where-Object { $_.kind -eq "resource group" }
    
    # ACT : delete service principals
    $list = $settings.resources | where-object { $_.servicePrincipal -ne $null } | Select-Object -ExpandProperty servicePrincipal
    foreach ($item in $list) {
        $servicePrincipal = $item | Get-ServicePrincipal
        if ($null -eq $servicePrincipal) { continue }
        
        Write-Host "destroy service principal $($servicePrincipal.appDisplayName) (id: '$($servicePrincipal.appId)')" -ForegroundColor Yellow
        # quelque chose est étrange !!    cela ne fonctionne pas !! =>         
        az ad sp delete --id $servicePrincipal.appId
    }

    # ACT : delete resources
    if ($null -eq ($resource | Get-RessourceGroup)) {
        Write-Host "resource group not found, nothing to delete" -ForegroundColor Yellow
        return
    }

    Write-Host "destroy resource group $($resource.name)" -ForegroundColor Yellow
    az group delete -n $resource.name --yes

    Write-Host "resources destroyed" -foregroundcolor Green
}
catch {
    Write-Error $Error[0]
}
finally {
    az logout
}