using DoctorEvaluationProject.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace DoctorEvaluationProject
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //DBolustur();
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }

        public void DBolustur()
        {
            using (DoctorEvaluation db = new DoctorEvaluation())
            {
                Hastaneler hs = new Hastaneler();
                hs.ad = "Üsküdar Devlet Hastanesi";
                db.Hospitals.Add(hs);
                db.SaveChanges();


                Doktorlar dktr = new Doktorlar();
                dktr.ad = "Hüseyin";
                dktr.soyad = "Biçen";
                dktr.brans = "Kalp Cerrahı";
                dktr.H_id = 1;
                db.Doctors.Add(dktr);
                db.SaveChanges();

                Degerlendirme dgr = new Degerlendirme();
                dgr.D_id = 1;
                dgr.yildiz = 4;
                dgr.yorum = "Cok ilgilendi.";
                db.Evaluations.Add(dgr);
                db.SaveChanges();


            }
        }
    }
}
