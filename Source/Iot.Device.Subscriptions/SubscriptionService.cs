using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Iot.Device.Subscriptions.Abstractions;

namespace Iot.Device.Subscriptions
{
    /// <inheritdoc />
    public class SubscriptionService : ISubscriptionService
    {
        private IEnumerable<Subscription> Subscriptions { get; }
        private TimeSpan ClockRate { get; }

        private bool ClockEnabled { get; }

        /// <summary>
        /// Manually built Subscription Service. Use <see cref="SubscriptionCollection"/> for the fluent interface
        /// To build a Subscription Service in an easy way.
        /// </summary>
        /// <param name="subscriptions">List of subscriptions to use</param>
        /// <param name="clockRate">
        /// Frequency to check clock events. Note that this is the minimum delay for clock events.
        /// Please make sure to use the <see cref="SubscriptionEvent.Delta"/> value to get the true delay since the last clock event
        /// </param>
        /// <param name="clockEnabled">Whether listening for the clock event is enabled</param>
        public SubscriptionService(IEnumerable<Subscription> subscriptions, TimeSpan clockRate, bool clockEnabled)
        {
            Subscriptions = subscriptions ?? throw new ArgumentNullException(nameof(subscriptions));
            ClockRate = clockRate;
            ClockEnabled = clockEnabled;
        }

        /// <inheritdoc />
        public async IAsyncEnumerable<ISubscriptionEvent> Run(GpioController controller, [EnumeratorCancellation] CancellationToken cancellationToken)
        {
            foreach (var subscription in Subscriptions.GroupBy(s => s.PinNumber))
            {
                controller.OpenPin(subscription.Key, subscription.First().PinMode);
            }

            var timestamp = DateTime.Now.Ticks;
            var waitPool = BuildWaitPool(controller, cancellationToken);

            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now.Ticks;
                var delta = now - timestamp;

                var resultTask = await Task.WhenAny(waitPool.Keys);
                var (pinNumber, builder) = waitPool[resultTask];
                waitPool.Remove(resultTask);
                waitPool[builder()] = (pinNumber, builder);

                if (pinNumber >= 0)
                {
                    delta = 0L;
                }
                else
                {
                    timestamp = now;
                }

                yield return new SubscriptionEvent(pinNumber, await resultTask, delta);
            }
        }

        /// <summary>
        /// Composes the pool of awaitable events that have been subscribed to, including the clock event
        /// </summary>
        private IDictionary<Task<WaitForEventResult>, (int PinNumber, Func<Task<WaitForEventResult>> Builder)> BuildWaitPool(GpioController controller, CancellationToken cancellationToken)
        {
            var result = new Dictionary<Task<WaitForEventResult>, (int, Func<Task<WaitForEventResult>>)>();
            foreach (var subscription in Subscriptions)
            {
                Task<WaitForEventResult> Builder() => controller.WaitForEventAsync(subscription.PinNumber, subscription.EventType, cancellationToken).AsTask();
                result.Add(Builder(), (subscription.PinNumber, Builder));
            }

            if (!ClockEnabled)
                return result;

            Task<WaitForEventResult> ClockTaskBuilder() => ClockTask(cancellationToken);
            result.Add(ClockTaskBuilder(), (-1, ClockTaskBuilder));

            return result;
        }

        private async Task<WaitForEventResult> ClockTask(CancellationToken cancellationToken)
        {
            await Task.Delay(TimeSpan.FromTicks(ClockRate.Ticks), cancellationToken);

            return new WaitForEventResult { EventTypes = PinEventTypes.None, TimedOut = true };
        }
    }
}