using System;
using System.Device.Gpio;
using Iot.Device.Subscriptions.Abstractions;

namespace Iot.Device.Subscriptions
{
    public class Subscription : ISubscription
    {
        public Subscription(int pinNumber, PinMode pinMode, PinEventTypes eventType)
        {
            if (pinNumber < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(pinNumber), pinNumber, "Pin number must be a positive value");
            }

            if (pinMode == PinMode.Output)
            {
                throw new ArgumentOutOfRangeException(nameof(pinMode), "PinMode.Output is not supported for subscriptions.");
            }

            PinNumber = pinNumber;
            PinMode = pinMode;
            EventType = eventType;
        }

        public int PinNumber { get; }

        public PinMode PinMode { get; }

        public PinEventTypes EventType { get; }
    }
}