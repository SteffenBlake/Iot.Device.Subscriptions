using System.Device.Gpio;

namespace Iot.Device.Subscriptions.Abstractions
{
    public interface ISubscription
    {
        int PinNumber { get; }
        PinMode PinMode { get; }
        PinEventTypes EventType { get; }
    }
}