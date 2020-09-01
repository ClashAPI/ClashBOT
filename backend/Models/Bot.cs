using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace backend.Models
{
    public class Bot
    {
        // public DiscordShardedClient Client { get; set; }
        public DiscordClient Client { get; set; }
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
            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefix = configJson.Prefix,
                EnableMentionPrefix = true,
                EnableDms = false,
                EnableDefaultHelp = true
            };

            // Client.UseCommandsNext(commandsConfig);
            Commands = Client.UseCommandsNext(commandsConfig);

            // await Client.StartAsync();
            await Client.ConnectAsync();
            // await Task.Delay(-1);
        }

        public Task OnClientReady(ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}