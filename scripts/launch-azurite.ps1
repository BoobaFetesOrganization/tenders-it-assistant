using namespace System;
using namespace System.IO;
using namespace System.Collections;

# arrange
$var = @{
    debug    = "$PSScriptRoot/../azure/localhost/azurite.log";
    location = "$PSScriptRoot/../azure/localhost/azurite";
    # valeur par defaut => blobHost  = "127.0.0.1";
    # valeur par defaut => blobPort  = 10000;
    # valeur par defaut => queueHost = "127.0.0.1"
    # valeur par defaut => queuePort = 10001
    # valeur par defaut => tableHost = "127.0.0.1"
    # valeur par defaut => tablePort = 10002
}

$azurite = @{
    FullFileName = (yarn global bin) + "\azurite.cmd"
}

# act
try {
    # vérifie que azurite est installé avec yarn et le mete à jour
    if ($null -eq (yarn global list | Select-String "azurite@")) {
        yarn global add azurite
    }
    yarn global upgrade azurite

    if (-not (Test-Path $var.location)) {
        New-Item -ItemType Directory -Path $var.location
    }
    
    $arguments = [ArrayList]@("azurite")
    foreach ($key in $var.Keys) {
        Write-Host "$key = $($var[$key])"
        switch ($key) {
            "blobHost" { [void]$arguments.Add("--blobHost $($var[$key])") }
            "blobPort" { [void]$arguments.Add("--blobPort $($var[$key])") }
            "debug" { [void]$arguments.Add("--debug $($var[$key])") } 
            "inMemoryPersistence" { if ($var[$key]) { [void]$arguments.Add("--inMemoryPersistence") } }
            "location" { [void]$arguments.Add("--location $($var[$key])") }
            "tableHost" { [void]$arguments.Add("--tableHost $($var[$key])") }
            "tablePort" { [void]$arguments.Add("--tablePort $($var[$key])") }
            Default {}
        }
    }
    
    $command = "$($azurite.FullFileName) $($arguments -join ' ')"
    Start-Process powershell.exe -ArgumentList "-NoExit", "-Command", $command
}
catch {
    Write-Host "an error occured: $_"
}