using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_CalismaPlanliDurus
    {
        public Guid Id { get; set; }
        public Guid CalismaId { get; set; }
        public string Kod { get; set; }
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public int Zaman { get; set; }

        public virtual T3_Calisma Calisma { get; set; }
    }
}
