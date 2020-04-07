using Microsoft.Extensions.Hosting;
using MVC_App.Siccar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVC_App.Cache
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
            while(true)
            {
                try
                {
                    _cache.UpdateStatusInCacheForEveryUser();
                }
                catch(Exception e)
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
