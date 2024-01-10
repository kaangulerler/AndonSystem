using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_UretimPersonel
    {
        public Guid Id { get; set; }
        public Guid UretimId { get; set; }
        public Guid PersonelId { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public int Zaman { get; set; }

        public virtual T3_Personel Personel { get; set; }
        public virtual T3_Uretim Uretim { get; set; }
    }
}
