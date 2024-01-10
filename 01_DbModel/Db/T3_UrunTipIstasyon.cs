using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_UrunTipIstasyon
    {
        public Guid Id { get; set; }
        public Guid TipId { get; set; }
        public Guid IstasyonId { get; set; }
        public float Zaman { get; set; }

        public virtual T3_Istasyon Istasyon { get; set; }
        public virtual T3_UrunTip Tip { get; set; }
    }
}
