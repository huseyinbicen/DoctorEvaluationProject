using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace DoctorEvaluationProject.Dialogs
{
    internal class DestekDialog : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(this.MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result;

            var ticketNumber = new Random().Next(0, 20000);

            await context.PostAsync($"Mesajınız : '{message.Text}'. Teşekkürler ; Sorun en yakın zamanda düzelecektir.");

            context.Done(ticketNumber);
        }
    }
}