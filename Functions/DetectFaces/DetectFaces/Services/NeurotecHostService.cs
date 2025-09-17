using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Neurotec.Biometrics.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DetectFaces.Services
{
    internal class NeurotecHostService(NeurotecService neurotecService) : IHostedService
    {
        public Task StartAsync(CancellationToken cancellationToken)
        {
            NeurotecService.Initialize();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            neurotecService.Dispose();
            return Task.CompletedTask;
        }
    }
}
