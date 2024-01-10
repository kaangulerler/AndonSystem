using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_DbModel.AppModel
{
     
    public class Urun
    {
        public Guid Id { get; set; }

        public Guid TipId { get; set; }

        public int ProjectId { get; set; }

        public string ProjectName { get; set; } = "";

        public string BomNo { get; set; } = ""; 

        public string Switchgear { get; set; } = "";

        public string Panel_No { get; set; } = "";

        public int Shortc_Time { get; set; }

        public int Discon { get; set; }

        public string Kod { get; set; } = "";
        
        public string Barkod { get; set; } = "";

        public UrunTip Tip { get; set; } = new UrunTip();
    }

    public class UrunTip
    {
        public Guid Id { get; set; }

        public string Product { get; set; } = "";

        public Single Rated_Volt { get; set; }

        public int Panel_Width { get; set; }

        public string Panel_Type { get; set; } = "";

        public int Panel_Curr { get; set; }

        public Single Shortc_Curr { get; set; }

        public int Ct { get; set; }

        public int Vt { get; set; }

        public int Vt_With { get; set; }

        public int Vt_Rem { get; set; }

        public int Vt_Fix { get; set; }

        public int Sa { get; set; }

        public string Cap_Volt_Ind { get; set; } = "";

        public string Es_Present { get; set; } = "";

        public string Es_Type_Subs { get; set; } = "";

        public int Ct_Sec_Con { get; set; }

        public int Vt_Sec_Con { get; set; }

        public List<UrunTipIstasyon> Liste { get; set; } = new List<UrunTipIstasyon>();
    }

    public class UrunTipIstasyon
    {
        public Guid Id { get; set; } = new Guid();
        public Guid TipId { get; set; } = new Guid();
        public Guid IstasyonId { get; set; } = new Guid();
        public string Istasyon { get; set; } = "";
        public float Zaman { get; set; } = 0;
    }

    public class Excel_Urun_Satır
    {
        public string ProjectId { get; set; } = "";
        public string ProjectName { get; set; } = "";
        public string Switchgear { get; set; } = "";
        public string BomNo { get; set; } = "";
        public string Panel_No { get; set; } = "";
        public string Shortc_Time { get; set; } = "";
        public string Discon { get; set; } = "";
        public string Product { get; set; } = "";
        public string Rated_Volt { get; set; } = "";
        public string Panel_Width { get; set; } = "";
        public string Panel_Type { get; set; } = "";
        public string Panel_Curr { get; set; } = "";
        public string Shortc_Curr { get; set; } = "";
        public string Ct { get; set; } = "";
        public string Vt { get; set; } = "";
        public string Vt_With { get; set; } = "";
        public string Vt_Rem { get; set; } = "";
        public string Vt_Fix { get; set; } = "";
        public string Sa { get; set; } = "";
        public string Cap_Volt_Ind { get; set; } = "";
        public string Es_Present { get; set; } = "";
        public string Es_Type_Subs { get; set; } = "";
        public string Ct_Sec_Con { get; set; } = "";
        public string Vt_Sec_Con { get; set; } = "";
        public string Hata { get; set; } = "";
    }
}
