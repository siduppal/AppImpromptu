using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
 */

public class Program
{
    public static void ProcessComposeExtensions(string appName, dynamic a)
    {
        // Handle input extensions aka composeExtensions
        var composeExtensionCommands = new List<string>();

        if (a.inputExtensions != null)
        {
            foreach (var ie in a.inputExtensions)
            {
                if (ie.commands != null)
                {
                    foreach (var iec in ie.commands)
                    {
                        composeExtensionCommands.Add(iec.title.ToString());
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
                        composeExtensionCommands.Add(cec.title.ToString());
                    }
                }
            }
        }

        foreach (var cec in composeExtensionCommands)
        {
            EmitPrompt(cec, appName, cec);
        }
    }

    public static void ProcessBots(string appName, dynamic a)
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
                                EmitPrompt(c.description.ToString(), appName, c.title.ToString());
                            }
                        }
                    }
                }
            }
        }
    }

    public static void EmitPrompt(string description, string appName, string command)
    {
        Console.WriteLine($"{description}|@{appName} {command}");
    }

    public static void Main()
    {
        var appDefintions = File.ReadAllText("appDefinitions.json");
        var appDatas = JsonConvert.DeserializeObject<dynamic>(appDefintions)?.appDefinitions;

        if (appDatas != null)
        {
            foreach (var a in appDatas)
            {
                var appName = a.name.Type == JTokenType.String ? a.name.ToString() : a.name.@short.ToString();

                ProcessComposeExtensions(appName, a);

                ProcessBots(appName, a);
                
            }
        }

    }
}