using AppDefnParser.ConfigProcessors;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.ConstrainedExecution;

/*
 * [HOW TO USE]
 *  - Run this exe command to get it to parse the appDefinition.json file and emit lines of the following form
 *    <description>|@<appName> <appCommand>
 * 
 *    For example: AppDefnParser.exe > prompts.txt
 *    
 * - To take only 500 prompts from the main file, use PowerShell like so
 *      Get-Content -TotalCount 500 .\prompts.txt > 500promts.txt
 * 
 * - You can now take the prompts to try out with GPT. Only PlayGround has a lower token limit than fine-tuning.
 * - If pasting in Foundry toolkit, make sure the last line reads "I: {{prompt}}".
 */

public class Program
{
    #region State
    private static ProcessedAppDefinitionsForPrompt processedAppDefinitions;
    private static ProcessedAppDefinitionsForPrompt processedPreferredAppDefinitions;
    private static PrioritizedAppConfig prioritizedAppNames;
    #endregion

    #region Logic for processing compose-extensions
    private static void ProcessComposeExtensions(string appName, dynamic a)
    {

        if (a.inputExtensions != null)
        {
            foreach (var ie in a.inputExtensions)
            {
                if (ie.commands != null)
                {
                    foreach (var iec in ie.commands)
                    {
                        // Some app's have description missing. Use title in those cases 'coz it's always there.
                        var description = iec.description?.ToString() ?? iec.title.ToString();

                        ProcessAppDefinition(description, appName, iec.title.ToString());
                    }
                }
            }
        }

        if (a.composeExtensions != null)
        {
            foreach (var ce in a.composeExtensions)
            {
                if (ce.commands != null)
                {
                    foreach (var cec in ce.commands)
                    {
                        ProcessAppDefinition(cec.description.ToString(), appName, cec.title.ToString());
                    }
                }
            }
        }
    }
    #endregion

    #region Logic for processing bots
    private static void ProcessBots(string appName, dynamic a)
    {
        if (a.bots != null)
        {
            foreach (var b in a.bots)
            {
                if (b.commandLists != null)
                {
                    foreach (var bcl in b.commandLists)
                    {
                        if (bcl.commands != null)
                        {
                            foreach (var c in bcl.commands)
                            {
                                ProcessAppDefinition(c.description.ToString(), appName, c.title.ToString());
                            }
                        }
                    }
                }
            }
        }
    }
    #endregion

    #region Logic for processing an app-defn
    private static void ProcessAppDefinition(string description, string appName, string command)
    {
        if (description.Length >= 20) // Ignore command descriptions that are too short.
        {
            var appInfo = new AppInfoForPrompt
            {
                AppName = appName,
                Description = description,
                Command = command
            };

            if (prioritizedAppNames?.PrioritizedAppNames?.Contains(appName) ?? false)
            {
                processedPreferredAppDefinitions.Add(appName, appInfo);
            }
            else
            {
                processedAppDefinitions.Add(appName, appInfo);
            }

        }
    }
    #endregion

    #region Logic for emitting GPT prompts
    private static void EmitPromptStart()
    {
        Console.WriteLine("Convert this text to a programmatic command:");
        Console.WriteLine();
    }
    private static void EmitPromptLine(string description, string appName, string command)
    {
        Console.WriteLine($"I: {description};");
        Console.WriteLine($"O: @{appName} {command};");
    }

    #endregion

    #region Helper class(es)

    private class AppInfoForPrompt
    {
        public string AppName { get; set; }
        public string Description { get; set; }
        public string Command { get; set; }
    }

    private class ProcessedAppDefinitionsForPrompt
        : Dictionary<string, List<AppInfoForPrompt>>
    {
        public void Add(string appName, AppInfoForPrompt appInfoForPrompt)
        {
            if (!this.ContainsKey(appName))
            {
                this.Add(appName, new List<AppInfoForPrompt>());
            }

            this[appName].Add(appInfoForPrompt);
        }
    }

    #endregion

    public static void Main()
    {
        // Reset our state
        processedAppDefinitions = new ProcessedAppDefinitionsForPrompt();
        processedPreferredAppDefinitions = new ProcessedAppDefinitionsForPrompt();
        prioritizedAppNames = PrioritizedAppConfig.Load(@"Configs\PrioritizedAppNames.json");

        // Start local state
        var appDefintions = File.ReadAllText(@"Data\appDefinitions.json");
        var appDatas = JsonConvert.DeserializeObject<dynamic>(appDefintions)?.appDefinitions;

        // Process the app-manifests from catalog
        if (appDatas != null)
        {
            foreach (var a in appDatas)
            {
                var appName = a.name.Type == JTokenType.String ? a.name.ToString() : a.name.@short.ToString();

                ProcessComposeExtensions(appName, a);

                ProcessBots(appName, a);

            }
        }

        // Emit the prompts
        EmitPromptStart();

        // Emit the preferred apps first
        foreach (var kvp in processedPreferredAppDefinitions)
        {
            foreach (var i in kvp.Value)
            {
                EmitPromptLine(i.Description, i.AppName, i.Command);
            }
        }

        // Emit the rest of the apps next
        foreach (var kvp in processedAppDefinitions)
        {
            foreach (var i in kvp.Value)
            {
                EmitPromptLine(i.Description, i.AppName, i.Command);
            }
        }

    }
}