using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Calisma
    {
        public T3_Calisma()
        {
            T3_CalismaIstasyonBosZaman = new HashSet<T3_CalismaIstasyonBosZaman>();
            T3_CalismaPersonelBosZaman = new HashSet<T3_CalismaPersonelBosZaman>();
            T3_CalismaPlanliDurus = new HashSet<T3_CalismaPlanliDurus>();
            T3_UretimCalisma = new HashSet<T3_UretimCalisma>();
        }

        public Guid Id { get; set; }
        public Guid IstasyonId { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Kod { get; set; }
        public bool Aktif { get; set; }
        public double Ortalama { get; set; }
        public int Hedef { get; set; }
        public int Aktuel { get; set; }
        public int Delta { get; set; }

        public virtual T3_Istasyon Istasyon { get; set; }
        public virtual ICollection<T3_CalismaIstasyonBosZaman> T3_CalismaIstasyonBosZaman { get; set; }
        public virtual ICollection<T3_CalismaPersonelBosZaman> T3_CalismaPersonelBosZaman { get; set; }
        public virtual ICollection<T3_CalismaPlanliDurus> T3_CalismaPlanliDurus { get; set; }
        public virtual ICollection<T3_UretimCalisma> T3_UretimCalisma { get; set; }
    }
}
