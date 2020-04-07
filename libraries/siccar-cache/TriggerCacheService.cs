using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Siccar.CacheManager
{
    public class TriggerCacheService : IHostedService
    {
        private ISiccarStatusCache _cache;
        public TriggerCacheService(ISiccarStatusCache cache)
        {
            _cache = cache;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            while (true)
            {
                try
                {
                    _cache.UpdateStatusInCacheForEveryUser();
                }
                catch (Exception)
                {
                    Console.WriteLine("Exception");
                }
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
