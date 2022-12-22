using Newtonsoft.Json;

namespace AppDefnParser.ConfigProcessors
{
    internal class AppIntentsConfig
    {
        public AppCommandIntent[]? AppIntents { get; set; }

        internal string? GetAppCommandIntent(string appName, string command)
        {
            return AppIntents?
                .FirstOrDefault(x => x?.AppName == appName)?
                .CommandIntents?
                .FirstOrDefault(x => x?.Command == command)?
                .Intent;
        }

        internal static AppIntentsConfig Load(string filePath)
        {
            var json = File.ReadAllText(filePath);

            var root = JsonConvert.DeserializeObject<AppIntentsConfig>(json);

            return root ?? new AppIntentsConfig();
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
