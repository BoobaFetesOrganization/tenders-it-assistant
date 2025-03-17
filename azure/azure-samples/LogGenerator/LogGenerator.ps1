
# code fortement modifié pour la transformation en fichier json 
# ensuite un petit coup deGenAi pour demander une transformation pour les colonne d'une custom table d'un Log Analytics Workspace
# s'en suit un autre prompt pour générer le schema du DCR
# code from : https://learn.microsoft.com/en-us/azure/azure-monitor/logs/tutorial-logs-ingestion-portal#generate-sample-data
param (
    [Parameter(Mandatory = $true)] $login,
    [Parameter(Mandatory = $true)] $Log, 
    [Parameter(Mandatory = $true)] $Output
)

$ErrorActionPreference = "Stop"

$scriptRoot = $PSScriptRoot
. $scriptRoot\LogGenerator.tools.ps1


# private functions

################
##### Usage
################
# LogGenerator.ps1
#   -Log <String>              - Log file to be forwarded
#   [-Type "file|API"]         - Whether the script should generate sample JSON file or send data via
#                                API call. Data will be written to a file by default.
#   [-Output <String>]         - Path to resulting JSON sample


$file_data = Get-Content $Log
############
## Convert plain log to JSON format and output to .json file
############
# If not provided, get output file name
if ($null -eq $Output) {
    $Output = Read-Host "Enter output file name" 
};

# Form file payload
$payload = @();
$records_to_generate = [math]::min($file_data.count, 500)
for ($i = 0; $i -lt $records_to_generate; $i++) {
    $properties = [hashtable]@{RawData = [string]$file_data[$i] }
    $parts = $properties.RawData -split "--"
        
    for ($index = 0 ; $index -lt $parts.Count ; $index++) {
        $value = $parts[$index].Trim()
        $uc = $value | Get-Usecase -index $index -partsCount $parts.Count            
        switch ($uc) {
            "Properties" { 
                $properties | Set-Properties -value $value
            }
            default {
                $properties.Add($uc, $value) 
            }
        }
    }

    $payload += $properties
}
# Write resulting payload to file
New-Item -Path $Output -ItemType "file" -Value ($payload | ConvertTo-Json) -Force
