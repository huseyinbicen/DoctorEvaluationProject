using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Threading;

namespace DoctorEvaluationProject.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {

        private const string DrDegerlendirme = "Doktor Değerlendirme";

        private const string DrSorgulama = "Doktor Sorgulama";


        public Task StartAsync(IDialogContext context)
        {
            
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            await context.PostAsync($"Merhaba, Doktor Değerlendiren ve Sorgulayan Uygulamaya Hoşgeldiniz.");
            await context.PostAsync($"Adın ne?");
            context.Wait(MessageWelcome);
        }

        private async Task MessageWelcome(IDialogContext context, IAwaitable<object> result)
        {
            var isim = await result as Activity;
            await context.PostAsync($"Hoşgeldin {isim.Text}");
            context.Wait(MessageDestek);
        }


        public virtual async Task MessageDestek(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            if (message.Text.ToLower().Contains("yardım") || message.Text.ToLower().Contains("destek") || message.Text.ToLower().Contains("sorun"))
            {
                await context.Forward(new DestekDialog(), this.ResumeAfterSupportDialog, message, CancellationToken.None);
            }
            else
            {
                this.ShowOptions(context);
            }
        }





        private async Task OnOptionSelected(IDialogContext context, IAwaitable<string> result)
        {
            try
            {
                string optionSelected = await result;

                switch (optionSelected)
                {
                    case DrDegerlendirme:
                        context.Call(new DoktorDegerlendirmeDialog(), this.ResumeAfterOptionDialog);
                        break;

                    case DrSorgulama:
                        context.Call(new DoktorSorgulamaDialog(), this.ResumeAfterOptionDialog);
                        break;
                }
            }
            catch (TooManyAttemptsException ex)
            {
                await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

                context.Wait(this.MessageReceivedAsync);
            }
        }




        public void ShowOptions(IDialogContext context)
        {
            PromptDialog.Choice(context, this.OnOptionSelected, new List<string>() { DrDegerlendirme, DrSorgulama }, "Birini Seç!", "Hiçbiri", 3);
        }


        private async Task ResumeAfterSupportDialog(IDialogContext context, IAwaitable<object> result)
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