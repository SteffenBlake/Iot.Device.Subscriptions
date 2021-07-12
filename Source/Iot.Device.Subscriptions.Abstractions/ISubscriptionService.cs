using System.Collections.Generic;
using System.Device.Gpio;
using System.Threading;

namespace Iot.Device.Subscriptions.Abstractions
{
    public interface ISubscriptionService
    {
        /// <summary>
        /// Runs the service, listening for subscription events. Will return both Input events and the Clock event
        /// </summary>
        /// <param name="controller">Controller to use for listening for events</param>
        /// <param name="cancellationToken"></param>
        IAsyncEnumerable<ISubscriptionEvent> Run(GpioController controller, CancellationToken cancellationToken);
    }
}