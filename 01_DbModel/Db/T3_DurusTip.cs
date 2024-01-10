using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_DurusTip
    {
        public T3_DurusTip()
        {
            InverseDurusTip = new HashSet<T3_DurusTip>();
            T3_IstasyonDurus = new HashSet<T3_IstasyonDurus>();
            T3_UretimDurus = new HashSet<T3_UretimDurus>();
        }

        public Guid Id { get; set; }
        public Guid? DurusTipId { get; set; }
        public string DurusTipTree { get; set; }
        public string Kod { get; set; }
        public string Barkod { get; set; }

        public virtual T3_DurusTip DurusTip { get; set; }
        public virtual ICollection<T3_DurusTip> InverseDurusTip { get; set; }
        public virtual ICollection<T3_IstasyonDurus> T3_IstasyonDurus { get; set; }
        public virtual ICollection<T3_UretimDurus> T3_UretimDurus { get; set; }
    }
}
