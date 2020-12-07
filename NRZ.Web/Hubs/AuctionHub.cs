using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NRZ.Models.Auction;
using NRZ.Services.Interfaces;
using NRZ.Web.Extensions;
using System;
using System.Threading.Tasks;

namespace NRZ.Web.Hubs
{
    [Authorize]
    public class AuctionHub : Hub
    {
        private readonly IAuctionService _auctionService;
        public AuctionHub(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
         
        public override async Task OnDisconnectedAsync(Exception exception)
        {
            await base.OnDisconnectedAsync(exception);
        }

        public async Task RemoveFromGroup(int auctionId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Auction {auctionId}");
        }

        public async Task AddToGroup(int auctionId)
        {
            if (await _auctionService.CanConnect(auctionId, Context.User.GetUserId()))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Auction {auctionId}");
            }
            else
            {
                await SendPrivateMessage(Context.UserIdentifier, "InvalidAuction", $"Unable to connect to auvtion ID = {auctionId}");
            }
        }

        public async Task Bid(int auctionId, decimal bid, DateTime clientTime)
        {
            try
            {
                BidModel model = new BidModel()
                {
                    AuctionId = auctionId,
                    Bid = bid,
                    BidderId = Context.User.GetUserId(),
                    ClientTime = clientTime.ToUniversalTime(),
                    ServerTime = DateTime.UtcNow
                };

                //model.Valid = await _auctionService.ValidateBid(model);
                //write bid to db
                (bool, DateTime?) result = await _auctionService.AddBid(model, Context.User.Identity.Name);

                if (result.Item1)
                {
                    //broadcast message to auction group with the new price
                    await SendMessageToGroup($"Auction {auctionId}", "HighestBid", bid);
                }
                else
                {
                    //send message to caller for invalid bid
                    await SendPrivateMessage(Context.UserIdentifier, "InvalidBid", "Invalid bid");
                }

                if (result.Item2.HasValue)
                {
                    await SendMessageToGroup($"Auction {auctionId}", "TimeExtend", result.Item2.Value);
                }
            }
            catch (Exception x)
            {
                await SendPrivateMessage(Context.UserIdentifier, "InvalidBid", x.Message);
            }
        }

        public Task SendMessageToAll(string user, string message)
        {
            return Clients.All.SendAsync("Message", user, message);
        }

        public Task SendMessageToCaller(string method, string message)
        {
            return Clients.Caller.SendAsync(method, message);
        }

        public Task SendMessageToGroup(string group, string method, object message)
        {
            return Clients.Group(group).SendAsync(method, message);
        }

        public Task SendPrivateMessage(string user, string method, string message)
        {
            return Clients.User(user).SendAsync(method, message);
        }
    }
}
