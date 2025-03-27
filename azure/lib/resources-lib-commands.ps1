using namespace System.IO;
using namespace System.Collections;

$resourcesCommandRoot = $PSScriptRoot
. $resourcesCommandRoot\resources-lib-azure.ps1
. $resourcesCommandRoot\resources-lib-file.ps1

$baseDir = ([DirectoryInfo]"$resourcesCommandRoot\..").FullName

function Login() {
    # upgrade az, there is some troubles when using dce and dcr (with the version : 2022-04-01)
    az config set auto-upgrade.enable=yes
    # equivalent to : az upgrade --yes *>$null

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

############################################################################################################
# Subscription
############################################################################################################


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

############################################################################################################
# Ressource Group
############################################################################################################

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
    [Hashtable] $references,
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
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $result = $resource | Get-RessourceGroup
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }
    $result | New-Resource-File
    $references[$resource.name] = $result

    $resource.servicePrincipals | ForEach-Object {
        $_ | Set-ServicePrincipal -references $references -scopes $result.id
    }
}

############################################################################################################
# Web App
############################################################################################################

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
    [Hashtable] $references,
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
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $result = $resource | Get-AppService-Plan
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | New-Resource-File
    $references[$resource.name] = $result
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
    [Parameter(Mandatory = $true)]
    [Hashtable] $references,    
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
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $result = $resource | Get-WebApp
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    # refresh the result with new appsettings config
    $result | New-Resource-File
    
    # set the appsettings after resource file creation becase data are sensitive
    $appSettings = $resource | Set-WebApp-AppSettings -references $references -ErrorFile $ErrorFile

    # add the appsettings to the references but not in the file
    $result | Add-Member -MemberType NoteProperty -Name "appSettings" -Value $appSettings
    $references[$resource.name] = $result
}

function Set-WebApp-AppSettings(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource,  
    [Parameter(Mandatory = $true)]
    [Hashtable] $references, 
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    if (-not $resource.siteConfig -or -not $resource.siteConfig.appSettings) { 
        Write-Host "no app settings to set (path : resources[kind=webapp].siteConfig.appSettings)" -ForegroundColor Yellow
        return 
    }

    # delete the file if it exists
    $name = "$($resource.name)-appsettings"
    $file = $name | Get-Secret-File
    if ($file.Exists) { $file.Delete() }

    # set values from the references
    $appSettings = @()
    foreach ($map in $resource.siteConfig.appSettings) {
        $item = @{
            name  = $map.name 
            value = $map | Get-Value-From -getReferenceFunc {
                param([string] $path)
                return $references.$path
            }
        }
        $appSettings += $item
    }
    $appSettings | ConvertTo-Json | Format-Json | Out-File -FilePath $file -Force
    Write-Host "  > stored : '.\.secrets\$($file.Name)'"

    # execute the command
    $cmd = "az webapp config appsettings set"
    $cmd += " -n $($resource.name)"
    $cmd += " -g $($resource.resourceGroup)"
    $cmd += " --settings @$($file.FullName)"
    $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
    
    return $appSettings
}

############################################################################################################
# Storage Account
############################################################################################################

function Get-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    return (az storage account show -n $resource.name -g $resource.resourceGroup 2>$null) | ConvertFrom-Json
}
function Set-Storage-Account(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [Hashtable] $references,
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
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $result = $resource | Get-Storage-Account 
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $connectionstringResult = (az storage account show-connection-string -n $resource.name -g $resource.resourceGroup) | ConvertFrom-Json
    $connectionstringResult | Add-Member -MemberType NoteProperty -Name "name" -Value "$($resource.name)-connectionstring"
    $result | Add-Member -MemberType NoteProperty -Name "connectionString" -Value $connectionstringResult.connectionString

    $connectionstringResult | New-Secret-File
    $result | New-Resource-File
    $references[$resource.name] = $result
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


############################################################################################################
# Log Analytics Workspace
############################################################################################################

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
    [Hashtable] $references,
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
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $result = $resource | Get-Log-Analytics-Workspace
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | New-Resource-File
    $references[$resource.name] = $result
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
    [Hashtable] $references, 
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
        
        $attempt = 1; $maxAttempts = 3
        while ($attempt -le $maxAttempts) {
            try {
                $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null    
            }
            catch {                
                if ($attempt -gt $maxAttempts) { 
                    Write-Error "command for '$($resource.name)' creation fails too many times. see above error message"
                    throw $_
                }
                $attempt++
                Start-Sleep -Seconds 10 # Délai avant la nouvelle tentative
                # open the line for attempt messages
                Write-Host "[!] retrying the creation of '$($resource.name)' (attempt : $attempt/$maxAttempts)" -ForegroundColor Yellow
            }
        }
        
        $result = $resource | Get-Log-Analytics-Workspace-Table
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    if ($attempt -eq 0) {
        # pas la peine de sauvegarder 3 fois le fichier..
        $result | New-Resource-File
        $references[$resource.name] = $result
    }
}


############################################################################################################
# Monitor Data Collection
############################################################################################################

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
    [Hashtable] $references,
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
        $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
        $result = $resource | get-Monitor-DataCollection-Endpoint
    }
    else {
        Write-Host "resource '$($resource.name)' already exists" -ForegroundColor Yellow
    }

    $result | New-Resource-File
    $references[$resource.name] = $result
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
    [Hashtable] $references, 
    [Parameter(Mandatory = $true)]
    [string] $location, 
    [Parameter()]
    [object[]] $tags,
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile
) {
    # always update the dcr !
    $msg = "Updated"
    if ($null -eq ($resource | Get-Monitor-DataCollection-Rule)) { $msg = "Created" }
    
    $ruleFile = $resource | Set-Rule-File -references $references 
    
    $cmd = "az monitor data-collection rule create"
    $cmd += " -n $($resource.name)"
    $cmd += " -g $($resource.resourceGroup)"
    $cmd += " -l $location"
    $cmd += " --rule-file ""$($ruleFile.FullName)"""
    $cmd += " --tags $(Get-Tags-AsKeyValue-ToString -tags $tags)"
    $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile | Out-Null
    
    Write-Host "resource '$($resource.name)' is $msg" -ForegroundColor Yellow    

    $result = $resource | Get-Monitor-DataCollection-Rule 
    $result | New-Resource-File
    $references[$resource.name] = $result
}

function Set-Rule-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource, 
    [Parameter(Mandatory = $true)]
    [Hashtable] $references
) {
    $file = [FileInfo]"$baseDir\$($resource.template.file)"
    if (-not $file.Exists) {
        throw "file not found : '$($file.FullName)'"
    }
        
    $ruleFile = "$($resource.name)-sent" | Get-Secret-File
    if ($ruleFile.Exists) { $ruleFile.Delete() }
    $rules = (Get-Content -Path $file.FullName) -join " "
    $rules > $ruleFile.FullName
    foreach ($map in $resource.template.mapping) {
        $value = $map | Get-Value-From -getReferenceFunc {
            param([string] $path)
            return $references.$path
        }        
        $rules = $rules -replace $map.placeholder, $value
    }

    $rules > $ruleFile.FullName
    return $ruleFile
}