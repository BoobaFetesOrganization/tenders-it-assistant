

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
    return (az group show -n $ressource.name)
}

function Set-RessourceGroup(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags
) {
    $result = $ressource | Get-RessourceGroup
    if ($result -eq $null) {
        # Créer le groupe de ressources s'il n'existe pas
        $cmd = "az group create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -l $($location)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString $tags)"
        $result = Invoke-Expression $cmd | Out-Null
        if (-not $?) {
            throw "Ressource group creation fails"
        }
    }  
    return $result     
}

function Get-AppService-Plan(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az appservice plan show -n $ressource.name -g $ressource.ressourceGroup)
}

function Set-AppService-Plan(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags
) {
    $result = $ressource | Get-AppService-Plan
    if ($result -eq $null) {
        # Créer le plan s'il n'existe pas
        $cmd = "az appservice plan create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " -l $($location)"
        $cmd += " --sku $($ressource.sku)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $result = Invoke-Expression $cmd
        if (-not $?) {
            throw "App service plan creation fails"
        }
    }
    return $result
}

function Get-WebApp(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az webapp show -n $ressource.name -g $ressource.ressourceGroup)
}
function Set-WebApp(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags
) {
    $result = $ressource | Get-WebApp
    if ($result -eq $null) {
        $cmd = "az webapp create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -p $($ressource.plan)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " -r $($ressource.runtime)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd += " --https-only"
        $result = Invoke-Expression $cmd
        if (-not $?) {
            throw "Web app creation fails"
        }
    }
    return $result;
}

function Get-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource
) {
    return (az storage account show -n $ressource.name -g $ressource.ressourceGroup)
}
function Set-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter()]
    [object[]] $tags
) {
    $result = $ressource | Get-Storage-Account
    if ($result -eq $null) {
        $cmd = "az storage account create"
        $cmd += " -n $($ressource.name)"
        $cmd += " -g $($ressource.ressourceGroup)"
        $cmd += " --sku $($ressource.sku)"
        $cmd += " --kind $($ressource."azure-kind")"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $result = Invoke-Expression $cmd
        if (-not $?) {
            throw "storage account creation fails"
        }
    }
    return $result;
}


function Set-Storage-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $ressource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags
) {
    $tableStorage = (az storage table exists -n $ressource.name --account-name $ressource.account) | ConvertFrom-Json
    if ($tableStorage.exists) {
        $cmd = "az storage table create"
        $cmd += " -n $($ressource.name)"
        $cmd += " --account-name $($ressource.account)"
        $cmd += " --auth-mode login"
        $result = Invoke-Expression $cmd
        if (-not $?) {
            throw "storage table creation fails"
        }
    }
    return $result;
}