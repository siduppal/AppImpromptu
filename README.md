# AppImpromptu

Attempt to generate GPT prompts to allow a Teams Bot to automatically identify apps from the Teams catalog that can assist the user.

## Dost thou seek a TLDR?
Just head on over here and fill in a value for prompt to experiment with the model predicting which app from Teams catalog could help. Remember that this only has ~100 examples to learn from, since this is an experiment.

[Pray thee, click here](https://foundrytoolkit.azurewebsites.net/playgroundv2?session=4ba4dc3d-ce4d-4d52-9411-5fb51e7a6e0a) 👈

## What doth transpire here?
- Build the code locally to create an executable file
- Run `AppDefnParser.exe` and use the `appDefinition.json` file to generate prompts for GPT
- Trim the prompts to 4000 tokens or less for experimentation
- Use PowerShell to select 100 prompts from the main file (e.g. `Get-Content -TotalCount 100 .\prompts.txt > 100promts.txt`)
- Go to the Foundry Playground website
- Paste the prompts in and test them by entering a phrase and submitting it to GPT (e.g. "I: [phrase];")

## Doth thou wish to use templates?
To use templating in Foundry:

- Set the last statement as `I: {{prompt}};`
- Specify a value for the "prompt" template parameter in the InputParameters, e.g. {"prompt": "what are our leads in Zoho?"}
- If everything is set up correctly, the model should suggest using @Zoho CRM Leads 🎉
