using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Personel
    {
        public T3_Personel()
        {
            T3_CalismaPersonelBosZaman = new HashSet<T3_CalismaPersonelBosZaman>();
            T3_UretimPersonel = new HashSet<T3_UretimPersonel>();
        }

        public Guid Id { get; set; }
        public string Kod { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Barkod { get; set; }

        public virtual ICollection<T3_CalismaPersonelBosZaman> T3_CalismaPersonelBosZaman { get; set; }
        public virtual ICollection<T3_UretimPersonel> T3_UretimPersonel { get; set; }
    }
}
