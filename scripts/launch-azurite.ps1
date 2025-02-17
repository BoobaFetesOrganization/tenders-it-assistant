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
    Path = "C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\Extensions\Microsoft\Azure Storage Emulator";
    File = "azurite.exe";
}

# act
try {
    
    if (-not (Test-Path $var.location)) {
        New-Item -ItemType Directory -Path $var.location
    }
    
    [FileInfo]$executable = Join-Path -Path $azurite.Path -ChildPath $azurite.File
    if (-not $executable.Exists) {
        throw "The azurite executable not exists, install azure development tools from Visual Studio 2022"
    }

    
    [ArrayList]$envVarPaths = [Environment]::GetEnvironmentVariable("PATH", "User") -split ';'
    $userEnvPathsContainsAzuriteDirectory = ($envVarPaths | Where-Object { $_ -ieq $azurite.Path } | Select-Object -First 1) -ne $null
    if (-not $userEnvPathsContainsAzuriteDirectory) {
        $envVarPaths.Add($azurite.Path)
        [Environment]::SetEnvironmentVariable("PATH", $envVarPaths -join ';', "User")
    }

    $arguments = [ArrayList]@()
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
    
    $azurite = Start-Process `
        -FilePath $executable.FullName `
        -ArgumentList ($arguments -join ' ') `
        -PassThru 
}
catch {
    Write-Host "an error occured: $_"
}