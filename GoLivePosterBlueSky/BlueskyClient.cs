using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlueskyLivePoster
{
    public class BlueskyClient
    {
        private readonly string _handle;         // Your full Bluesky handle (e.g. kaydeecodes.bsky.social)
        private readonly string _appPassword;    // App password generated from Bluesky
        private string? _accessJwt;

        public BlueskyClient(string handle, string appPassword)
        {
            _handle = handle;
            _appPassword = appPassword;
        }

        public async Task<bool> LoginAsync()
        {
            using var client = new HttpClient();

            var body = new
            {
                identifier = _handle,
                password = _appPassword
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://bsky.social/xrpc/com.atproto.server.createSession", content);
            var result = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"❌ Login failed: {result}");
                return false;
            }

            var doc = JsonDocument.Parse(result);
            _accessJwt = doc.RootElement.GetProperty("accessJwt").GetString();
            Console.WriteLine("✅ Logged in to Bluesky!");
            return true;
        }

        public async Task<bool> PostAsync(string text)
        {
            if (_accessJwt == null)
            {
                Console.WriteLine("❌ Not authenticated.");
                return false;
            }

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _accessJwt);

            var record = new Dictionary<string, object>
            {
                ["$type"] = "app.bsky.feed.post",
                ["createdAt"] = DateTime.UtcNow.ToString("o"),
                ["text"] = text
            };

            var body = new Dictionary<string, object>
            {
                ["repo"] = _handle,
                ["collection"] = "app.bsky.feed.post",
                ["record"] = record
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://bsky.social/xrpc/com.atproto.repo.createRecord", content);
            var result = await response.Content.ReadAsStringAsync();

            Console.WriteLine("📥 Bluesky API response:");
            Console.WriteLine(result);

            return response.IsSuccessStatusCode;
        }
    }
}
