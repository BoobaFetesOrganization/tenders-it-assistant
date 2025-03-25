param (
    [switch]$skipLogin,
    [switch]$skipLogout
)

$scriptRoot = $PSScriptRoot
. $scriptRoot\lib\resources-lib-commands.ps1

function Test-User-Acceptance([Parameter(Mandatory = $true)][string]$message) {
    Write-Host $message -foregroundcolor Yellow
    Write-Host "(Y to accept otherwise any other caracter)" -foregroundcolor Gray
    $Host.UI.RawUI.FlushInputBuffer()
    $key = $Host.UI.RawUI.ReadKey()

    $accepted = "$($key.Character)".ToLower() -eq "y"
    if ( $accepted) { Write-Host " -> accepted !" -ForegroundColor Yellow }
    else { Write-Host " -> refused !" -ForegroundColor Yellow }
    
    return $accepted
}

try {
    if (-not $skipLogin) { Login }

    Write-Host "============================================================" -ForegroundColor Green
    Write-Host "    DESTROY RESOURCES                                       " -ForegroundColor Green
    Write-Host "============================================================" -ForegroundColor Green

    # ARRANGE
    $settings = Get-Settings
    
    $settings.subscription | Set-Subscription
    $subscription = $settings.subscription | Get-Subscription
    Write-Host "subscription : `n$($subscription | ConvertTo-Json | Format-Json)"

    # ACT : delete service principals    
    if (Test-User-Acceptance "Do you want to proceed with deleting the service principals?") {
        $settings.resources | Where-Object { $_.servicePrincipals -is [array] } `
        | ForEach-Object {
            foreach ($item in $_.servicePrincipals) {
                $sp = $item | Get-ServicePrincipal
                if ($sp -eq $null) {
                    Write-Host "service principal '$($item.name)' not found" -ForegroundColor Red
                    continue
                }
                Write-Host "destroy service principal $($sp.appDisplayName) (id: '$($sp.appId)')" -ForegroundColor Green
                az ad sp delete --id $sp.appId
            }
        } 
    }


    # ACT : delete resources and resource group    
    if (Test-User-Acceptance "Do you want to proceed with deleting the resource group?") {
        # CHK : check if resource group exists
        $resourceGroup = $settings.resources | Where-Object { $_.kind -eq "resource group" } | Get-RessourceGroup
        if ($null -eq $resourceGroup) {    
            Write-Host "resource group not found." -ForegroundColor Red
            exit 1
        }

        # ACT : delete Monitor resources : dcr 
        $settings.resources `
        | Where-Object { $_.kind -eq "monitor data-collection rule" } `
        | Foreach-Object {
            Write-Host "destroy monitor's data collection $($_.name)" -ForegroundColor Green
            az monitor data-collection rule delete --yes --name $_.name --resource-group $_.resourceGroup --subscription $subscription.id
        }

        # ACT : delete Monitor resources : dce 
        $settings.resources `
        | Where-Object { $_.kind -eq "monitor data-collection endpoint" } `
        | Foreach-Object {
            Write-Host "destroy monitor's data collection $($_.name)" -ForegroundColor Green
            az monitor data-collection endpoint delete --yes --name $_.name --resource-group $_.resourceGroup --subscription $subscription.id
        }
    
        Write-Host "destroy resource group $($resourceGroup.name)" -ForegroundColor Green
        az group delete -n $resourceGroup.name --yes    
    }
}
catch {
    Write-Error $Error[0]
}
finally {
    if (-not $skipLogout) { az logout }
}