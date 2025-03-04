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
    $group = $settings.ressources | Where-Object { $_.kind -eq "ressource group" }
    Get-RessourceGroup $group

    write-host "destroy ressource group $($group.name)" -ForegroundColor Yellow
    az group delete -n $group.name --yes

    Write-Host "ressources destroyed" -foregroundcolor Green
}
catch {
    Write-Error $Error[0]
}
finally {
    az logout
}