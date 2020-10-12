using System.Collections.ObjectModel;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus.VoiceNext;

namespace backend.Models
{
    public class Bot
    {
        // public DiscordShardedClient Client { get; set; }
        public DiscordShardedClient Client { get; set; }
        public static VoiceNextClient VoiceClient;
        public CommandsNextModule Commands { get; private set; }

        public Bot()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("botconfig.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                LogLevel = LogLevel.Warning,
                UseInternalLogHandler = true
            };

            // Client = new DiscordShardedClient(config);
            Client = new DiscordShardedClient(config);

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefix = configJson.Prefix,
                EnableMentionPrefix = true,
                EnableDms = false,
                EnableDefaultHelp = true
            };

            Client.UseCommandsNext(commandsConfig);
            // Commands = Client.UseCommandsNext(commandsConfig);

            var voiceConfig = new VoiceNextConfiguration
            {
                EnableIncoming = false
            };
            
            VoiceClient = Client.UseVoiceNext(voiceConfig).Values.First();
            await Client.StartAsync();
            // await Client.ConnectAsync();
            // await Task.Delay(-1);
        }

        public Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}