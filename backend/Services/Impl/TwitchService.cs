using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using backend.Models;
using Microsoft.Extensions.DependencyInjection;
using TwitchLib.Api;
using TwitchLib.Api.Services;
using TwitchLib.Api.Services.Events;
using TwitchLib.Api.Services.Events.LiveStreamMonitor;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;
using User = TwitchLib.Api.Helix.Models.Users.User;

namespace backend.Services
{
    public class TwitchService : ITwitchService
    {
        private LiveStreamMonitorService _monitor;
        private TwitchAPI _api;
        private readonly IServiceProvider _serviceProvider;

        public TwitchService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            Task.Run(() => ConfigLiveMonitorAsync());
        }

        public void Init()
        {
            Console.WriteLine("Init...");
        }
        
        private async Task ConfigLiveMonitorAsync()
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var guildService = scope.ServiceProvider.GetRequiredService<IGuildService>();
                
                _api = new TwitchAPI();

                _api.Settings.ClientId = "d0qgyohqojd71mpvsz7p0ldre5lfic";
                _api.Settings.AccessToken = "ciif9s2t2hhrn8tcx92e9p6g8xxpbq";

                _monitor = new LiveStreamMonitorService(_api, 60);
                
                var guilds = await guildService.GetAllAsync();
                var items = guilds.Select(g => g.TwitchPlugin.TwitchChannelSubscriptions).ToList();
                var channelsToMonitor = new List<string>();

                foreach (var subscription in items.SelectMany(item => item.Where(subscription => !channelsToMonitor.Contains(subscription.StreamerId))))
                {
                    channelsToMonitor.Add(subscription.StreamerId);
                }

                _monitor.SetChannelsById(channelsToMonitor);

                _monitor.OnStreamOnline += Monitor_OnStreamOnline;
                _monitor.OnStreamOffline += Monitor_OnStreamOffline;
                _monitor.OnStreamUpdate += Monitor_OnStreamUpdate;

                _monitor.OnServiceStarted += Monitor_OnServiceStarted;
                _monitor.OnChannelsSet += Monitor_OnChannelsSet;

                _monitor.Start();

                await Task.Delay(-1);
            }
        }

        private void Monitor_OnStreamOnline(object sender, OnStreamOnlineArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var botService = scope.ServiceProvider.GetRequiredService<IBotService>();
                botService.SendTwitchNotification(e.Channel, TwitchNotificationType.StreamUp);
                
                Console.WriteLine($"[{e.Stream.StartedAt}] Channel {e.Channel} is now live");
            }
        }

        private void Monitor_OnStreamUpdate(object sender, OnStreamUpdateArgs e)
        {
            // throw new NotImplementedException();
        }

        private void Monitor_OnStreamOffline(object sender, OnStreamOfflineArgs e)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var botService = scope.ServiceProvider.GetRequiredService<IBotService>();
                botService.SendTwitchNotification(e.Channel, TwitchNotificationType.StreamUp);

                Console.WriteLine($"[{DateTime.Now}] Channel {e.Channel} went offline.");
            }
        }

        private void Monitor_OnChannelsSet(object sender, OnChannelsSetArgs e)       
        {
            // throw new NotImplementedException();
        }

        private void Monitor_OnServiceStarted(object sender, OnServiceStartedArgs e)
        {
            Console.WriteLine("Twitch service has been injected and listening for events.");
            Console.WriteLine(_api.Helix.Users.GetUsersAsync(logins: new List<string> {"dyannatv"}).Result.Users.FirstOrDefault().Id);
            // throw new NotImplementedException();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            var response = await _api.Helix.Users.GetUsersAsync(new List<string> {id});
            return response.Users.FirstOrDefault();
        }
    }
}