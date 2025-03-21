$resourcesCommandRoot = $PSScriptRoot
. $resourcesCommandRoot\resources-lib-azure.ps1
. $resourcesCommandRoot\resources-lib-file.ps1

function Login() {
    # disable the subscription selector feature once logged in
    az config set core.login_experience_v2=off
    # enable dynamic install
    az config set extension.use_dynamic_install=yes_without_prompt
    
    # login with device code
    $subscription = az login --use-device-code | ConvertFrom-Json | Select-Object -First 1

    if ($subscription -is [array]) { $subscription = $subscription[0] }
    if (-not $subscription) { throw "Login failed" }
    Write-Host "login success" -ForegroundColor Yellow

    return $subscription
}

function Get-Subscription(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name
) {
    return (az account show -n $name 2>$null) | ConvertFrom-Json
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
    return (az group show -n $resource.name 2>$null) | ConvertFrom-Json
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }
    $result | Get-RessourceGroup | New-Resource-File
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile -testFalsePositiveAct { 
            $errorContent = Get-Content $ErrorFile
            return $errorContent[10] -match "CryptographyDeprecationWarning" 
        }
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-AppService-Plan | New-Resource-File
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile -testFalsePositiveAct { 
            $errorContent = Get-Content $ErrorFile
            return $errorContent[10] -match "CryptographyDeprecationWarning" 
        }
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-WebApp | New-Resource-File

    $resource.servicePrincipal | Set-ServicePrincipal -resource $resource
}

function Get-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az storage account show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile 
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Storage-Account | New-Resource-File
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
    if (-not $tableStorage.exists) {
        $cmd = "az storage table create"
        $cmd += " -n $($resource.name)"
        $cmd += " --account-name $($resource.account)"
        $cmd += " --auth-mode login"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }
}

function Get-Log-Analytics-Workspace(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az monitor log-analytics workspace show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile 
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Log-Analytics-Workspace | New-Resource-File

    $resource.servicePrincipal | Set-ServicePrincipal -resource $resource
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
        $cmd += " --kind $($resource."azure-kind")"
        $cmd += " -l $location"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile 
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Monitor-DataCollection-Endpoint | New-Resource-File
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
        $cmd += " --kind $($resource."azure-kind")"
        $cmd += " -l $location"
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile 
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Monitor-DataCollection-Rule | New-Resource-File
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile 
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Log-Analytics-Workspace-Table | New-Resource-File
}