using System;
using System.IO;
using System.Text.Json;

namespace BlueskyLivePoster
{
    public class Settings
    {
        public string? Handle { get; set; }
        public string? AppPassword { get; set; }

        public static Settings Load(string path = "settings.json")
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("⚠️  settings.json not found.");
                CreateTemplate(path);
                Console.WriteLine("📄 A template settings.json file has been created.");
                Console.WriteLine("✏️  Please edit it with your Bluesky handle and app password, then run the program again.");
                Environment.Exit(1);
            }

            var json = File.ReadAllText(path);
            var settings = JsonSerializer.Deserialize<Settings>(json);

            if (string.IsNullOrWhiteSpace(settings?.Handle) || string.IsNullOrWhiteSpace(settings.AppPassword))
            {
                Console.WriteLine("⚠️  settings.json is incomplete.");
                Console.WriteLine("✏️  Please make sure 'handle' and 'appPassword' are filled in.");
                Environment.Exit(1);
            }

            return settings;
        }

        private static void CreateTemplate(string path)
        {
            var template = new Settings
            {
                Handle = "yourusername.bsky.social",
                AppPassword = "xxxx-xxxx-xxxx-xxxx"
            };

            var json = JsonSerializer.Serialize(template, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(path, json);
        }
    }
}