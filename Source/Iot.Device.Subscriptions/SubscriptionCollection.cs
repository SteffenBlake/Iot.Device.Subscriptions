using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using Iot.Device.Subscriptions.Abstractions;

namespace Iot.Device.Subscriptions
{
    /// <inheritdoc />
    public class SubscriptionCollection : ISubscriptionCollection
    {
        /// <inheritdoc />
        public bool ClockEnabled { get; set; }

        /// <inheritdoc />
        public TimeSpan ClockRate { get; set; } = TimeSpan.FromMilliseconds(500);

        /// <inheritdoc />
        public IEnumerable<ISubscription> Subscriptions => _subscriptions.AsReadOnly();

        // Private Mutable list of subscriptions
        private List<Subscription> _subscriptions { get; } = new List<Subscription>();

        /// <inheritdoc />
        public ISubscriptionCollection Subscribe(int pinNumber, PinMode pinMode, PinEventTypes eventType)
        {
            if (pinMode == PinMode.Output)
            {
                throw new ArgumentOutOfRangeException(nameof(pinMode), "PinMode.Output is not supported for subscriptions.");
            }

            if (Subscriptions.Any(s => s.PinNumber == pinNumber && s.EventType == eventType))
            {
                throw new InvalidOperationException($"Subscription already exists for pin {pinNumber}:{Enum.GetName(eventType)}");
            }

            if (Subscriptions.Any(s => s.PinNumber == pinNumber && s.PinMode != pinMode))
            {
                throw new InvalidOperationException($"Conflicting PinModes for pin {pinNumber}, all pin subscriptions must use the same PinMode.");
            }

            _subscriptions.Add(new Subscription(pinNumber, pinMode, eventType));

            return this;
        }

        /// <inheritdoc />
        public ISubscriptionCollection Unsubscribe(int pinNumber, PinEventTypes eventType)
        {
            var match = _subscriptions.FirstOrDefault(s => s.PinNumber == pinNumber && s.EventType == eventType);

            if (match == null)
            {
                throw new InvalidOperationException($"No subscription exists for pin {pinNumber}:{Enum.GetName(eventType)}");
            }

            _subscriptions.Remove(match);

            return this;
        }

        /// <inheritdoc />
        public ISubscriptionService Build()
        {
            return new SubscriptionService(_subscriptions, ClockRate, ClockEnabled);
        }
    }
}
