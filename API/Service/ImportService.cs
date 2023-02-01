using Business.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace API.Service
{
    public class ImportService : IHostedService, IDisposable
    {
        private Timer _timer;
        private readonly IUploadViagemService _viagemService; 

        public ImportService (
            IUploadViagemService viagemService
        )
        {
            _viagemService = viagemService;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(ExecuteImport, null, TimeSpan.FromMinutes(0), TimeSpan.FromMinutes(30));
            return Task.CompletedTask;
        }
        private async void ExecuteImport(object state)
        {
            var viagens = await _viagemService.UploadViagens();
            foreach (var viagem in viagens)
            {
                var a = viagem;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite,0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
