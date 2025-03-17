$resourceFunctionRoot = $PSScriptRoot

function New-Resource-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    $fullfileName = "$resourceFunctionRoot\resources\$($resource.name).json"
    $resource | ConvertTo-Json | Out-File -FilePath $fullfileName -Force
    Write-Host "resource '$($resource.name)' stored in '$fullfileName'" -ForegroundColor Green
}
function New-Resource-File-Service-Principal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $servicePrincipal,
    [Parameter(Mandatory = $true)]
    [object] $resource
) {
    $fullfileName = "$resourceFunctionRoot\resources\$($resource.name)--service-principal--$($sp.name).json"
    $servicePrincipal | ConvertTo-Json | Out-File -FilePath $fullfileName -Force
    Write-Host "credentials (service principal) '$($servicePrincipal.name)' stored in '$fullfileName'" -ForegroundColor Yellow
}

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

function Get-Subscription(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name
) {
    return (az account show -n $name) | ConvertFrom-Json
}

function Set-Subscription(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name
) {
    return (az account set --subscription $name)
}
function Get-RessourceGroup(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az group show -n $resource.name) | ConvertFrom-Json
}

function Set-RessourceGroup(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $resource | Get-RessourceGroup
    if ($null -eq $result) {
        # Créer le groupe de resources s'il n'existe pas
        $cmd = "az group create"
        $cmd += " -n $($resource.name)"
        $cmd += " -l $($location)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString $tags)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-RessourceGroup | New-Resource-File
}

function Get-AppService-Plan(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az appservice plan show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
}

function Set-AppService-Plan(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $resource | Get-AppService-Plan
    if ($null -eq $result) {
        # Créer le plan s'il n'existe pas
        $cmd = "az appservice plan create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " -l $($location)"
        $cmd += " --sku $($resource.sku)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-AppService-Plan | New-Resource-File
}

function Get-WebApp(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az webapp show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
}
function Set-WebApp(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $resource | Get-WebApp
    if ($null -eq $result) {
        $cmd = "az webapp create"
        $cmd += " -n $($resource.name)"
        $cmd += " -p $($resource.plan)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " -r $($resource.runtime)"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd += " --https-only"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-WebApp | New-Resource-File

    $resource.deployment.servicePrincipal | Set-ServicePrincipal -ErrorFile $ErrorFile -resource $resource.name

    # configure login and password for gituhb actions

}

function Get-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az storage account show -n $resource.name -g $resource.resourceGroup) | ConvertFrom-Json
}
function Set-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $resource | Get-Storage-Account
    if ($null -eq $result) {
        $cmd = "az storage account create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " --sku $($resource.sku)"
        $cmd += " --kind $($resource."azure-kind")"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-Storage-Account | New-Resource-File
}


function Set-Storage-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $tableStorage = (az storage table exists -n $resource.name --account-name $resource.account 2>$null) | ConvertFrom-Json
    if ($tableStorage.exists) {
        $cmd = "az storage table create"
        $cmd += " -n $($resource.name)"
        $cmd += " --account-name $($resource.account)"
        $cmd += " --auth-mode login"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-Log-Analytics-Workspace(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az monitor log-analytics workspace show -n $resource.name -g $resource.resourceGroup) | ConvertFrom-Json
}
function Set-Log-Analytics-Workspace(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $resource | Get-Log-Analytics-Workspace
    if ($null -eq $result) {
        $cmd = "az monitor log-analytics workspace create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " --sku $($resource.sku)"
        $cmd += " -l $location"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-Log-Analytics-Workspace | New-Resource-File

    $resource.servicePrincipal | Set-ServicePrincipal -ErrorFile $ErrorFile -OutputFile "$($resource.name)--service-pincipal.json"
}

function Get-Monitor-DataCollection-Endpoint(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az monitor data-collection endpoint show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
}
function Set-Monitor-DataCollection-Endpoint(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {    
    $result = $resource | Get-Monitor-DataCollection-Endpoint
    if ($null -eq $result) {
        $cmd = "az monitor data-collection endpoint create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " --public-network-access $($resource."public-network-access")"
        $cmd += " -l $location"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-Monitor-DataCollection-Endpoint | New-Resource-File
}

function Get-Monitor-DataCollection-Rule(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az monitor data-collection rule show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
}
function Set-Monitor-DataCollection-Rule(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [string] $dcrId, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {    
    $result = $resource | Get-Monitor-DataCollection-Rule
    if ($null -eq $result) {
        $endpoint = @{name = $resource.endpoint; resourceGroup = $resource.resourceGroup } | Get-Monitor-DataCollection-endpoint
        if ($null -eq $endpoint -or [string]::IsNullOrEmpty($endpoint.id)) {
            Write-Host "endpoint '$($resource.endpoint)' (rg='$($resource.resourceGroup)') not found, Data collection rules not created" -ForegroundColor Yellow
        }
        $cmd = "az monitor data-collection rule create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " --data-collection-endpoint-id $($endpoint.id)"
        $cmd += " -l $location"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-Monitor-DataCollection-Rule | New-Resource-File
}

function Get-Log-Analytics-Workspace-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az monitor log-analytics workspace table show -n $resource.name -g $resource.resourceGroup --workspace-name $resource.workspaceName 2>$null) | ConvertFrom-Json
}
function Set-Log-Analytics-Workspace-Table(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource,    
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {    
    $result = $resource | Get-Log-Analytics-Workspace-Table
    if ($null -eq $result) {
        $cmd = "az monitor log-analytics workspace table create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " --workspace-name $($resource.workspaceName)"
        $cmd += " --plan $($resource.plan)"
        $cmd += " --retention-time $($resource.retentionTime)"
        $cmd += " --total-retention-time $($resource.totalRetentionTime)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $resource | Get-Log-Analytics-Workspace-Table | New-Resource-File
}

function Get-ServicePrincipal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name
) {
    return $null#(az ad sp show --id "http://$name" 2>$null) | ConvertFrom-Json
}

function Set-ServicePrincipal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name,
    [Parameter(Mandatory = $true)]
    [object] $resource
) {
    $fullfileName = "$resourceFunctionRoot\resources\$($resource.name)--service-pincipal.json"
    $result = $name | Get-ServicePrincipal
    if ($null -eq $result) {
        $sp = az ad sp create-for-rbac --name $name --skip-assignment | ConvertFrom-Json
        Write-Host "Service principal '$name' created and saved to $OutputFile" -ForegroundColor Green
    }
    else {
        $sp = @{
            appId        = $result.appId
            tenant       = $result.appOwnerTenantId
            subscription = (az account show | ConvertFrom-Json).id
        }
        $sp.secret = (az ad sp credential reset --name $name | ConvertFrom-Json).password
        Write-Host "Service principal '$name' already exists" -ForegroundColor Yellow
    }
    $sp | New-Resource-File-Service-Principal -resource $resource
}