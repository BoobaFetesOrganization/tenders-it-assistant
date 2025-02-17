using namespace System;
using namespace System.IO;
using namespace System.Collections;

param(
    [Parameter(Mandatory = $true)]
    [ValidateSet("add", "remove", "update-database")]
    [string[]]$actions,
    [string]$migration
)

# arrange
$var = @{
    location = "$PSScriptRoot/..\src\back\GenAIChat\GenAIChat.Infrastructure.Database.Sqlite.Migrations";    
}
function Test-Migrations() {
    [DirectoryInfo]$migrationsDir = "$($var.location)/Migrations"
    return $migrationsDir.Exists -and $migrationsDir.GetFiles().Length -gt 0
}
# act
try {
    if ($actions -contains "remove" ) { 
        dotnet ef migrations remove -s $var.location
    }
    
    if ($actions -contains "add") {
        if ([String]::IsNullOrEmpty($migration)) {
            $migration = "update"
        }
        if (-not (Test-Migrations)) {
            $migration = "InitialCreate"
        }
        dotnet ef migrations add $migration -s $var.location
    }
    if ($actions -contains "update-database") { 
        if (Test-Migrations) {
            dotnet ef database update -s $var.location
        }
    }
}
catch {
    Write-Host "an error occured: $_"
}