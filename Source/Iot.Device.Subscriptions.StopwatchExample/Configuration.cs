using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Iot.Device.Subscriptions.StopwatchExample
{
    /// <summary>
    /// Hard Typed Configuration
    /// See: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-5.0
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The Hex based BusId the LCD I2C Controller is bound to.
        /// Usually "0x27" for the popular PCF8574 driver
        /// Or "0x3F" for the popular PCF8574A driver
        /// See the following handy guide to figure out what Bus Id your driver has connected on
        /// https://learn.adafruit.com/adafruits-raspberry-pi-lesson-4-gpio-setup/configuring-i2c
        /// </summary>
        public string I2CBusIdHex { get; set; }

        /// <summary>
        /// The computed Base 10 representation of <see cref="I2CBusIdHex"/>
        /// </summary>
        public int I2CBusIdInt => Convert.ToInt32(I2CBusIdHex, 16);

        /// <summary>
        /// The driver of your device, see Pcx857Enum.cs for the full list
        /// </summary>
        public Pcx857xEnum I2CDriver { get; set; }

        /// <summary>
        /// Size of the LCD display. See LcdEnum.cs for the list of supported sizes
        /// </summary>
        public LcdEnum LcdSize { get; set; }

        /// <summary>
        /// How often to check for clock events and update the display
        /// </summary>
        public TimeSpan ClockRate => TimeSpan.FromMilliseconds(ClockRateMs);

        /// <summary>
        /// How often to check for clock events and update the display in milliseconds
        /// </summary>
        public long ClockRateMs { get; set; }

        /// <summary>
        /// What PinMode to listen to events with.
        /// </summary>
        public PinMode BtnPinMode { get; set; }

        /// <summary>
        /// Sensitivity to delay button events after detection, in ticks
        /// </summary>
        public long BtnSensitivityTicks => TimeSpan.FromMilliseconds(BtnSensitivityMs).Ticks;

        /// <summary>
        /// Sensitivity to delay button events after detection, in milliseconds
        /// </summary>
        public long BtnSensitivityMs { get; set; }

        /// <summary>
        /// Whether a Reset Button is used
        /// </summary>
        public bool ResetBtnEnabled => ResetBtnPin > -1;

        /// <summary>
        /// The GPIO pin the Reset Button is connected to
        /// </summary>
        public int ResetBtnPin { get; set; }

        /// <summary>
        /// Whether a Pause Button is used
        /// </summary>
        public bool PauseBtnEnabled => PauseBtnPin > -1;

        /// <summary>
        /// The GPIO pin the Pause Button is connected to
        /// </summary>
        public int PauseBtnPin { get; set; }

        /// <summary>
        /// Whether a Stop Button is used
        /// </summary>
        public bool StopBtnEnabled => StopBtnPin > -1;

        /// <summary>
        /// The GPIO pin the Stop Button is connected to
        /// </summary>
        public int StopBtnPin { get; set; }
    }
}
