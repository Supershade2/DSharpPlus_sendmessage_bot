using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DSharpPlus;
using System.Text.RegularExpressions;
using DSharpPlus.Entities;
using System.Globalization;
using Microsoft.Security.Application;
using Newtonsoft.Json;
namespace MCStreamBot
{
    public class Program
    {
        static MessageData message;
        static DiscordClient discord;
        static DiscordChannel discordChannel;
        static void Main(string[] args)
        {
            switch (args.Length < 3)
            {
                case false:
                    args[2] = Sanitizer.GetSafeHtmlFragment(args[2]);
                    /*for (int i = 0; i < args.Length; i++)
                    {
                        args[i] = Regex.Escape(args[i]);
                        char[] arg_string = args[i].ToCharArray();
                        string temp = "";
                        for (int i1 = 0; i1 < arg_string.Length; i1++)
                        {
                            if (arg_string[i1] == 92)
                            {

                            }
                            else
                            {
                                temp += arg_string[i1];
                            }
                        }
                        args[i] = temp;
                    }*/
                    break;

                default:
                    Console.Write("please supply a token, channel number, and message to issue to discord server");
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
            string json;
            TimeSpan span = TimeSpan.Zero;
            DiscordMessage discordMessage;
            discord = new DiscordClient(new DiscordConfiguration
            {
                Token = args[0],
                TokenType = TokenType.Bot
            });
            //Discord channel id gets plugged into here
            discordChannel = discord.GetChannelAsync(Convert.ToUInt64(args[1])).Result;
            await discord.ConnectAsync();
            if (System.IO.File.Exists(args[2].Split(' ')[0] + "lastlive.json")) 
            {
                json = System.IO.File.ReadAllText(args[2].Split(' ')[0] + "lastlive.json");
                message = JsonConvert.DeserializeObject<MessageData>(json);
                span = DateTimeOffset.Now - message.timeOffset;
            }
            switch (span == TimeSpan.Zero)
            {
                case true:
                    discordMessage = await discord.SendMessageAsync(discordChannel, args[2], false, null);
                    message.timeOffset = discordMessage.CreationTimestamp;
                    message.message = args[2];
                    json = JsonConvert.SerializeObject(message);
                    System.IO.File.WriteAllText(args[2].Split(' ')[0] + "lastlive.json", json);
                    break;
                default:
                    if(span.TotalSeconds < 30)
                    { 
                        
                    }
                    else 
                    {
                        discordMessage = await discord.SendMessageAsync(discordChannel, args[2], false, null);
                        message.timeOffset = discordMessage.CreationTimestamp;
                        message.message = args[2];
                        json = JsonConvert.SerializeObject(message);
                        System.IO.File.WriteAllText(args[2].Split(' ')[0] + "lastlive.json", json);
                    }
                    break;
            }
            await discord.DisconnectAsync();
        }
        public struct MessageData 
        {
            public DateTimeOffset timeOffset { get; set; }
            public string message { get; set; }
        }
    }
}