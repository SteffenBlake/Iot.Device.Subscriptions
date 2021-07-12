using System.Device.Gpio;
using Iot.Device.Subscriptions.Abstractions;

namespace Iot.Device.Subscriptions
{
    public class SubscriptionEvent : ISubscriptionEvent
    {
        public int PinNumber { get; }
        public long Delta { get; }
        public WaitForEventResult Result { get; }
        
        public bool IsClock => PinNumber == -1;

        public SubscriptionEvent(int pinNumber, WaitForEventResult result, long delta)
        {
            PinNumber = pinNumber;
            Result = result;
            Delta = delta;
        }
    }
}