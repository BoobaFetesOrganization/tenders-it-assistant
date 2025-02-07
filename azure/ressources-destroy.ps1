$scriptRoot = $PSScriptRoot
. $scriptRoot\ressources-functions.ps1

az login -u "hyper_silverstar@hotmail.fr"

# ARRANGE
$settings = get-content -path "$($scriptRoot)\ressources.json" -Raw | ConvertFrom-Json
# project ressources
$account = az account show -n "subscription-pay-as-you-go" | ConvertFrom-Json

write-host "set active account to ""$($account.name)""" -ForegroundColor Yellow
az account set --subscription $account.name

# ACT
$group = $settings.ressources | Where-Object { $_.kind -eq "ressource group" }
Get-RessourceGroup $group

write-host "destroy ressource group $($group.name)" -ForegroundColor Yellow
az group delete -n $group.name --yes

Write-Host "ressources destroyed" -foregroundcolor Green