$root = $PSScriptRoot
$secretsFile = "$root/../.secrets/env-variables.json"

. "$root/set-dev-secrets.ps1" -secretsFile $secretsFile