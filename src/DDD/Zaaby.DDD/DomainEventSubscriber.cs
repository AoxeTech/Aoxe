using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;

namespace Zaaby.DDD
{
    public class DomainEventSubscriber : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            throw new System.NotImplementedException();
        }
    }
}