$projectPath = "./Iot.Device.Subscriptions.StopwatchExample.csproj"
$publishPath = "./Deploy"

dotnet publish $projectPath -p:PublishProfile="linux-arm" --output $publishPath -p:DebugType=None