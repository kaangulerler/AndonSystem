using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_PlanliCalisma
    {
        public Guid Id { get; set; }
        public Guid IstasyonId { get; set; }
        public string Kod { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public int Zaman { get; set; }
        public int Hedef { get; set; }

        public virtual T3_Istasyon Istasyon { get; set; }
    }
}
