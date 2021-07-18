$myDocumentsPath = [Environment]::GetFolderPath("MyDocuments")
$secretsPath = Join-Path -Path $myDocumentsPath -ChildPath "/Secrets/Iot.Device.Subscriptions.secrets.json"
$secrets = Get-Content $secretsPath | Out-String | ConvertFrom-Json

$version = "1.1.0"

dotnet build -c Release /p:Version=$version
dotnet pack -c Release /p:Version=$version -o "./Nuget"

Set-Location "./Nuget"

dotnet nuget push "*.nupkg" -k $secrets.ApiKey --source https://api.nuget.org/v3/index.json

Set-Location ".."