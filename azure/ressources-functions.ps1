


function Invoke-Az-Command(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $cmd,    
    [Parameter(Mandatory = $true)]
    [string] $name, 
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    Invoke-Expression $cmd 2>$ErrorFile | Out-Null
    $hasError = Test-Path $ErrorFile
    if ($hasError) {
        $cmdError = Get-Content $ErrorFile
        Write-Host "resource '$name' creation fails" -ForegroundColor Red
        Write-Host ""
        Write-Host " > Command : " -ForegroundColor Red
        Write-Host $cmd -ForegroundColor Red
        Write-Host ""
        Write-Host " > Error : " -ForegroundColor Red
        Write-Host $cmdError -ForegroundColor Red
        throw "resource '$name' creation fails. see above error message"
    }
    Write-Host "resource '$name' created" -ForegroundColor Green
}

function Get-Tags-AsKeyValue( 
    [Parameter(Mandatory = $true)]
    [object[]] $tags
) {
    $result = @()
    foreach ($tag in $tags) {
        $result += "$($tag.name)=$($($tag.value))" # Ajoute un espace après chaque tag
    }
    return $result
}
function Get-Tags-AsKeyValue-ToString( 
    [Parameter(Mandatory = $true)]
    [object[]] $tags
) {
    return (Get-Tags-AsKeyValue $tags) -join " "
}

function Get-RessourceGroup(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az group show -n $ressource.name) | ConvertFrom-Json
}

function Set-RessourceGroup(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $ressource | Get-RessourceGroup
    if ($null -eq $result) {
        # Créer le groupe de ressources s'il n'existe pas
        $cmd = "az group create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -l $($location)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString $tags)"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-AppService-Plan(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az appservice plan show -n $ressource.name -g $ressource.ressourceGroup 2>$null) | ConvertFrom-Json
}

function Set-AppService-Plan(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $ressource | Get-AppService-Plan
    if ($null -eq $result) {
        # Créer le plan s'il n'existe pas
        $cmd = "az appservice plan create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " -l $($location)"
        $cmd += " --sku $($ressource.sku)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-WebApp(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az webapp show -n $ressource.name -g $ressource.ressourceGroup 2>$null) | ConvertFrom-Json
}
function Set-WebApp(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $ressource | Get-WebApp
    if ($null -eq $result) {
        $cmd = "az webapp create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -p $($ressource.plan)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " -r $($ressource.runtime)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd += " --https-only"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az storage account show -n $ressource.name -g $ressource.ressourceGroup) | ConvertFrom-Json
}
function Set-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $ressource | Get-Storage-Account
    if ($null -eq $result) {
        $cmd = "az storage account create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " --sku $($ressource.sku)"
        $cmd += " --kind $($ressource."azure-kind")"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}


function Set-Storage-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $tableStorage = (az storage table exists -n $ressource.name --account-name $ressource.account 2>$null) | ConvertFrom-Json
    if ($tableStorage.exists) {
        $cmd = "az storage table create"
        $cmd += " -n $($ressource.name)"
        $cmd += " --account-name $($ressource.account)"
        $cmd += " --auth-mode login"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-Log-Analytics-Workspace(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az monitor log-analytics workspace show -n $ressource.name -g $ressource.ressourceGroup) | ConvertFrom-Json
}
function Set-Log-Analytics-Workspace(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $ressource | Get-Log-Analytics-Workspace
    if ($null -eq $result) {
        $cmd = "az monitor log-analytics workspace create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " --sku $($ressource.sku)"
        $cmd += " -l $location"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-Log-Analytics-Workspace-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az monitor log-analytics workspace table show -n $ressource.name -g $ressource.ressourceGroup --workspace-name $ressource.workspaceName 2>$null) | ConvertFrom-Json
}
function Set-Log-Analytics-Workspace-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {    
    $result = $ressource | Get-Log-Analytics-Workspace-Table
    if ($null -eq $result) {
        $cmd = "az monitor log-analytics workspace table create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " --workspace-name $($ressource.workspaceName)"
        $cmd += " --plan $($ressource.plan)"
        $cmd += " --retention-time $($ressource.retentionTime)"
        $cmd += " --total-retention-time $($ressource.totalRetentionTime)"
        $cmd | Invoke-Az-Command -name $ressource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($ressource.name)' already exists" -ForegroundColor Yellow
    }
}