namespace WindowsFormsApp1
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;

    public class DoctorEvaluationDatabase : DbContext
    {
        public DoctorEvaluationDatabase()
            : base("name=DoctorEvaluationDatabase")
        {
        }

        public virtual DbSet<Hastaneler> Hospitals { get; set; }
        public virtual DbSet<Doktorlar> Doctors { get; set; }
        public virtual DbSet<Degerlendirme> Evaluations { get; set; }
    }

    public class Hastaneler
    {
        [Key]
        public int Id { get; set; }

        public string ad { get; set; }
    }

    public class Doktorlar
    {
        [Key]
        public int Id { get; set; }

        public string ad { get; set; }

        public string soyad { get; set; }

        public string brans { get; set; }

        public int H_id { get; set; }
    }

    public class Degerlendirme
    {
        [Key]
        public int Id { get; set; }

        public int yildiz { get; set; }

        public string yorum { get; set; }

        public int D_id { get; set; }
    }
}