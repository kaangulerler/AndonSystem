using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Urun
    {
        public T3_Urun()
        {
            T3_Uretim = new HashSet<T3_Uretim>();
        }

        public Guid Id { get; set; }
        public Guid TipId { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Switchgear { get; set; }
        public string Panel_No { get; set; }
        public int Shortc_Time { get; set; }
        public int Discon { get; set; }
        public string Bom { get; set; }
        public string Kod { get; set; }
        public string Barkod { get; set; }

        public virtual T3_UrunTip Tip { get; set; }
        public virtual ICollection<T3_Uretim> T3_Uretim { get; set; }
    }
}
