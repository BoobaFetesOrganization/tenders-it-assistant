# $ErrorActionPreference = "Stop"
$scriptroot = $PSScriptRoot
Clear-Host

# 1. Se connecter à Azure
# az login --use-device-code

$spSettings = Get-Content -Path $scriptroot/resources/tenders-it-assistant-log-analytics-sp--service-principal.json -Raw | ConvertFrom-Json
$dceSettings = Get-Content -Path $scriptroot/resources/tenders-it-assistant-dce.json -Raw | ConvertFrom-Json
$dcrSettings = Get-Content -Path $scriptroot/resources/tenders-it-assistant-WebAppLogs-dcr.json -Raw | ConvertFrom-Json


# # gemini : vérification demandées
# $workspaceId = $logAnalytics.customerId
# # Requête KQL pour vérifier la présence de la table et sa connectivité
# az monitor log-analytics query --workspace $workspaceId --analytics-query "$dcrSteam | take 1"

# 2. Obtenez le jeton d'accès

$tenantId = $spSettings.tenant      # Identifiant du locataire Azure
$appId = $spSettings.appId          # Identifiant de l'application (Client ID)
$appSecret = $spSettings.password   # Secret de l'application
$dcrImmutableId = $dcrSettings.immutableId
$endpoint = $dceSettings.logsIngestion.endpoint

Write-Host "variables:"
write-host " > tenantId: $tenantId"
write-host " > appId: $appId"
write-host " > appSecret: $appSecret"
write-host " > dcrImmutableId: $dcrImmutableId"
write-host " > streamName: $streamName"
write-host " > endpoint: $endpoint"

$tokenArgs = @{
    Uri         = "https://login.microsoftonline.com/$tenantId/oauth2/token"
    Method      = "Post"
    ContentType = "application/x-www-form-urlencoded"
    Body        = @{
        grant_type    = "client_credentials"
        client_id     = $appId
        client_secret = $appSecret
        scope         = "https://monitor.azure.com"
    }
}
$response = Invoke-RestMethod @tokenArgs
$accessToken = $response.access_token
Write-Host " > accessToken"
Write-Host "$accessToken"
Write-Host

# 3. Faire l'appel POST à l'API
$streamNames = @()
foreach ($flow in $dcrSettings.dataFlows) {
    if ($flow.streams -is [array]) { 
        foreach ($stream in $flow.streams) {
            $streamNames += $stream
        }
    }
    else { 
        $streamNames += $flow.streams 
    }
}

foreach ($sn in $streamNames) {
    $request = @{
        Uri         = "$endpoint/dataCollectionRules/$dcrImmutableId/streams/$($sn)?api-version=2023-01-01"
        Method      = "Post"
        ContentType = "application/json"
        Headers     = @{ Authorization = "Bearer $accessToken" }
        Body        = '{
            "Time":  "2025-03-14 19:21:54.689 +01:00",
            "Computer":  "axel",
            "AdditionalContext":  "2025-03-14 19:21:54.689 +01:00 -- INF -- 200 -- GET -- /swagger/index.html -- 46.5328 -- Serilog.AspNetCore.RequestLoggingMiddleware -- { MimeType: \"text/html;charset=utf-8\", Host: \"localhost:3001\", Scheme: \"https\", RequestId: \"0HNB34KCHSM2S:00000001\", ConnectionId: \"0HNB34KCHSM2S\", MachineName: \"DRAGON\", ThreadId: 9 }",
        }'
    }
    write-host " > uri: [$($request.Method)] $($request.Uri)"
    $response = Invoke-RestMethod @request 
}
