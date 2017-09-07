using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using DoctorEvaluationProject.Database;

namespace DoctorEvaluationProject.Dialogs
{


    [Serializable]
    public class DoktorSorgulamaDialog : IDialog<object>
    {

        private string Isim;
        private string Soyisim;
        private string Brans;
        private string Hastane_Adi;


        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Doktor Sorgulama Bölümündesiniz!");
            await context.PostAsync("Doktor ismini yazınız?");
            context.Wait(this.D_isim);
        }

        private async Task D_isim(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            //veriKaydet.Isim = activity.Text;
            await context.PostAsync("Doktor Soyismini yazınız?");
            context.Wait(D_Soyisim);
        }

        private async Task D_Soyisim(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            // veriKaydet.Soyisim = activity.Text;
            await context.PostAsync("Doktorun Branşı nedir?");
            context.Wait(D_Hastane);
        }

        private async Task D_Unvan(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            //veriKaydet.Brans = activity.Text;
            await context.PostAsync("Doktorun Çalıştığı hastaneyi yazınız?");
            context.Wait(D_isim);
        }

        private async Task D_Hastane(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            //veriKaydet.Hastane_Adi = activity.Text;
            await context.PostAsync("Bilgiler alındı... Lütfen bekleyiniz.");
            context.Wait(D_Kaydet);

        }

        private async Task D_Kaydet(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            DoctorEvaluation db = new DoctorEvaluation();

            Hastaneler HS = new Hastaneler();
            Doktorlar DR = new Doktorlar();
            Degerlendirme DGR = new Degerlendirme();

            Doktorlar DRx = null;
            DRx = db.Doctors.Where(x => x.ad == Isim && x.soyad == Soyisim).FirstOrDefault();
            if (DRx != null)
            {
                List<Degerlendirme> nesne = db.Evaluations.Where(x => x.Id == DRx.Id).ToList();
                foreach (var item in nesne)
                {
                    await context.PostAsync("Yorum : " + item.yorum.ToString() + "\nPuan : " + item.yildiz.ToString());
                }
            }
            else
            {
                await context.PostAsync("Böyle bir kayıt bulunamadı. Değerlendirme bölümünden doktor değerlendirebilirsiniz.");
            }

            context.Wait(D_isim);
        }
    }
}