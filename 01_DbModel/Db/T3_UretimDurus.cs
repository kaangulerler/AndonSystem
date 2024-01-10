using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_UretimDurus
    {
        public Guid Id { get; set; }
        public Guid UretimId { get; set; }
        public Guid DurusTipId { get; set; }
        public string DurusTip { get; set; }
        public string Kod { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public int Zaman { get; set; }

        public virtual T3_DurusTip DurusTipNavigation { get; set; }
        public virtual T3_Uretim Uretim { get; set; }
    }
}
