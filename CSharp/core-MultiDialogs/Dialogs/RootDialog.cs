namespace MultiDialogsBot.Dialogs
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;

    [Serializable]
    public class RootDialog : IDialog<object>
    {
        private const string FlightsOption = "Academy";

        private const string HotelsOption = "Corporate workspace";
        
        private const string SoftwareOption = "Software";
        
        private const string HardwareOption = "Hardware";

        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }
        
        public async Task Startnext(IDialogContext context)
        {
            context.Wait(this.StartnextAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("help") || message.Text.ToLower().Contains("support") || message.Text.ToLower().Contains("problem"))
            {
                await context.Forward(new SupportDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
            }
            else
            {
                this.ShowOptions(context);
            }
        }
        
        public virtual async Task StartnextAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
           var message=await result;
           
           this.ShowSecondOptions(context);
        }

        private void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { FlightsOption, HotelsOption }, "Is your issue concerned with Academy or Workspace?", "Not a valid option", 3);
        }
        
        private void ShowSecondOptions(IDialogContext context)
        {
           PromptDialog.Choice(context, this.OnSecondOptionSelected, new List<string>() {SoftwareOption, HardwareOption },"Is your issue concerned with Hardware or Software?","Not a valid option", 3);
        }

        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case FlightsOption:
                        ShowSecondOptions(context);
                        context.Call(new FlightsDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case HotelsOption:
                        context.Call(new HotelsDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }
        
        private async Task OnSecondOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case SoftwareOption:
                        context.Call(new SoftwareDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case HardwareOption:
                        context.Call(new HardwareDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }

        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<int> result)
        {
            var ticketNumber = await result;

            await context.PostAsync($"Thanks for contacting our support team. Your ticket number is {ticketNumber}.");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task ResumeAfterOptionDialog(IDialogContext context, IAwaitable<object> result)
        {
            try
            {
                var message = await result;
            }
            catch (Exception ex)
            {
                await context.PostAsync($"Failed with message: {ex.Message}");
            }
            finally
            {
                context.Wait(this.MessageReceivedAsync);
            }
        }
    }
}
