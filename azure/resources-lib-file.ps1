$resourcesLibFileRoot = $PSScriptRoot

function Clear-Error-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $ErrorFile
) {
    if (Test-Path $ErrorFile) { 
        Remove-Item $ErrorFile 
    }
}

function Clear-Resources-Files {
    Remove-Item -Path "$($resourcesLibFileRoot)\resources\*" `
        -Exclude ".gitkeep", "*--service-principal.json" `
        -Recurse -Force 
}

function Get-Settings {
    get-content -path "$($resourcesLibFileRoot)\resources.json" -Raw | ConvertFrom-Json
}

function New-Resource-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    $file = "\resources\$($resource.name).json"
    $fullfileName = Join-Path $resourcesLibFileRoot $file
    $resource | ConvertTo-Json | Out-File -FilePath $fullfileName -Force
    Write-Host "resource '$($resource.name)' stored in '.$file'"
}
function New-Resource-File-Service-Principal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $servicePrincipal,
    [Parameter(Mandatory = $true)]
    [object] $resource
) {
    $file = "\resources\$($resource.name)--service-principal.json"
    $fullfileName = Join-Path $resourcesLibFileRoot $file
    $servicePrincipal | ConvertTo-Json | Out-File -FilePath $fullfileName -Force
    Write-Host "service principal '$($servicePrincipal.displayName)' stored in '.$file'"
}