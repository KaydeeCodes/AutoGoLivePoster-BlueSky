using System;
using System.Threading.Tasks;

namespace BlueskyLivePoster
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string platform = "twitch";
            string link = "https://twitch.tv/kaydeecodes";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "--platform" && i + 1 < args.Length)
                    platform = args[i + 1];
                if (args[i] == "--link" && i + 1 < args.Length)
                    link = args[i + 1];
            }

            var settings = Settings.Load();
            var handle = settings.Handle!;
            var appPassword = settings.AppPassword!;


            var client = new BlueskyClient(handle, appPassword);

            Console.WriteLine("🔐 Logging in...");
            if (!await client.LoginAsync())
                return;

            var message = $"🔴 I'm LIVE on {platform.ToUpper()}! Come hang out 👉 {link} 🕒 {DateTime.Now:HH:mm:ss}";

            Console.WriteLine("📤 Posting to Bluesky...");
            if (await client.PostAsync(message))
            {
                Console.WriteLine("✅ Post successful!");
            }
            else
            {
                Console.WriteLine("❌ Failed to post.");
            }
        }
    }
}