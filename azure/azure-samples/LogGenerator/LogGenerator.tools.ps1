function Get-Usecase(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string]$value, 
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [int] $index, 
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [int] $partsCount) {
    if ($partsCount -eq 5) {
        switch ($index) {
            0 { return "TimeGenerated" }
            1 { return "Level" }
            2 { return "Message" }
            3 { return "Context" }
            4 { return "Properties" }
        }
    }
    else {
        switch ($index) {
            0 { return "TimeGenerated" }
            1 { return "Level" }
            2 { return "Status" }
            3 { return "Method" }
            4 { return "Message" }
            5 { return "Elapsed" }
            6 { return "Context" }
            7 { return "Properties" }
        }
    }
}
function Set-Properties(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [hashtable] $ref,
    [string] $value
) {
    try {     
        # act : add properties to do not Jsonify     
        if ($value -match "ExceptionDetail: \[(?<content>.*)\]" ) {
            # "[$($matches["content"])]" | ConvertFrom-Python-ToJson <# | ForEach-Object {
            #     $ref.Add($_.Name, $_.Value)
            # } #>
            $ref.Add("ExceptionDetail", "[$($matches["content"])]")
            $value = $value -replace ", ExceptionDetail: .*\]", ""
            
        }
        
        $props = $value | ConvertFrom-Json 
        foreach ($prop in $props.PSObject.Properties) {
            $ref.Add($prop.Name, $prop.Value) 
        }
    }
    catch {                        
        Write-Error $Error[0]
    }                        
}

function ConvertFrom-Python-ToJson(
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string]$entry) {         
    # Nettoyage initial
    $buffer = $entry.TrimStart('[').TrimEnd(']').Trim()

    $result = [hashtable]@{}
    try {               
        while ($buffer.Length -gt 0) {        
            $nextPropertyIndex = $buffer.indexOf(", ")
            $property = $buffer.Substring(0, $nextPropertyIndex).TrimStart("(").TrimEnd(")")
                
            if ($property -match '^"(?<key>.*)":\s"(?<value>.*)"$') {
                $result.Add($matches["key"], $matches["value"])
            }

            $buffer = $buffer.Substring($nextPropertyIndex + 2)
        }
    }
    catch {
        <#Do this if a terminating exception happens#>
        Write-Error $Error[0]
    }
    
    # Affichage du résultat
    return $result | Format-List
}

function ConvertFrom-Python-ToJson-NestedElements ( 
    [Parameter(ValueFromPipeline = $true, Mandatory = $true)]
    [string]$element
) {
    if ($element.StartsWith('[')) {
        # Traiter comme un tableau
        $element = $element.TrimStart('[').TrimEnd(']')
        $items = $element.Split('], [')
        $resultArray = @()
        foreach ($item in $items) {
            $resultArray += $item | ConvertFrom-Python-ToJson-NestedElements
        }
        return $resultArray
    }
    elseif ($element.StartsWith('(')) {
        # Traiter comme un objet
        $element = $element.TrimStart('(').TrimEnd(')')
        $properties = $element.Split('), (')
        $resultObject = [PSCustomObject]@{}
        foreach ($property in $properties) {
            $keyValue = $property.Split(': ')
            $key = $keyValue[0].Trim('""')
            $value = $keyValue[1].Trim('""')
            $resultObject.$key = $value
        }
        return $resultObject
    }
    else {
        # Valeur simple
        return $element
    }
}

