using _01_DbModel.Db;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_Servis.Model
{
    internal class ServisModel
    {
    }

    public class SayfaModel
    {
        public Guid IstasyonId { get; set; } = new();
        public string Istasyon { get; set; } = "";
        public string Durum { get; set; } = "";
        public int DurumTip { get; set; } = 0;
        public bool Online { get; set; } = false;
        public CalismaModel Calisma { get; set; } = new();
        public List<UretimModel> ListUretimAktif { get; set; } = new();
        public List<UretimModel> ListUretim { get; set; } = new();
        public string AktifUretimModel { get; set; } = "";
        public string DurusUretimModel { get; set; } = "";
        public List<string[]> DashboardModel { get; set; } = new();
    }

    public class CalismaModel
    {
        public string Kod { get; set; } = "";
        public string Bas { get; set; } = "";
        public string Bit { get; set; } = "";
        public int Hedef { get; set; }
        public int Aktuel { get; set; }
        public int Delta { get; set; }
    }

    public class UretimModel
    {
        public int Durum { get; set; }  // 0 : üretiliyor , 1 : Gecikme , 2 : Duruşta , 5 : Üretim Tamamlandı
        public string DurumKod { get; set; } = string.Empty;
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Kod { get; set; } = "";
        public string Barkod { get; set; } = "";
        public int DkHedef { get; set; }
        public int DkHedefG { get; set; }
        public int DkGerçek { get; set; }

        //Ürün
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = "";
        public string Switchgear { get; set; } = "";
        public string Panel_No { get; set; } = "";
        public int Shortc_Time { get; set; }
        public int Discon { get; set; }
        //ÜrünTip
        public string Product { get; set; } = "";
        public float Rated_Volt { get; set; }
        public int Panel_Width { get; set; }
        public string Panel_Type { get; set; } = "";
        public int Panel_Curr { get; set; }
        public float Shortc_Curr { get; set; }
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
    }

    public class OnlineModel
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime Zaman { get; set; } = DateTime.Now;
    }

    public class InputModel
    {
        public string Personel { get; set; } = "";
        public string Rulo { get; set; } = "";
        public string Pano { get; set; } = "";
        public string Durus { get; set; } = "";
    }

    public class DashboardModel
    {
        public Dashboard_Panel_Model Panel { get; set; } = new Dashboard_Panel_Model();
        public List<string[]> TimeLine { get; set; } = new ();
        public int UretimSayisi { get; set; }
        public List<object> Pie { get; set; } = new ();
        public List<object[]> Pie_Personel_Calisma { get; set; } = new ();
        public List<object[]> Pie_Durus { get; set; } = new ();


    }

    public class Dashboard_Panel_Model
    {
        public string Kod { get; set; } = "";
        public int Target { get; set; }
        public int Aktuel { get; set; }
        public int Delta { get; set; }

    }


    public class UretimOlay
    {
        public Guid Id { get; set; }
        public Guid UretimId { get; set; }
        public int Tip { get; set; }
        public string TipKod { get; set; } = "";
        public string Renk { get; set; } = "";
        public string Kod { get; set; } = "";
        public DateTime Baslangic { get; set; }
        public DateTime Bitis { get; set; }
        public string Tooltip { get; set; } = "";
        public string Tooltip_Header { get; set; } = "";
        public string Tooltip_Body { get; set; } = "";
        public string Tooltip_Footer { get; set; } = "";

        public bool IsOverlapping(UretimOlay other)
        {
            return Baslangic <= other.Bitis && other.Baslangic <= Bitis;
        } 
    }

    public class Lojistik
    {
        public string Istasyon { get; set; } = string.Empty;
        public string Wbs { get; set; } = string.Empty;
        public string Panel { get; set; } = string.Empty;
        public int Sira { get; set; }
    }
}
