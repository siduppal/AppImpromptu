using Newtonsoft.Json;

namespace AppDefnParser.ConfigProcessors
{
    public class PrioritizedAppConfig
    {
        public string[]? PrioritizedAppNames { get; set; }

        private PrioritizedAppConfig() 
        {
            
        }

        public static PrioritizedAppConfig Load(string filePath)
        {
            var root = JsonConvert.DeserializeObject<PrioritizedAppConfig>(File.ReadAllText(filePath));

            // Parse JSON file and return a PrioritizedAppConfig object
            return root ?? new PrioritizedAppConfig();
        }
    }

}
