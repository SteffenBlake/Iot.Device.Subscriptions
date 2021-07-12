if [ "$(id -u)" != 0 ]; then
    echo "Install.sh requires being run as root"
    echo "Stopping..."
    exit 1
fi

if [ ! -x "$(command -v wget)" ]; then
    echo "Please install 'wget' for this script to work"
fi
if [ ! -x "$(command -v tar)" ]; then
    echo "Please install 'tar' for this script to work"
fi

echo "Creating temp folder and install directory"
mkdir /temp/
mkdir /opt/StopwatchExample

echo "Downloading Example from https://github.com/SteffenBlake/Iot.Device.Subscriptions/tarball/main"
wget -qO- --show-progress https://github.com/SteffenBlake/Iot.Device.Subscriptions/tarball/main | tar xz -C /temp
mv /temp/*/Source/Iot.Device.Subscriptions.StopwatchExample/Deploy/* /opt/StopwatchExample
rm -R /temp

chmod 755 /opt/StopwatchExample/Iot.Device.Subscriptions.StopwatchExample
echo "Install complete, execute '/opt/StopwatchExample/Iot.Device.Subscriptions.StopwatchExample' to run"