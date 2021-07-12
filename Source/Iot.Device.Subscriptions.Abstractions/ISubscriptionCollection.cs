using System;
using System.Collections.Generic;
using System.Device.Gpio;

namespace Iot.Device.Subscriptions.Abstractions
{
    public interface ISubscriptionCollection
    {
        /// <summary>
        /// Enable/Disable clock event subscription
        /// </summary>
        public bool ClockEnabled { get; set; }

        /// <summary>
        /// How often, at minimum, to fire clock events, if enabled.
        /// Note this is the minimum time, use the Delta value on clock events
        /// to check how long as actually transpired since the prior clock event
        /// </summary>
        TimeSpan ClockRate { get; set; }

        /// <summary>
        /// Immutable list of existing Subscriptions. Use <see cref="Subscribe"/> and <see cref="Unsubscribe"/> to modify
        /// </summary>
        IEnumerable<ISubscription> Subscriptions { get; }

        /// <summary>
        /// Subscribes to a pin input event
        /// </summary>
        /// <param name="pinNumber">Pin Number to listen on</param>
        /// <param name="pinMode">Pin Mode to listen with, all subscriptions to the same pin must use the same mode</param>
        /// <param name="eventType">Type of Event to listen for</param>
        ISubscriptionCollection Subscribe(int pinNumber, PinMode pinMode, PinEventTypes eventType);

        /// <summary>
        /// Unsubscribes to a pin input event
        /// </summary>
        /// <param name="pinNumber">Pin Number to unsubscribe from</param>
        /// <param name="eventType">Type of Event to unsubscribe</param>
        ISubscriptionCollection Unsubscribe(int pinNumber, PinEventTypes eventType);

        /// <summary>
        /// Compiles the <see cref="ISubscriptionCollection"/> into an immutable <see cref="ISubscriptionService"/> which can be run
        /// </summary>
        ISubscriptionService Build();
    }
}