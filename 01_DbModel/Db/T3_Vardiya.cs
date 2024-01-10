using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Vardiya
    {
        public T3_Vardiya()
        {
            T3_IstasyonVardiya = new HashSet<T3_IstasyonVardiya>();
            T3_VardiyaMola = new HashSet<T3_VardiyaMola>();
        }

        public Guid Id { get; set; }
        public string Kod { get; set; }
        public TimeSpan Baslangic { get; set; }
        public TimeSpan Bitis { get; set; }
        public int Zaman { get; set; }
        public bool GunPazartesi { get; set; }
        public bool GunSali { get; set; }
        public bool GunCarsamba { get; set; }
        public bool GunPersembe { get; set; }
        public bool GunCuma { get; set; }
        public bool GunCumartesi { get; set; }
        public bool GunPazar { get; set; }

        public virtual ICollection<T3_IstasyonVardiya> T3_IstasyonVardiya { get; set; }
        public virtual ICollection<T3_VardiyaMola> T3_VardiyaMola { get; set; }
    }
}
