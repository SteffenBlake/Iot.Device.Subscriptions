$projectPath = "./Iot.Device.Subscriptions.StopwatchExample.csproj"
$publishPath = "./Release"

dotnet publish $projectPath -p:PublishProfile="linux-arm" --output $publishPath -p:DebugType=None