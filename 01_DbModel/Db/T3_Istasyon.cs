using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Istasyon
    {
        public T3_Istasyon()
        {
            T3_Calisma = new HashSet<T3_Calisma>();
            T3_IstasyonDurus = new HashSet<T3_IstasyonDurus>();
            T3_IstasyonVardiya = new HashSet<T3_IstasyonVardiya>();
            T3_PlanliCalisma = new HashSet<T3_PlanliCalisma>();
            T3_PlanliDurus = new HashSet<T3_PlanliDurus>();
            T3_Uretim = new HashSet<T3_Uretim>();
            T3_UrunTipIstasyon = new HashSet<T3_UrunTipIstasyon>();
        }

        public Guid Id { get; set; }
        public Guid LokasyonId { get; set; }
        public string LokasyonTree { get; set; }
        public string Kod { get; set; }
        public int Sira { get; set; }
        public int SiraNo { get; set; }
        public string IpAdres { get; set; }
        public string Barkod { get; set; }
        public int Reset { get; set; }
        public int SnSorgu { get; set; }
        public DateTime Zaman { get; set; }

        public virtual T3_Lokasyon Lokasyon { get; set; }
        public virtual ICollection<T3_Calisma> T3_Calisma { get; set; }
        public virtual ICollection<T3_IstasyonDurus> T3_IstasyonDurus { get; set; }
        public virtual ICollection<T3_IstasyonVardiya> T3_IstasyonVardiya { get; set; }
        public virtual ICollection<T3_PlanliCalisma> T3_PlanliCalisma { get; set; }
        public virtual ICollection<T3_PlanliDurus> T3_PlanliDurus { get; set; }
        public virtual ICollection<T3_Uretim> T3_Uretim { get; set; }
        public virtual ICollection<T3_UrunTipIstasyon> T3_UrunTipIstasyon { get; set; }
    }
}
