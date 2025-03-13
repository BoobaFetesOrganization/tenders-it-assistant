$resourcesLibAzureRoot = $PSScriptRoot
. $resourcesLibAzureRoot\resources-lib-file.ps1

function Invoke-Az-Command(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $cmd,    
    [Parameter(Mandatory = $true)]
    [string] $name, 
    [Parameter(Mandatory = $true)]
    [string] $ErrorFile,
    [Parameter()]
    [ScriptBlock] $testFalsePositiveAct
) {
    $response = Invoke-Expression $cmd 2>$ErrorFile
    $errorFileExistsAct = { Test-Path $ErrorFile }
    
    # delete false positive error file : error file exist but has no content
    $isFalsePositive = $null -eq (Get-Content -Path $ErrorFile)
    if ($testFalsePositiveAct) { $isFalsePositive = $isFalsePositive -or (& $testFalsePositiveAct) }
    if ((& $errorFileExistsAct) -and $isFalsePositive) { 
        Remove-Item -Path $ErrorFile -Force 
    }

    $hasError = & $errorFileExistsAct
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
    return $response | ConvertFrom-Json
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

function Get-ServicePrincipal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name
) {
    return (az ad sp list --filter "displayname eq '$name'" | ConvertFrom-Json)[0]
}

function Set-ServicePrincipal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name,
    [Parameter(Mandatory = $true)]
    [object] $resource
) {
    $result = $name | Get-ServicePrincipal
    if ($null -eq $result) {
        $resourceGroup = (az group show -n $resource.resourceGroup 2>$null) | ConvertFrom-Json
        
        $cmd = "az ad sp create-for-rbac"
        $cmd += " -n $name"
        $cmd += " --role Contributor"
        $cmd += " --scopes $($resourceGroup.id)"
        $servicePrincipal = $cmd | Invoke-Az-Command -name $resource.name -ErrorFile $ErrorFile -testFalsePositiveAct { 
            $errorContent = Get-Content $ErrorFile
            return $errorContent[7] -match "The output includes credentials that you must protect" 
        }
        
        $servicePrincipal | New-Resource-File-Service-Principal -resource $resource
    }
    else {
        Write-Host "service principal '$($result.appDisplayName)' already exists" -ForegroundColor Yellow
    }
}