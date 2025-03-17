using namespace System.IO;
using namespace System.Collections;

$resourcesCommandRoot = $PSScriptRoot
. $resourcesCommandRoot\resources-lib-azure.ps1
. $resourcesCommandRoot\resources-lib-file.ps1

$baseDir = ([DirectoryInfo]"$resourcesCommandRoot\..").FullName

function Login() {
    # upgrade az, there is some troubles when using dce and dcr (with the version : 2022-04-01)
    az config set auto-upgrade.enable=yes *>$null
    # equivalent to : az upgrade --yes *>$null

    # disable the subscription selector feature once logged in
    az config set core.login_experience_v2=off *>$null
    # enable dynamic install
    az config set extension.use_dynamic_install=yes_without_prompt *>$null
    
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

    $spList = $resource.servicePrincipals
    if ($null -eq $spList -or -not($spList -is [array])) { 
        $spList = @() 
    }
        
    foreach ($sp in $spList) {
        $sp | Set-ServicePrincipal -scopes $result.id
    }

    return $result
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-WebApp | New-Resource-File
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
    return $result
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
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Monitor-DataCollection-Endpoint | New-Resource-File
    return $result
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
        $file = [FileInfo]"$baseDir\$($resource.columnsFile)"
        if (-not $file.Exists) {
            throw "file '$file' not found"
        }
        $columns = (Get-Content -Path $file.FullName) -join " "
        $cmd = "az monitor log-analytics workspace table create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " --workspace-name $($resource.workspaceName)"
        $cmd += " --plan $($resource.plan)"
        $cmd += " --columns $($columns)"
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $resource | Get-Log-Analytics-Workspace-Table | New-Resource-File
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
        $result | New-Resource-File
    }
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
    [string] $location, 
    [Parameter(Mandatory = $true)]
    [Hashtable] $references, 
    [Parameter()]
    [object[]] $tags,
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    $result = $resource | Get-Monitor-DataCollection-Rule
    if ($null -eq $result) {
        $ruleFile = $resource | Set-Rule-File -references $references        

        $cmd = "az monitor data-collection rule create"
        $cmd += " -n $($resource.name)"
        $cmd += " -g $($resource.resourceGroup)"
        $cmd += " -l $location"
        $cmd += " --rule-file ""$($ruleFile.FullName)"""
        $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
        $result = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | Get-Monitor-DataCollection-Rule | New-Resource-File
}

function Set-Rule-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [Hashtable] $references
) {
    $file = [FileInfo]"$baseDir\$($resource.template.file)"
    if (-not $file.Exists) {
        throw "file '$file' not found"
    }
        
    $ruleFile = [FileInfo]"$baseDir\resources\$($resource.name)-sent--keep.json"    
    if ($ruleFile.Exists) { $ruleFile.Delete() }
    $rules = (Get-Content -Path $file.FullName) -join " "
    $rules > $ruleFile.FullName
    foreach ($map in $resource.template.mapping) {
        if ($map.path) { 
            $value = Get-Value-From -path $map.path -getReferenceFunc {
                param([string] $path)
                return $references.$path
            }
        }
        else { 
            $value = $map.value 
        }
        $rules = $rules -replace $map.placeholder, $value
        $rules > $ruleFile.FullName
    }

    return $ruleFile
}