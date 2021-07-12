$myDocumentsPath = [Environment]::GetFolderPath("MyDocuments")
$secretsPath = Join-Path -Path $myDocumentsPath -ChildPath "/Secrets/StopwatchExample.secrets.json"
$secrets = Get-Content $secretsPath | Out-String | ConvertFrom-Json

$publishPath = "./Release/*"
$deployPath = "/opt/StopwatchExample"
$deployUser = $secrets.username + "@" + $secrets.hostname
$deployTarget =  $deployUser + ":" + $deployPath

if ($secrets.keyPath -eq "") {
    "No key detected, authenticating via Password"
    scp -r $publishPath $deployTarget
} else {
    "Key detected, authenticating with SSH Key"
    scp -i $secrets.keyPath -r $publishPath $deployTarget
}