using System.Diagnostics;
using Microsoft.AspNetCore.SignalR;
using RFIDApi.Hubs;

namespace RFIDApi.Service
{
    public class RfidSignalRDispatcher : BackgroundService
    {
        private readonly IHubContext<RFIDHubs> _hubContext;

        public RfidSignalRDispatcher(IHubContext<RFIDHubs> hubContext)
        {
            _hubContext = hubContext;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Debug.WriteLine("Dispatcher started");
            await foreach (var tag in RfidSignalRQueue.SignalChannel.Reader.ReadAllAsync(stoppingToken))
            {
                await _hubContext.Clients.All.SendAsync("ReceiveRFIDData", tag, stoppingToken);
                await Task.Delay(100, stoppingToken);
            }
        }
    }
}
