using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Uretim
    {
        public T3_Uretim()
        {
            T3_UretimCalisma = new HashSet<T3_UretimCalisma>();
            T3_UretimDurus = new HashSet<T3_UretimDurus>();
            T3_UretimPersonel = new HashSet<T3_UretimPersonel>();
            T3_UretimPlanliDurus = new HashSet<T3_UretimPlanliDurus>();
        }

        public Guid Id { get; set; }
        public Guid UrunId { get; set; }
        public Guid IstasyonId { get; set; }
        public string Kod { get; set; }
        public string Barkod { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public DateTime BitisHedef { get; set; }
        public int Miktar { get; set; }
        public int SureGercek { get; set; }
        public int SureHedef { get; set; }
        public int SureHedefG { get; set; }

        public virtual T3_Istasyon Istasyon { get; set; }
        public virtual T3_Urun Urun { get; set; }
        public virtual ICollection<T3_UretimCalisma> T3_UretimCalisma { get; set; }
        public virtual ICollection<T3_UretimDurus> T3_UretimDurus { get; set; }
        public virtual ICollection<T3_UretimPersonel> T3_UretimPersonel { get; set; }
        public virtual ICollection<T3_UretimPlanliDurus> T3_UretimPlanliDurus { get; set; }
    }
}
