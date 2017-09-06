using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DoctorEvaluationDatabase db = new DoctorEvaluationDatabase();

            string isim = textBox1.Text;
            string soyisim = textBox2.Text;
            string brans = textBox3.Text;
            string hastaneAdi = textBox4.Text;
            int yildiz = Convert.ToInt32(textBox5.Text);
            string yorum = textBox6.Text;

            Hastaneler HS = new Hastaneler();
            Doktorlar DR = new Doktorlar();
            Degerlendirme DGR = new Degerlendirme();

            Doktorlar DRv = null;
            DRv = db.Doctors.Where(x => x.ad == isim && x.soyad == soyisim).FirstOrDefault();

            if (DRv != null)
            {
                DGR.yildiz = yildiz;
                DGR.yorum = yorum;
                DGR.D_id = DR.Id;
                db.Evaluations.Add(DGR);
                db.SaveChanges();
            }
            else
            {
                DR.ad = isim;
                DR.soyad = soyisim;
                DR.brans = brans;

                Hastaneler HSv = null;
                HSv = db.Hospitals.Where(x => x.ad == hastaneAdi).FirstOrDefault();

                if (HSv != null)
                {
                    DR.H_id = HS.Id;
                    db.Doctors.Add(DR);
                    db.SaveChanges();

                    DGR.yildiz = yildiz;
                    DGR.yorum = yorum;
                    DGR.D_id = DR.Id;
                    db.Evaluations.Add(DGR);
                    db.SaveChanges();
                }
                else
                {
                    HS.ad = hastaneAdi;
                    db.Hospitals.Add(HS);
                    db.SaveChanges();

                    DR.H_id = 3;
                    db.Doctors.Add(DR);
                    db.SaveChanges();

                    DGR.yildiz = yildiz;
                    DGR.yorum = yorum;
                    DGR.D_id = DR.Id;
                    db.Evaluations.Add(DGR);
                    db.SaveChanges();
                }
            }
        }
    }
}
