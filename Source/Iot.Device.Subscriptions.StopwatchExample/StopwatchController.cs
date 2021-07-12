using System;
using System.Device.Gpio;
using System.Device.I2c;
using Iot.Device.CharacterLcd;

namespace Iot.Device.Subscriptions.StopwatchExample
{
    public class StopwatchController : IDisposable
    {
        public StopwatchController(I2cDevice i2C, Pcx857x.Pcx857x driver, GpioController lcdController, Hd44780 lcd, GpioController board)
        {
            I2C = i2C ?? throw new ArgumentNullException(nameof(i2C));
            Driver = driver ?? throw new ArgumentNullException(nameof(driver));
            LcdController = lcdController ?? throw new ArgumentNullException(nameof(lcdController));
            Lcd = lcd ?? throw new ArgumentNullException(nameof(lcd));
            Board = board ?? throw new ArgumentNullException(nameof(board));
        }

        private I2cDevice I2C { get; }
        private Pcx857x.Pcx857x Driver { get; }
        private GpioController LcdController { get; }
        public Hd44780 Lcd { get; }
        public GpioController Board { get; }

        public void Dispose()
        {
            I2C?.Dispose();
            Driver?.Dispose();
            LcdController?.Dispose();
            Lcd?.Dispose();
            Board?.Dispose();
        }
    }
}
