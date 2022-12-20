# AppImpromptu

Attempt to generate GPT prompts to allow a Teams Bot to automatically identify apps from the Teams catalog that can assist the user.

## How to use
- Build the code locally to generate the executable.
- Run `AppDefnParser.exe` to get it to parse the `appDefinition.json` file which must be located alongside the exe, and emit lines of the following form:
     `<description>`|@`<appName>` `<appCommand>`
  
     For example: `AppDefnParser.exe > prompts.txt`

     `<description>`, `<appName>`, `<appCommand>` are values hydrated from the app-manifest of each app in Teams' catalog.
     
- Now you can feed these prompts to GPT. However, the Playground has a limit on the tokens (while fine-tuning has higher limits).
- To take only 500 prompts from the main file, use may use PowerShell like so
       `Get-Content -TotalCount 500 .\prompts.txt > 500promts.txt`
  
