using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_DbModel.AppModel
{
    internal class T3
    {
    }

    public enum T3_Hat
    {
        MWS = 0,
        ZS1 = 1,
        ZS2 = 2,
    }

    public class T3_Mesaj
    {
        public Guid Id { get; set; } 
        public int Fonksiyon { get; set; }  // 1 : Üretim   , 2: Duruş
        public string Nesne { get; set; } = "";
        public bool Tip { get; set; } = false; // False = Komut, True = Cevap
        public bool Durum { get; set; } = false; // False = Yazılamadı, True = Yazıldı
    }

    public class T3_MesajBarkod
    {
        public int BarkodTip { get; set; }
        public string Baslik { get; set; } = "";
        public string Mesaj { get; set; } = "";
        public string Barkod { get; set; } = "";
        public int PanelTip { get; set; }
    }

    public class T3_IstasyonModel
    {
        public Guid Id { get; set; }
        public string Kod { get; set; } = "";
        public int  Durum { get; set; }
        public string Calisma_Kod { get; set; } = "";
        public string Calisma_Bas { get; set; } = "";
        public string Calisma_Bit { get; set; } = "";
        public int Calisma_Target { get; set; }
        public int Calisma_Actual { get; set; }
        public int Calisma_Delta { get; set; }
    }

}
