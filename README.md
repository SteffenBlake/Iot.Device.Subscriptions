# Iot.Device.Subscriptions



# How to Use

WIP

# How to use the Example project

## Configuring the Example Project

All configuration for the example project is performed via its AppSettings.json file, regardless of which route you use below to test it.

By default the configuration is as such:

1. The I2CBusId is presumed to be running on 0x27
2. The driver is presumed to be a `Pcf8574`
3. The Lcd is presumed to be a 20x4 screen
4. The Clock Rate for checking updates is set to 500ms
5. Button mode is set to Pull Up
6. The Reset button is enabled and on Pin 29, which is GPIO-5 for a Raspberry Pi
7. The Pause button is enabled and on Pin 33, which is GPIO-13 for a Raspberry Pi
8. The Stop button is enabled and on Pin 40, which is GPIO-21 for a Raspberry Pi

You may modify any of these values, see Configuration.cs for more details on modifying these values.

## Setting up your board

Assuming all three buttons enabled (Though you can also simulate buttons with 2 wires on a breadboard if needed), you will need the following:

- Up to 3 buttons (disable buttons via the configuration above if you dont have three)
- An LCD screen powered by an I2C backpack of the Pc\*857\* driver variety, either a 16x2 or 20x4 size (these are the most common LCD screens you can get)

Connect the following:
- Reset button between pins 29 and 30, GPIO-5 <> Ground
- Pause button between pins 33 and 40, GPIO-13 <> Ground
- Stop button between pins 39 and 40, Ground <> GPIO-21 
- LCD screen backpack to the usual Pins 3+4+5+6, SDA/SCL/5V/Ground, you can look up guides on how to do this correctly as its a common task

## Copy and Go (The easy way, not building it yourself) (Debian/Raspbian/Ubuntu/etc)

Invoke the following on your target machine as root.

`bash <(curl -s Install.sh)`

## Deploy Windows > Debian/Raspbian/Ubuntu/etc

Step 1: Checkout this github via `git clone https://github.com/SteffenBlake/Iot.Device.Subscriptions.git` on the host Windows machine

Step 2: Run `/Source/Iot.Device.Subscriptions.StopwatchExample/Publish.ps1` via Powershell

Step 3: Copy `StopwatchExample.secrets.json` to `<My Documents>/Secrets/StopwatchExample.secrets.json`

Step 4: Fill out the above new secrets file with your login credentials for your device you wish to deploy to. Leave `keyPath` as an empty string for password based authentication, otherwise, fill it in with the absolute path to the SSH key to login with (if you have SSH key auth setup)

Step 5: On the deploy target execute `sudo mkdir /opt/StopwatchExample` and `sudo chown <username> /opt/StopwatchExample`, ensure you use the same username here as what you set in the secrets.json file

Step 6: Configure `AppSettings.json` via your preferred text editor as per the above section "Configuring the Example Project", you may also do this step later after Step 8 on the target machine instead, if you prefer.

Step 7: Run `/Source/Iot.Device.Subscriptions.StopwatchExample/Deploy.ps1` via Powershell back on the Windows machine. If all goes well you should see the files copy over

Step 8: On the deploy target run `chmod 755 /opt/StopwatchExample/Iot.Device.Subscriptions.StopwatchExample`

Step 9: Finally simply invoke `/opt/StopwatchExample/Iot.Device.Subscriptions.StopwatchExample`
