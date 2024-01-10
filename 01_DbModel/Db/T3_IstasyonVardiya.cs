using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_IstasyonVardiya
    {
        public Guid Id { get; set; }
        public Guid VardiyaId { get; set; }
        public Guid IstasyonId { get; set; }
        public DateTime Zaman { get; set; }
        public int Hedef { get; set; }
        public bool Aktif { get; set; }

        public virtual T3_Istasyon Istasyon { get; set; }
        public virtual T3_Vardiya Vardiya { get; set; }
    }
}
