# AutoGoLivePoster-BlueSky
This is an application created in C# in Jetbrains Rider, to work on Linux Mint 21.2. This will automatically post on BlueSky when going live via streamerbot intergration. 


### Installation
1. example-settings.json, fill in your information and save file excluding "example-"
``` json
    {
    "Handle": "USERNAME.bsky.social",
    "AppPassword": "xxxx-xxxx-xxxx-xxxx"
    }   

```
2. Ensure strings are changed accordingly:
``` C#
    string platform = "twitch";
    string streamLink = "https://twitch.tv/kaydeecodes";
    string tweetText = $"🔴 I'm LIVE now on {platform.ToUpper()}! Come hang out 👉 {streamLink} 🎮 {DateTime.Now:T} <3 ";
```

## License
This project uses the MIT License. See the `LICENSE.txt` file for details.
Feel free to customize or extend this as needed for your project! Let me know if you need further assistance.


