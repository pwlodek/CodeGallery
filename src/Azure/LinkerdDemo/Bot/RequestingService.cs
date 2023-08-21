using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Bot
{
    public class RequestingService : IHostedService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<RequestingService> _logger;

        public RequestingService(IHttpClientFactory httpClientFactory, ILogger<RequestingService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var client = _httpClientFactory.CreateClient();
                try
                {
                    var response = await client.GetAsync($"http://gateway-svc/api/values/1");
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning($"{DateTime.Now}: values/1 failed");
                    }
                    else
                    {
                        _logger.LogInformation($"{DateTime.Now}: values/1 succeeded");
                    }
                    await Task.Delay(1000);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error while getting values/1");
                }
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
    }
}
