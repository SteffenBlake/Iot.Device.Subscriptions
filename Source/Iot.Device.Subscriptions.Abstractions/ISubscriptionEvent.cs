using System.Device.Gpio;

namespace Iot.Device.Subscriptions.Abstractions
{
    public interface ISubscriptionEvent
    {
        int PinNumber { get; }
        long Delta { get; }
        WaitForEventResult Result { get; }
        bool IsClock { get; }
    }
}