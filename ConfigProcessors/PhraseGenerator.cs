using Newtonsoft.Json;
using System.Globalization;

namespace AppDefnParser.ConfigProcessors
{

    internal class IntentPhrasesAssist
    {
        private IntentPhrases[]? IntentPhrases { get; set; }

        public static IntentPhrasesAssist? Load(string filePath)
        {
            var json = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<IntentPhrasesAssist>(json);
        }

        public string[]? GetPhrases(string intentName)
        {
            return this.IntentPhrases?.FirstOrDefault(x => x.Intent == intentName)?.ExamplePhrases;                    
        }
    }

    internal class IntentPhrases
    {
        public string? Intent { get; set; }
        public string[]? ExamplePhrases { get; set; }
    }

}
