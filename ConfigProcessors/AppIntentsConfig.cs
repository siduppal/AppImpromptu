using Newtonsoft.Json;

namespace AppDefnParser.ConfigProcessors
{
    internal class AppIntentsConfig
    {
        private AppCommandIntent[]? AppIntents { get; set; }

        internal AppCommandIntent? GetAppIntent(string appName)
        {
            return AppIntents?.FirstOrDefault(x => x.AppName == appName);
        }

        internal static AppIntentsConfig? Load(string filePath)
        {
            var json = File.ReadAllText(filePath);

            var root = JsonConvert.DeserializeObject<AppIntentsConfig>(json);

            return root;
        }
        
        private AppIntentsConfig()
        {
        }

    }

    internal class AppCommandIntent
    {
        public string? AppName { get; set; }
        public CommandIntent[]? CommandIntents { get; set; }
    }

    internal class CommandIntent
    {
        public string? Command { get; set; }
        public string? Intent { get; set; }
    }

}
