using namespace System.IO;

$resourcesLibFileRoot = $PSScriptRoot

$rootDir = ([DirectoryInfo]"$resourcesLibFileRoot\..\..").FullName
$baseDir = ([DirectoryInfo]"$resourcesLibFileRoot\..").FullName
$global:resourcesDir = ([DirectoryInfo]"$rootDir\.resources").FullName
$global:secretsDir = ([DirectoryInfo]"$rootDir\.secrets").FullName

function Set-Resources-Folders(
    [Parameter(Mandatory = $true)]
    [array] $subs
) {
    $temps = @($rootDir, "<template>")
    $temps = $temps + $subs
    $segments = @()
    foreach ($temp in $temps) { $segments += $temp.Trim("/").Trim("\") }
    $path = $segments -join "/"

    $global:resourcesDir = ([DirectoryInfo]($path -replace "<template>", ".resources")).FullName
    if (-not(Test-Path $global:resourcesDir)) { New-Item -ItemType Directory -Path $global:resourcesDir }

    $global:secretsDir = ([DirectoryInfo]($path -replace "<template>", ".secrets")).FullName
    if (-not(Test-Path $global:secretsDir)) { New-Item -ItemType Directory -Path $global:secretsDir }
}
function Clear-Error-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $ErrorFile
) {
    if (Test-Path $ErrorFile) { 
        Remove-Item $ErrorFile 
    }
}

function Clear-Resources-Files {  
    Remove-Item -Path "$global:resourcesDir\*" -Recurse -Force 
}
function Clear-Secrets-Files {  
    Remove-Item -Path "$global:secretsDir\*" -Recurse -Force 
}

$script:settings = $null
function Get-Settings([string] $name) {
    if ($null -eq $script:settings) {
        $path = "$baseDir\resources-$name.json"
        if (-not(Test-Path $path)) {
            throw "No settings found in '$path'"
        }
        $script:settings = Get-Content -path $path -Raw | ConvertFrom-Json
        if ($null -eq $script:settings) {
            throw "settings file can not be read or is empty. file: '$path'"
        }
    }
    return $script:settings
}

function New-Resource-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    $file = $resource.name | Get-Ressource-File
    $resource | ConvertTo-Json | Format-Json | Out-File -FilePath $file -Force
    Write-Host "  > stored : '.\.resources\$($file.Name)'"
}
function Get-Ressource-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name,
    [string] $extension = "json"
) {
    return [FileInfo](Join-Path $global:resourcesDir "$name.$extension")
}

function New-Secret-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $resource
) {
    $file = $resource.name | Get-Secret-File
    $resource | ConvertTo-Json | Format-Json | Out-File -FilePath $file -Force
    Write-Host "  > stored : '.\.secrets\$($file.Name)'"
}

function Get-Secret-File(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string] $name,
    [string] $extension = "json"
) {
    return [FileInfo](Join-Path $global:secretsDir "$name.$extension")
}

function New-Resource-File-Service-Principal(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $servicePrincipal
) {
    $spFile = $servicePrincipal.displayName | Get-Secret-File
    $servicePrincipal | ConvertTo-Json | Format-Json | Out-File -FilePath $spFile.FullName -Force
    Write-Host "service principal stored in '.\.secrets\$($spFile.Name)'"
}

# code found at : https://stackoverflow.com/questions/56322993/proper-formating-of-json-using-powershell
function Format-Json {
    <#
    .SYNOPSIS
        Prettifies JSON output.
        Version January 3rd 2024
        Fixes:
            - empty [] or {} or in-line arrays as per https://stackoverflow.com/a/71664664/9898643
              by Widlov (https://stackoverflow.com/users/1716283/widlov)
            - Unicode Apostrophs \u0027 as written by ConvertTo-Json are replaced with regular single quotes "'"
            - multiline empty [] or {} are converted into inline arrays or objects
    .DESCRIPTION
        Reformats a JSON string so the output looks better than what ConvertTo-Json outputs.
    .PARAMETER Json
        Required: [string] The JSON text to prettify.
    .PARAMETER Minify
        Optional: Returns the json string compressed.
    .PARAMETER Indentation
        Optional: The number of spaces (1..1024) to use for indentation. Defaults to 2.
    .PARAMETER AsArray
        Optional: If set, the output will be in the form of a string array, otherwise a single string is output.
    .EXAMPLE
        $json | ConvertTo-Json | Format-Json -Indentation 4
    .OUTPUTS
        System.String or System.String[] (the latter when parameter AsArray is set)
    #>
    [CmdletBinding(DefaultParameterSetName = 'Prettify')]
    Param(
        [Parameter(Mandatory = $true, Position = 0, ValueFromPipeline = $true)]
        [string]$Json,

        [Parameter(ParameterSetName = 'Minify')]
        [switch]$Minify,

        [Parameter(ParameterSetName = 'Prettify')]
        [ValidateRange(1, 1024)]
        [int]$Indentation = 2,

        [Parameter(ParameterSetName = 'Prettify')]
        [switch]$AsArray
    )

    if ($PSCmdlet.ParameterSetName -eq 'Minify') {
        return ($Json | ConvertFrom-Json) | ConvertTo-Json -Depth 100 -Compress
    }

    # If the input JSON text has been created with ConvertTo-Json -Compress
    # then we first need to reconvert it without compression
    if ($Json -notmatch '\r?\n') {
        $Json = ($Json | ConvertFrom-Json) | ConvertTo-Json -Depth 100
    }

    $indent = 0
    $regexUnlessQuoted = '(?=([^"]*"[^"]*")*[^"]*$)'

    $result = ($Json -split '\r?\n' | ForEach-Object {
            # If the line contains a ] or } character, 
            # we need to decrement the indentation level unless:
            #   - it is inside quotes, AND
            #   - it does not contain a [ or {
            if (($_ -match "[}\]]$regexUnlessQuoted") -and ($_ -notmatch "[\{\[]$regexUnlessQuoted")) {
                $indent = [Math]::Max($indent - $Indentation, 0)
            }

            # Replace all colon-space combinations by ": " unless it is inside quotes.
            $line = (' ' * $indent) + ($_.TrimStart() -replace ":\s+$regexUnlessQuoted", ': ')

            # If the line contains a [ or { character, 
            # we need to increment the indentation level unless:
            #   - it is inside quotes, AND
            #   - it does not contain a ] or }
            if (($_ -match "[\{\[]$regexUnlessQuoted") -and ($_ -notmatch "[}\]]$regexUnlessQuoted")) {
                $indent += $Indentation
            }

            # ConvertTo-Json returns all single-quote characters as Unicode Apostrophs \u0027
            # see: https://stackoverflow.com/a/29312389/9898643
            $line -replace '\\u0027', "'"

            # join the array with newlines and convert multiline empty [] or {} into inline arrays or objects
        }) -join [Environment]::NewLine -replace '(\[)\s+(\])', '$1$2' -replace '(\{)\s+(\})', '$1$2'

    if ($AsArray) { return , [string[]]($result -split '\r?\n') }
    $result
}

function Get-Value-From(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [PsCustomObject] $map,
    [scriptblock] $getReferenceFunc
) {
    return Get-Settings | Get-Value -map $map -getReferenceFunc $getReferenceFunc
}
    
function Get-Value(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [object] $reference,
    [Parameter(Mandatory = $true)]
    [PsCustomObject] $map,
    [scriptblock] $getReferenceFunc
) {
    if ($map.value ) { 
        return $map.value
    }
    
    $ref = $reference
    $paths = $map.path.Split(".")
    foreach ($p in $paths) {
        if ($p.StartsWith('$ref|') ) {
            $value = $p.Substring('$ref|'.Length)
            if (-not $getReferenceFunc) {
                throw 'getReferenceFunc is mandatory when using ''$path'' with $ref| argument'
            }
            $ref = $getReferenceFunc.Invoke($value) | select-object -first 1
            continue
        }
        elseif ($p -match "(?<property>.*)\[(?<filter_property>.*)='(?<filter_value>.*)'\]") {
            $property = $matches['property']
            $filter_property = $matches['filter_property']
            $filter_value = $matches['filter_value']
            $ref = $ref.$property | Where-Object { $_.$filter_property -eq $filter_value }
            if ($ref -is [array]) {
                throw "'$p' has an invalid filter, because ther is more than one element"
            }
            continue
        }
        
        $ref = $ref.$p
    }
    return $ref
}