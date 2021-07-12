﻿using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.CharacterLcd;
using Iot.Device.Pcx857x;
using Iot.Device.Subscriptions.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Iot.Device.Subscriptions.StopwatchExample
{
    /// <summary>
    /// To test this program out, you will need the following:
    /// 1x Controller Board (RPI, Arduino, etc)
    /// 1~3x Buttons
    /// 1x LED Character Panel with standard I2C backpack
    /// 
    /// Configuring which buttons are enabled, which pins to listen on, and which type of LCD controller to use
    /// Can all be configured via AppSettings.json, as well as the built in support for Environment Variables,
    /// Command line Args, and Project Secrets
    ///
    /// See AppSettings.json and Configuration.cs for more details on each config value
    /// </summary>

    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Program Started");

            var (config, lcd, board) = Boilerplate(args);
            lcd.DisplayOn = true;
            lcd.Clear();

            var subscriptionService = BuildSubscriptions(config);

            var clock = 0L;
            var paused = false;
            await foreach (var subEvent in subscriptionService.Run(board, CancellationToken.None))
            {
                if (subEvent.IsClock && !paused)
                {
                    clock += subEvent.Delta;
                }

                if (subEvent.PinNumber == config.ResetBtnPin)
                {
                    clock = 0L;
                }

                if (subEvent.PinNumber == config.PauseBtnPin)
                {
                    paused = !paused;
                }

                if (subEvent.PinNumber == config.StopBtnPin)
                {
                    break;
                }

                lcd.SetCursorPosition(0, 0);
                lcd.Write($"{TimeSpan.FromTicks(clock):g}");
            }
            lcd.Clear();
            lcd.DisplayOn = false;
            Console.WriteLine("Program Ended");
        }


        static (Configuration config, Hd44780 lcd, GpioController board) Boilerplate(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddEnvironmentVariables()
                .AddJsonFile("AppSettings.json", true)
                .AddUserSecrets<Program>(true)
                .AddCommandLine(args)
                .Build()
                .Get<Configuration>();

            using var i2c = I2cDevice.Create(new I2cConnectionSettings(1, config.I2CBusIdInt));
            using Pcx857x.Pcx857x driver = config.I2CDriver switch
            {
                Pcx857xEnum.Pca8574 => new Pca8574(i2c),
                Pcx857xEnum.Pca8575 => new Pca8575(i2c),
                Pcx857xEnum.Pcf8574 => new Pcf8574(i2c),
                Pcx857xEnum.Pcf8575 => new Pcf8575(i2c),
                _ => throw new ArgumentOutOfRangeException()
            };
            using var lcdController = new GpioController(PinNumberingScheme.Logical, driver);

            using Hd44780 lcd = config.LcdSize switch
            {
                LcdEnum.Lcd16x2 => new Lcd1602(0, 2, new[] { 4, 5, 6, 7 }, 3, 0.1f, 1, lcdController),
                LcdEnum.Lcd20x4 => new Lcd2004(0, 2, new[] { 4, 5, 6, 7 }, 3, 0.1f, 1, lcdController),
                _ => throw new ArgumentOutOfRangeException()
            };

            using var boardController = new GpioController();

            return (config, lcd, boardController);
        }

        static ISubscriptionService BuildSubscriptions(Configuration config)
        {
            var collection = new SubscriptionCollection
            {
                ClockEnabled = true,
                ClockRate = config.ClockRate
            };
            if (config.ResetBtnEnabled)
            {
                collection.Subscribe(config.ResetBtnPin, config.BtnPinMode, PinEventTypes.Rising);
            }
            if (config.PauseBtnEnabled)
            {
                collection.Subscribe(config.PauseBtnPin, config.BtnPinMode, PinEventTypes.Rising);
            }
            if (config.StopBtnEnabled)
            {
                collection.Subscribe(config.StopBtnPin, config.BtnPinMode, PinEventTypes.Rising);
            }

            return collection.Build();
        }
    }
}