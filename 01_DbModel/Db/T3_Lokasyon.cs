using System;
using System.Collections.Generic;

namespace _01_DbModel.Db
{
    public partial class T3_Lokasyon
    {
        public T3_Lokasyon()
        {
            InverseUst = new HashSet<T3_Lokasyon>();
            T3_Istasyon = new HashSet<T3_Istasyon>();
        }

        public Guid Id { get; set; }
        public Guid? UstId { get; set; }
        public string Kod { get; set; }

        public virtual T3_Lokasyon Ust { get; set; }
        public virtual ICollection<T3_Lokasyon> InverseUst { get; set; }
        public virtual ICollection<T3_Istasyon> T3_Istasyon { get; set; }
    }
}
