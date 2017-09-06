
using System;
    using System.Threading.Tasks;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Connector;
    using System.Collections.Generic;
using DoctorEvaluationProject.Database;
using System.Linq;

namespace DoctorEvaluationProject.Dialogs
{
    

    [Serializable]
    public class DoktorDegerlendirmeDialog : IDialog<object>
    {
        private string Isim;
        private string Soyisim;
        private string Brans;
        private string Hastane_Adi;
        private int Yildiz;
        private string Yorum;


        public async Task StartAsync(IDialogContext context)
        {
            await context.PostAsync("Doktor Değerlendirme Bölümündesiniz!");
            await context.PostAsync("Doktor ismini yazınız?");
            context.Wait(this.D_isim);
        }

        //public void DatabaseCalistir()
        //{
        //    DoctorEvaluation2 db = new DoctorEvaluation2();
        //    Doktorlar DR = new Doktorlar();
        //    Hastaneler HS = new Hastaneler();
        //    Degerlendirme DGR = new Degerlendirme();


        //    DR.ad = isim;
        //    DR.soyad = soyisim;
        //    DR.unvan = brans;
        //    DR.H_id = 2;
        //    db.Doktorlar.Add(DR);
        //    db.SaveChanges();

        //    HS.ad = hastaneAdi;
        //    db.Hastaneler.Add(HS);
        //    db.SaveChanges();

        //    DGR.yildiz = yildiz;
        //    DGR.yorum = yorum;
        //    DGR.D_id = 2;
        //    db.Degerlendirme.Add(DGR);
        //    db.SaveChanges();

        //}

        private async Task D_isim(IDialogContext context, IAwaitable<object> result)
        {

            var activity = await result as Activity;
            Isim = activity.Text;
            await context.PostAsync("Doktor Soyismini yazınız?");
            context.Wait(D_Soyisim);
        }

        private async Task D_Soyisim(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            Soyisim = activity.Text;
            await context.PostAsync("Doktor Branşını yazınız?");
            context.Wait(D_Unvan);
        }

        private async Task D_Unvan(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            Brans = activity.Text;
            await context.PostAsync("Doktorun Çalıştığı Hastaneyi yazınız?");
            context.Wait(D_Hastane);
        }

        private async Task D_Hastane(IDialogContext context, IAwaitable<object> result)
        {
            //var reply = context.MakeMessage();
            //reply.AttachmentLayout = AttachmentLayoutTypes.List;
            var activity = await result as Activity;
            Hastane_Adi = activity.Text;
            await context.PostAsync("Doktoru Değerlendirin: \n - 1 \n - 2 \n - 3 \n - 4 \n - 5");
            context.Wait(D_Yildiz);
        }

        private async Task D_Yildiz(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;
            Yildiz = Convert.ToInt16(activity.Text);
            await context.PostAsync("Doktora Yorum yazınız.");
            context.Wait(D_Yorum);
        }

        private async Task D_Yorum(IDialogContext context, IAwaitable<object> result)
        {
            //DataIslemleri veri = new DataIslemleri();
            var activity = await result as Activity;
            Yorum = activity.Text;
            await context.PostAsync("Bilgiler kontrol ediltikten sonra veri tabanına kaydedilecektir. \n Teşekkür Ederiz.");
            //DatabaseCalistir();
            //veri.VeriKontrol(Isim, Soyisim, Brans, Hastane_Adi, Yildiz, Yorum);
            context.Wait(D_Kaydet);
        }

        private async Task D_Kaydet(IDialogContext context, IAwaitable<IMessageActivity> result)
        {

            DoctorEvaluation db = new DoctorEvaluation();



            Hastaneler HS = new Hastaneler();
            Doktorlar DR = new Doktorlar();
            Degerlendirme DGR = new Degerlendirme();

            Doktorlar DRv = null;
            DRv = db.Doctors.Where(x => x.ad == Isim && x.soyad == Soyisim).FirstOrDefault();

            if (DRv != null)
            {
                DGR.yildiz = Yildiz;
                DGR.yorum = Yorum;
                DGR.D_id = DR.Id;
                db.Evaluations.Add(DGR);
                db.SaveChanges();
            }
            else
            {
                DR.ad = Isim;
                DR.soyad = Soyisim;
                DR.brans = Brans;

                Hastaneler HSv = null;
                HSv = db.Hospitals.Where(x => x.ad == Hastane_Adi).FirstOrDefault();

                if (HSv != null)
                {
                    DR.H_id = HS.Id;
                    db.Doctors.Add(DR);
                    db.SaveChanges();

                    DGR.yildiz = Yildiz;
                    DGR.yorum = Yorum;
                    DGR.D_id = DR.Id;
                    db.Evaluations.Add(DGR);
                    db.SaveChanges();
                }
                else
                {
                    HS.ad = Hastane_Adi;
                    db.Hospitals.Add(HS);
                    db.SaveChanges();

                    DR.H_id = 3;
                    db.Doctors.Add(DR);
                    db.SaveChanges();

                    DGR.yildiz = Yildiz;
                    DGR.yorum = Yorum;
                    DGR.D_id = DR.Id;
                    db.Evaluations.Add(DGR);
                    db.SaveChanges();
                }

            }

            await context.PostAsync("Veriler Veri Tabanına kaydedildi.");
            
            context.Wait(D_isim);
        }



        //private async Task D_Soyisim(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;
        //    doktor.D_Soyad = activity.Text;
        //    await context.PostAsync("Doktorun Ünvanı nedir?");
        //    context.Wait(D_Hastane);
        //}

        //private async Task D_Unvan(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;
        //    doktor.D_Unvan = activity.Text;
        //    await context.PostAsync("Doktorun Çalıştığı hastaneyi yazınız?");
        //    context.Wait(D_isim);
        //}

        //private async Task D_Hastane(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;
        //    doktor.D_Hastane = activity.Text;
        //    await context.PostAsync("Bilgiler alındı... Lütfen bekleyiniz.");
        //    context.Wait<int>(D_derece);
        //}

        //private void ShowOptions(IDialogContext context)
        //{
        //    PromptDialog.Choice(context, D_derece, new List<int>() { 1,2,3,4,5 }, "Değerlendir", "Hiçbiri", 3);
        //}

        //private async Task D_derece(IDialogContext context, IAwaitable<int> result)
        //{
        //    try
        //    {
        //        int optionSelected = await result;
        //        int deger = 0;
        //        switch (optionSelected)
        //        {
        //            case 1:
        //                deger = 1;
        //                break;

        //            case 2:
        //                deger = 2;
        //                break;
        //            case 3:
        //                deger = 3;
        //                break;
        //            case 4:
        //                deger = 4;
        //                break;
        //            case 5:
        //                deger = 5;
        //                break;
        //        }
        //        context.Wait(Yorum);
        //    }
        //    catch (TooManyAttemptsException ex)
        //    {
        //        await context.PostAsync($"Ooops! Too many attemps :(. But don't worry, I'm handling that exception and you can try again!");

        //        context.Wait<int>(this.D_derece);
        //    }


        //}
        //private async Task Yorum(IDialogContext context, IAwaitable<object> result)
        //{
        //    var activity = await result as Activity;
        //    doktor.yorum.Add(activity.Text);
        //    await context.PostAsync("Bilgiler alındı... Lütfen bekleyiniz.");
        //    context.Wait(Yorum);
        //}

    }
}