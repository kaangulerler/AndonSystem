using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_UrunTip
    {
        public T3_UrunTip()
        {
            T3_Urun = new HashSet<T3_Urun>();
            T3_UrunTipIstasyon = new HashSet<T3_UrunTipIstasyon>();
        }

        public Guid Id { get; set; }
        public string Product { get; set; }
        public float Rated_Volt { get; set; }
        public int Panel_Width { get; set; }
        public string Panel_Type { get; set; }
        public int Panel_Curr { get; set; }
        public float Shortc_Curr { get; set; }
        public int Ct { get; set; }
        public int Vt { get; set; }
        public int Vt_With { get; set; }
        public int Vt_Rem { get; set; }
        public int Vt_Fix { get; set; }
        public int Sa { get; set; }
        public string Cap_Volt_Ind { get; set; }
        public string Es_Present { get; set; }
        public string Es_Type_Subs { get; set; }
        public int Ct_Sec_Con { get; set; }
        public int Vt_Sec_Con { get; set; }

        public virtual ICollection<T3_Urun> T3_Urun { get; set; }
        public virtual ICollection<T3_UrunTipIstasyon> T3_UrunTipIstasyon { get; set; }
    }
}
