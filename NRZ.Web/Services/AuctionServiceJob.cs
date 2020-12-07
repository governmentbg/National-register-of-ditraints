using Hangfire;
using Microsoft.AspNetCore.SignalR;
using NRZ.Services.Interfaces;
using NRZ.Web.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NRZ.Web.Services
{
    public class AuctionServiceJob : IHangfireJob
    {
        private readonly IAuctionService _aucService;
        private readonly IHubContext<AuctionHub> _aucHub;
        public AuctionServiceJob(IAuctionService aucService, IHubContext<AuctionHub> hubContext)
        {
            _aucService = aucService;
            _aucHub = hubContext;    
        }

        public async Task Run(IJobCancellationToken token)
        {
            token.ThrowIfCancellationRequested();
            await RunAtTimeOf(DateTime.UtcNow);
        }

        public async Task RunAtTimeOf(DateTime now)
        {
            //Do some work
            var aucResults = await _aucService.ProcessFinishedAuctionsAsync();
            if(aucResults.Count > 0)
            {
                foreach (var res in aucResults)
                {
                    await _aucHub.Clients.Group($"Auction {res.AuctionId}").SendAsync("AuctionEnd", res);
                }
            }
        }
    }
}
