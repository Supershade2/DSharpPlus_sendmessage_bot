using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using System.Text.RegularExpressions;
using DSharpPlus.Entities;
namespace MCStreamBot
{
    public class Program
    {
        static DiscordClient discord;
        static DiscordChannel discordChannel;
        static void Main(string[] args)
        {
            switch (args.Length == 0)
            {
                case false:
                    for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = Regex.Escape(args[i]);
                    }
                    break;

                default:
                    Console.Write("please supply arguments to issue to discord server"+Environment.NewLine+"Press any key to exit:");
                    Console.ReadKey();
                    Environment.Exit(Environment.ExitCode);
                    break;
            }
            try
            {
                MainDiscord(args).ConfigureAwait(false).GetAwaiter().GetResult();
                Environment.Exit(Environment.ExitCode);
            }
            catch (Exception)
            {

                throw;
            }
        }
        static async Task MainDiscord(string[] args)
        {
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = "Discord token here",
                TokenType = TokenType.Bot
            });
            //Discord channel id gets plugged into here
            discordChannel = discord.GetChannelAsync(11111111111).Result;

            await discord.ConnectAsync();
            DiscordMessage discordMessage = await discord.SendMessageAsync(discordChannel,args[0],false,null);
            await discord.DisconnectAsync();
        }
    }
}
