$scriptRoot = $PSScriptRoot
. $scriptRoot\resources-lib-commands.ps1

function Test-User-Acceptance([Parameter(Mandatory = $true)][string]$message) {
    Write-Host $message -foregroundcolor Yellow
    Write-Host "(Y to accept otherwise any other caracter)" -foregroundcolor Gray
    $key = $Host.UI.RawUI.ReadKey()

    $accepted = "$($key.Character)".ToLower() -eq "y"
    if ( $accepted) { Write-Host " -> accepted !" -ForegroundColor Yellow }
    else { Write-Host " -> refused !" -ForegroundColor Yellow }
    
    return $accepted
}

try {
    Login | Out-Null

    # ARRANGE
    $settings = Get-Settings
    
    $settings.subscription | Set-Subscription
    $subscription = $settings.subscription | Get-Subscription
    Write-Host "subscription : `n$($subscription | ConvertTo-Json | Format-Json)"

    # ACT : delete Monitor resources : dce 
    if (Test-User-Acceptance "Do you want to proceed with deleting the monitor endpoints?") {
        $settings.resources `
        | Where-Object { $_.kind -eq "monitor data-collection endpoint" } `
        | Foreach-Object {
            Write-Host "destroy monitor's data collection $($_.name)" -ForegroundColor Yellow
            az monitor data-collection endpoint delete --yes --name $_.name --resource-group $_.resourceGroup --subscription $subscription.id
        }
    }
    
    # ACT : delete resources    
    if (Test-User-Acceptance "Do you want to proceed with deleting the resources?") {
        $resource = $settings.resources | Where-Object { $_.kind -eq "resource group" }
        if ($null -eq ($resource | Get-RessourceGroup)) {
            Write-Host "resource group not found, nothing to delete" -ForegroundColor Yellow
            return
        }
        Write-Host "destroy resource group $($resource.name)" -ForegroundColor Yellow
        az group delete -n $resource.name --yes
    }
    
    # ACT : delete service principals    
    if (Test-User-Acceptance "Do you want to proceed with deleting the service principals?") {
        $settings.resources `
        | Where-Object { $_.servicePrincipal -ne $null } `
        | ForEach-Object {
            $sp = $_.servicePrincipal | Get-ServicePrincipal
            if ($sp -eq $null) {
                Write-Host "service principal not found, nothing to delete" -ForegroundColor Yellow
                return
            }
            Write-Host "destroy service principal $($sp.appDisplayName) (id: '$($sp.appId)')" -ForegroundColor Yellow
            az ad sp delete --id $sp.appId
        } 
    }
}
catch {
    Write-Error $Error[0]
}
finally {
    az logout
}