using Newtonsoft.Json;

namespace AppDefnParser.ConfigProcessors
{

    internal class IntentPhrasesAssist
    {
        public IntentPhrases[]? IntentPhrases { get; set; }

        public static IntentPhrasesAssist Load(string filePath)
        {
            return JsonConvert.DeserializeObject<IntentPhrasesAssist>(System.IO.File.ReadAllText(filePath)) ?? new IntentPhrasesAssist();
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
