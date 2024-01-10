using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_VardiyaMola
    {
        public Guid Id { get; set; }
        public Guid VardiyaId { get; set; }
        public string Kod { get; set; }
        public TimeSpan Baslangic { get; set; }
        public TimeSpan Bitis { get; set; }
        public int Zaman { get; set; }

        public virtual T3_Vardiya Vardiya { get; set; }
    }
}
