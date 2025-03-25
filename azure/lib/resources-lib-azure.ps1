using namespace System.IO;

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
    [ScriptBlock] $TestIsValidFunc
) {
    $response = Invoke-Expression $cmd 2>$ErrorFile
    $err = [FileInfo] $ErrorFile
    
    # when error.log exists, the command should returns an error message..
    $hasError = $err.Exists
    if ($hasError) { 
        $content = Get-Content -Path $ErrorFile -Raw
        # .. but the error.log file can be empty
        if ([string]::IsNullOrWhiteSpace($content)) { $hasError = $false }
        # .. or contains a false error message
        elseif ($TestIsValidFunc) { 
            $hasError = -not $TestIsValidFunc.Invoke($content)[0]
        }
        # .. or contains a false error message from az cli extensions
        $hasWarning = { param([string]$content)
            $falseErrors = @()
            $falseErrors += $content -match "^az : .*\\.azure\\cliextensions(.|\r\n)*invalid escape(.|\r\n)*Create a data collection rule\.\r\n$"  
            $falseErrors += $content -match "^az :.* UserWarning: You are using cryptography(.|\r\n)*NativeCommandError(.|\r\n)*\r\n$"   
            $falseErrors += $content -match "^.* UserWarning: You are using cryptography(.|\r\n)*resources-create.ps1(\r\n)*\r\n$"   
            $falseErrors += $content -match "^az : WARNING(.|\r\n)*The output includes credentials that you must protect(.|\r\n)*https://aka.ms/azadsp-cli\r\n$"
            return ($falseErrors | Where-Object { $_ -eq $true }).Count -gt 0
        }
        if ($hasError -and $hasWarning.invoke($content)[0]) {
            $hasError = $false
        }
        # so, when the error.log contains a false error message, it is deleted
        if (-not $hasError) { 
            Remove-Item -Path $ErrorFile -Force 
        }

        return $response | ConvertFrom-Json
    }

    if ($hasError) {
        Write-Host "command for '$name' fails" -ForegroundColor Red
        Write-Host ""
        Write-Host " > Command : " -ForegroundColor Red
        Write-Host $cmd -ForegroundColor Red
        Write-Host ""
        Write-Host " > Error : " -ForegroundColor Red
        Write-Host $content -ForegroundColor Red
        throw "command for '$name' creation fails. see above error message"
    }

    Write-Host "command for '$name' succesfully executed" -ForegroundColor Green
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
    [object] $sp
) {
    return (az ad sp list --filter "displayname eq '$($sp.name)'" | ConvertFrom-Json)[0]
}

function Set-ServicePrincipal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $sp,
    [Parameter(Mandatory = $true)]
    [Hashtable] $references,  
    [Parameter(Mandatory = $true)]
    [string] $scopes
) {
    $result = $sp | Get-ServicePrincipal
    if ($null -eq $result) {
        $cmd = "az ad sp create-for-rbac"
        $cmd += " -n $($sp.name)"
        $cmd += " --role ""$($sp.role)"""
        $cmd += " --scopes $scopes"
        $servicePrincipal = $cmd | Invoke-Az-Command -name $sp.name -ErrorFile $ErrorFile        
        $servicePrincipal | New-Resource-File-Service-Principal
    }
    else {
        Write-Host "service principal '$($result.appDisplayName)' already exists" -ForegroundColor Yellow
        $spFile = $sp.name | Get-Secret-File
        $servicePrincipal = Get-Content -Path $spFile.FullName -Raw | ConvertFrom-Json
    }

    $references[$sp.name] = $servicePrincipal
}