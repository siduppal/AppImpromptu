# AppImpromptu

Attempt to generate GPT prompts to allow a Teams Bot to automatically identify apps from the Teams catalog that can assist the user.

# Dost thou seek a TLDR?
Just head on over here and fill in a value for prompt to experiment with the model predicting which app from Teams catalog could help. Remember that this only has ~100 examples to learn from, since this is an experiment.

[Pray thee, click here](https://foundrytoolkit.azurewebsites.net/playgroundv2?session=4ba4dc3d-ce4d-4d52-9411-5fb51e7a6e0a) 👈

## What's going on here?
- Build the code locally to generate the executable.
- Run `AppDefnParser.exe` to get it to parse the `appDefinition.json` file which must be located alongside the exe, and emit a bunch of prompts to feed GPT.
- GPT completion API has ~4000 token limit for prompts, which is ok for experimentation, so you need to trim the prompts generated.
- To take only 100 prompts from the main file, use may use PowerShell like so (feel free to update the number as you wish).
       `Get-Content -TotalCount 100 .\prompts.txt > 100promts.txt`
- Head on over to [Foundry Playground](https://foundrytoolkit.azurewebsites.net/playgroundv2)
- Paste the prompt in.
- Now type a phrase that you expect a user to send, prefixing it with `"I: "` and end it with a semi-colon. Submit the prompt to GPT to examine the results.

### Note:
If using the templating support in Foundry, remember to set the last statement as:
`I: {{prompt}};`

and for InputParameters, specify a value for the template parameter like so:

`{"prompt": "what are our leads in Zoho?"}` 

and you should see the model suggest you to use `@Zoho CRM Leads`! 🎉

![image](https://user-images.githubusercontent.com/7799064/208786239-d00f3116-4e01-4862-bcc2-f056b041e7d1.png)
