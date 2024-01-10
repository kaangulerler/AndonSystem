using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_UretimCalisma
    {
        public Guid Id { get; set; }
        public Guid UretimId { get; set; }
        public Guid CalismaId { get; set; }

        public virtual T3_Calisma Calisma { get; set; }
        public virtual T3_Uretim Uretim { get; set; }
    }
}
