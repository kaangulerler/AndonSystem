using _01_DbModel;
using _01_DbModel.AppModel;
using _01_DbModel.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Text;

namespace _05_AdminUI.Pages
{

    public class UrunModel : PageModel
    {
        HttpClientHandler clientHandler = new HttpClientHandler();

        [BindProperty]
        public IFormFile? Dosya { get; set; }

        private readonly IConfiguration _configuration;

        public IpAdres? IpAdres { get; set; }
        public string IpAdres_Api = "";

        public class HataUrun
        {
            public int Satýr { get; set; }
            public int Renk { get; set; }
            public string Hata { get; set; } = "";
        }
         
        public UrunModel(IConfiguration iConfig)
        {
            _configuration = iConfig;

            Dictionary<string, object> settings = _configuration.GetSection("IpAdres")
                                                                .Get<Dictionary<string, object>>();
            string json = JsonConvert.SerializeObject(settings);

            IpAdres = JsonConvert.DeserializeObject<IpAdres>(json);

            if (IpAdres != null)
                IpAdres_Api = IpAdres.IpApi;

            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

        }

        public void OnGet()
        {

        }



        public IActionResult OnPost()
        {
            StringBuilder sb = new StringBuilder();
            List<Urun> list_Urun = new List<Urun>();
            List<UrunTip> list_UrunTip = new List<UrunTip>();
            List<T3_Istasyon> list_Istasyon = new List<T3_Istasyon>();
            List<HataUrun> list_Hata = new List<HataUrun>();

            var client = new HttpClient(clientHandler);
            var url = IpAdres_Api + "api/UrunTip/";
            var response = client.GetStringAsync(url).Result;

            var mesaj = JsonConvert.DeserializeObject<XReturn>(response);

            if (mesaj != null)
            {
                var obje = mesaj.Obje.ToString() ?? "[]";
                list_UrunTip = JsonConvert.DeserializeObject<List<UrunTip>>(obje.ToString()) ?? new List<UrunTip>();
            }

            client = new HttpClient(clientHandler);
            url = IpAdres_Api + "api/Urun/";
            response = client.GetStringAsync(url).Result;

            mesaj = JsonConvert.DeserializeObject<XReturn>(response);

            if (mesaj != null)
            {
                var obje = mesaj.Obje.ToString() ?? "[]";
                list_Urun = JsonConvert.DeserializeObject<List<Urun>>(obje.ToString()) ?? new List<Urun>();
            }

            try
            {
                if (Dosya != null)
                {
                    IFormFile file = Dosya;

                    if (file.Length > 0)
                    {
                        bool hataVarmý = false;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            var stream = new MemoryStream();

                            using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                            {
                                ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                                ws.Cells[1, 1].Value = "ProjectId";
                                ws.Cells[1, 2].Value = "ProjectName";
                                ws.Cells[1, 3].Value = "SWITCHGEAR";
                                ws.Cells[1, 4].Value = "PRODUCT";
                                ws.Cells[1, 5].Value = "RATED_VOLT";
                                ws.Cells[1, 6].Value = "PANEL_No.";
                                ws.Cells[1, 7].Value = "PANEL_WIDTH";
                                ws.Cells[1, 8].Value = "PANEL_TYPE";
                                ws.Cells[1, 9].Value = "PANEL_CURR";
                                ws.Cells[1, 10].Value = "SHORTC_CURR";
                                ws.Cells[1, 11].Value = "SHORTC_TIME";
                                ws.Cells[1, 12].Value = "CT";
                                ws.Cells[1, 13].Value = "VT";
                                ws.Cells[1, 14].Value = "VT_WITH";
                                ws.Cells[1, 15].Value = "VT_REM";
                                ws.Cells[1, 16].Value = "VT_FIX";
                                ws.Cells[1, 17].Value = "CT_SEC_CON";
                                ws.Cells[1, 18].Value = "VT_SEC_CON";
                                ws.Cells[1, 19].Value = "ZS2_DISCON";
                                ws.Cells[1, 20].Value = "SA";
                                ws.Cells[1, 21].Value = "CAP_VOLT_IND";
                                ws.Cells[1, 22].Value = "ES_PRESENT";
                                ws.Cells[1, 23].Value = "ES_TYPE_SUBS";
                                ws.Cells[1, 24].Value = "HATA";

                                file.CopyTo(ms);
                                using (ExcelPackage package = new ExcelPackage(ms))
                                {
                                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                                    int colCount = worksheet.Dimension.End.Column;  //get Column Count
                                    int rowCount = worksheet.Dimension.End.Row;     //get row count

                                    if (colCount >= 23)
                                    {
                                        List<Excel_Urun_Satýr> yeniListe = new List<Excel_Urun_Satýr>();

                                        for (int row = 2; row <= rowCount; row++)
                                        {
                                            int satýrDurum = 0; 
                                            int projectid = int.Parse((worksheet.Cells[row, 1].Value?.ToString() ?? "").Replace(".", ","));
                                            string projectname = worksheet.Cells[row, 2].Value?.ToString() ?? "";
                                            string switchgear = worksheet.Cells[row, 3].Value?.ToString() ?? "";
                                            string bomno = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                                            string PRODUCT = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                                            Single RATED_VOLT = Single.Parse((worksheet.Cells[row, 6].Value?.ToString() ?? "").Replace(".", ","));
                                            string panelno = worksheet.Cells[row, 7].Value?.ToString() ?? "";
                                            int PANEL_WIDTH = int.Parse((worksheet.Cells[row, 8].Value?.ToString() ?? "").Replace(".", ","));
                                            string PANEL_TYPE = worksheet.Cells[row, 9].Value?.ToString() ?? "";
                                            int PANEL_CURR = int.Parse((worksheet.Cells[row, 10].Value?.ToString() ?? "").Replace(".", ","));
                                            Single SHORTC_CURR = Single.Parse((worksheet.Cells[row, 11].Value?.ToString() ?? "").Replace(".", ","));
                                            int shotrctime = int.Parse((worksheet.Cells[row, 12].Value?.ToString() ?? "").Replace(".", ","));
                                            int CT = int.Parse((worksheet.Cells[row, 13].Value?.ToString() ?? "").Replace(".", ","));
                                            int VT = int.Parse((worksheet.Cells[row, 14].Value?.ToString() ?? "").Replace(".", ","));
                                            int VT_WITH = int.Parse((worksheet.Cells[row, 15].Value?.ToString() ?? "").Replace(".", ","));
                                            int VT_REM = int.Parse((worksheet.Cells[row, 16].Value?.ToString() ?? "").Replace(".", ","));
                                            int VT_FIX = int.Parse((worksheet.Cells[row, 17].Value?.ToString() ?? "").Replace(".", ","));
                                            int CT_SEC_CON = int.Parse((worksheet.Cells[row, 18].Value?.ToString() ?? "").Replace(".", ","));
                                            int VT_SEC_CON = int.Parse((worksheet.Cells[row, 19].Value?.ToString() ?? "").Replace(".", ","));
                                            int discon = int.Parse((worksheet.Cells[row, 20].Value?.ToString() ?? "").Replace(".", ","));
                                            int SA = int.Parse((worksheet.Cells[row, 21].Value?.ToString() ?? "").Replace(".", ","));
                                            string CAP_VOLT_IND = worksheet.Cells[row, 22].Value?.ToString() ?? "";
                                            string ES_PRESENT = worksheet.Cells[row, 23].Value?.ToString() ?? "";
                                            string ES_TYPE_SUBS = worksheet.Cells[row, 24].Value?.ToString() ?? "";

                                            var item = new T3_UrunTip
                                            {
                                                Id = Guid.NewGuid(),
                                                Product = PRODUCT,
                                                Rated_Volt = RATED_VOLT,
                                                Panel_Width = PANEL_WIDTH,
                                                Panel_Type = PANEL_TYPE,
                                                Panel_Curr = PANEL_CURR,
                                                Shortc_Curr = SHORTC_CURR,
                                                Ct = CT,
                                                Ct_Sec_Con = CT_SEC_CON,
                                                Vt = VT,
                                                Vt_With = VT_WITH,
                                                Vt_Rem = VT_REM,
                                                Vt_Fix = VT_FIX,
                                                Vt_Sec_Con = VT_SEC_CON,
                                                Sa = SA,
                                                Cap_Volt_Ind = CAP_VOLT_IND,
                                                Es_Present = ES_PRESENT,
                                                Es_Type_Subs = ES_TYPE_SUBS,
                                                T3_UrunTipIstasyon = new List<T3_UrunTipIstasyon>(),
                                            };

                                            var urunTipVarmý = list_UrunTip.Where(p => p.Product == item.Product &&
                                                                                        p.Rated_Volt == item.Rated_Volt &&
                                                                                        p.Panel_Width == item.Panel_Width &&
                                                                                        p.Panel_Type == item.Panel_Type &&
                                                                                        p.Panel_Curr == item.Panel_Curr &&
                                                                                        p.Shortc_Curr == item.Shortc_Curr &&
                                                                                        p.Ct == item.Ct &&
                                                                                        p.Vt == item.Vt &&
                                                                                        p.Vt_With == item.Vt_With &&
                                                                                        p.Vt_Rem == item.Vt_Rem &&
                                                                                        p.Vt_Fix == item.Vt_Fix &&
                                                                                        p.Ct_Sec_Con == item.Ct_Sec_Con &&
                                                                                        p.Vt_Sec_Con == item.Vt_Sec_Con &&
                                                                                        p.Sa == item.Sa &&
                                                                                        p.Cap_Volt_Ind == item.Cap_Volt_Ind &&
                                                                                        p.Es_Present == item.Es_Present &&
                                                                                        p.Es_Type_Subs == item.Es_Type_Subs).FirstOrDefault();
                                            if (urunTipVarmý != null)
                                            {


                                                var urunVarmý = list_Urun.Where(p => p.ProjectId == projectid &&
                                                                                    p.ProjectName == projectname &&
                                                                                    p.Switchgear == switchgear &&
                                                                                    p.Panel_No == panelno &&
                                                                                    p.Shortc_Time == shotrctime &&
                                                                                    p.Discon == discon &&
                                                                                    p.TipId == urunTipVarmý.Id)
                                                                         .FirstOrDefault();

                                                if (urunVarmý == null)
                                                {
                                                    var urun = new Urun
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        ProjectId = projectid,
                                                        ProjectName = projectname,
                                                        BomNo = bomno,
                                                        Switchgear = switchgear,
                                                        Panel_No = panelno,
                                                        Shortc_Time = shotrctime,
                                                        Discon = discon,
                                                        Kod = "",
                                                        TipId = urunTipVarmý.Id,
                                                        Tip = urunTipVarmý
                                                    };

                                                    list_Urun.Add(urun);
                                                }
                                                else
                                                {
                                                    satýrDurum = 1;
                                                }
                                            }
                                            else
                                            {
                                                satýrDurum = 2;
                                            }

                                            string hataYazý = "";

                                            switch (satýrDurum)
                                            {
                                                case 1:
                                                    {
                                                        hataYazý = "Ürün daha önce kayýt edilmiþ.";
                                                        break;
                                                    }
                                                case 2:
                                                    {
                                                        hataYazý = "Ürün tipi bulunamadý.";
                                                        break;
                                                    }
                                            }


                                            yeniListe.Add(new()
                                            {
                                                ProjectId = projectid.ToString(),
                                                ProjectName = projectname,
                                                Switchgear = switchgear,
                                                BomNo = bomno,
                                                Product = PRODUCT,
                                                Rated_Volt = RATED_VOLT.ToString().Replace(",", "."),
                                                Panel_No = panelno,
                                                Panel_Width = PANEL_WIDTH.ToString(),
                                                Panel_Type = PANEL_TYPE,
                                                Panel_Curr = PANEL_CURR.ToString(),
                                                Shortc_Curr = SHORTC_CURR.ToString().Replace(",", "."),
                                                Shortc_Time = shotrctime.ToString(),
                                                Ct = CT.ToString(),
                                                Vt = VT.ToString(),
                                                Vt_With = VT_WITH.ToString(),
                                                Vt_Rem = VT_REM.ToString(),
                                                Vt_Fix = VT_FIX.ToString(),
                                                Ct_Sec_Con = CT_SEC_CON.ToString(),
                                                Vt_Sec_Con = VT_SEC_CON.ToString(),
                                                Discon = discon.ToString(),
                                                Sa = SA.ToString(),
                                                Cap_Volt_Ind = CAP_VOLT_IND.ToString(),
                                                Es_Present = ES_PRESENT.ToString(),
                                                Es_Type_Subs = ES_TYPE_SUBS.ToString(),
                                                Hata = hataYazý,

                                            });

                                        }

                                        yeniListe = yeniListe.OrderByDescending(p => p.Hata)
                                                             .ThenBy(p => p.Product)
                                                             .ThenBy(p => p.ProjectId)
                                                             .ThenBy(p => p.Switchgear)
                                                             .ThenBy(p => p.Panel_No)
                                                             .ToList();

                                        for (int row = 0; row < yeniListe.Count; row++)
                                        {
                                            var x = yeniListe[row];

                                            ws.Cells[row + 2, 1].Value = x.ProjectId;
                                            ws.Cells[row + 2, 2].Value = x.ProjectName;
                                            ws.Cells[row + 2, 3].Value = x.Switchgear;
                                            ws.Cells[row + 2, 4].Value = x.BomNo;
                                            ws.Cells[row + 2, 5].Value = x.Product;
                                            ws.Cells[row + 2, 6].Value = x.Rated_Volt;
                                            ws.Cells[row + 2, 7].Value = x.Panel_No;
                                            ws.Cells[row + 2, 8].Value = x.Panel_Width;
                                            ws.Cells[row + 2, 9].Value = x.Panel_Type;
                                            ws.Cells[row + 2, 10].Value = x.Panel_Curr;
                                            ws.Cells[row + 2, 11].Value = x.Shortc_Curr;
                                            ws.Cells[row + 2, 12].Value = x.Shortc_Time;
                                            ws.Cells[row + 2, 13].Value = x.Ct;
                                            ws.Cells[row + 2, 14].Value = x.Vt;
                                            ws.Cells[row + 2, 15].Value = x.Vt_With;
                                            ws.Cells[row + 2, 16].Value = x.Vt_Rem;
                                            ws.Cells[row + 2, 17].Value = x.Vt_Fix;
                                            ws.Cells[row + 2, 18].Value = x.Ct_Sec_Con;
                                            ws.Cells[row + 2, 19].Value = x.Vt_Sec_Con;
                                            ws.Cells[row + 2, 20].Value = x.Discon;
                                            ws.Cells[row + 2, 21].Value = x.Sa;
                                            ws.Cells[row + 2, 22].Value = x.Cap_Volt_Ind;
                                            ws.Cells[row + 2, 23].Value = x.Es_Present;
                                            ws.Cells[row + 2, 24].Value = x.Es_Type_Subs;
                                            ws.Cells[row + 2, 25].Value = x.Hata;

                                            Color back = Color.White;
                                            Color text = Color.Black;
                                             
                                            switch (x.Hata)
                                            {
                                                case "Ürün daha önce kayýt edilmiþ.":
                                                    {
                                                        hataVarmý = true;
                                                        back = Color.Yellow;
                                                        text = Color.Black;
                                                        break;
                                                    }
                                                case "Ürün tipi bulunamadý.":
                                                    {
                                                        hataVarmý = true;
                                                        back = Color.Red;
                                                        text = Color.White;
                                                        break;
                                                    }
                                            }

                                            ws.Rows[row + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                            ws.Rows[row + 2].Style.Fill.BackgroundColor.SetColor(back);
                                            ws.Rows[row + 2].Style.Font.Color.SetColor(text);

                                        }
                                    }
                                }
                                for (int i = 1; i < 25; i++)
                                    ws.Columns[i].AutoFit();
                                objExcelPackage.Save();
                            }

                            if (!hataVarmý)
                            {
                                client = new HttpClient(clientHandler);
                                url = IpAdres_Api + "api/Urun/";

                                var gelen = client.PostAsJsonAsync(url, list_Urun).Result;
                                mesaj = gelen.Content.ReadFromJsonAsync<XReturn>().Result;

                                if (mesaj != null)
                                {
                                    if (mesaj.Islem)
                                    {
                                        return Page();
                                    }
                                }
                            }
                            else
                            {
                                stream.Position = 0;
                                string excelName = "Hata_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

                                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
                            }
                        }
                    }
                }
            }
            catch  
            {
                return RedirectToPage("Hata");
            }

            return Page();
        }

        public IActionResult OnPostIndir()
        {
            StringBuilder sb = new StringBuilder();

            try
            {
                DateTime baþ = DateTime.Now;

                List<Urun> list_Urun = new List<Urun>();

                var client = new HttpClient(clientHandler);
                var url = IpAdres_Api + "api/Urun/";
                var response = client.GetStringAsync(url).Result;

                var mesaj = JsonConvert.DeserializeObject<XReturn>(response);

                if (mesaj != null)
                {
                    var obje = mesaj.Obje.ToString() ?? "[]";
                    list_Urun = JsonConvert.DeserializeObject<List<Urun>>(obje.ToString()) ?? new List<Urun>();
                }

                var stream = new MemoryStream();

                using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "ProjectId";
                    ws.Cells[1, 2].Value = "ProjectName";
                    ws.Cells[1, 3].Value = "SWITCHGEAR";
                    ws.Cells[1, 4].Value = "PRODUCT";
                    ws.Cells[1, 5].Value = "RATED_VOLT";
                    ws.Cells[1, 6].Value = "PANEL_No.";
                    ws.Cells[1, 7].Value = "PANEL_WIDTH";
                    ws.Cells[1, 8].Value = "PANEL_TYPE";
                    ws.Cells[1, 9].Value = "PANEL_CURR";
                    ws.Cells[1, 10].Value = "SHORTC_CURR";
                    ws.Cells[1, 11].Value = "SHORTC_TIME";
                    ws.Cells[1, 12].Value = "CT";
                    ws.Cells[1, 13].Value = "VT";
                    ws.Cells[1, 14].Value = "VT_WITH";
                    ws.Cells[1, 15].Value = "VT_REM";
                    ws.Cells[1, 16].Value = "VT_FIX";
                    ws.Cells[1, 17].Value = "CT_SEC_CON";
                    ws.Cells[1, 18].Value = "VT_SEC_CON";
                    ws.Cells[1, 19].Value = "ZS2_DISCON";
                    ws.Cells[1, 20].Value = "SA";
                    ws.Cells[1, 21].Value = "CAP_VOLT_IND";
                    ws.Cells[1, 22].Value = "ES_PRESENT";
                    ws.Cells[1, 23].Value = "ES_TYPE_SUBS";

                    int index = 2;
                    foreach (var item in list_Urun)
                    {
                        ws.Cells[index, 1].Value = item.ProjectId;
                        ws.Cells[index, 2].Value = item.ProjectName;
                        ws.Cells[index, 3].Value = item.Switchgear;
                        ws.Cells[index, 4].Value = item.Tip.Product;
                        ws.Cells[index, 5].Value = item.Tip.Rated_Volt.ToString().Replace(",", ".");
                        ws.Cells[index, 6].Value = item.Panel_No;
                        ws.Cells[index, 7].Value = item.Tip.Panel_Width;
                        ws.Cells[index, 8].Value = item.Tip.Panel_Type;
                        ws.Cells[index, 9].Value = item.Tip.Panel_Curr;
                        ws.Cells[index, 10].Value = item.Tip.Shortc_Curr.ToString().Replace(",", ".");
                        ws.Cells[index, 11].Value = item.Shortc_Time;
                        ws.Cells[index, 12].Value = item.Tip.Ct;
                        ws.Cells[index, 13].Value = item.Tip.Vt;
                        ws.Cells[index, 14].Value = item.Tip.Vt_With;
                        ws.Cells[index, 15].Value = item.Tip.Vt_Rem;
                        ws.Cells[index, 16].Value = item.Tip.Vt_Fix;
                        ws.Cells[index, 17].Value = item.Tip.Ct_Sec_Con;
                        ws.Cells[index, 18].Value = item.Tip.Vt_Sec_Con;
                        ws.Cells[index, 19].Value = item.Discon;
                        ws.Cells[index, 20].Value = item.Tip.Sa;
                        ws.Cells[index, 21].Value = item.Tip.Cap_Volt_Ind;
                        ws.Cells[index, 22].Value = item.Tip.Es_Present;
                        ws.Cells[index, 23].Value = item.Tip.Es_Type_Subs;
                        index++;
                    }

                    for (int i = 1; i < 24; i++)
                        ws.Columns[i].AutoFit();

                    objExcelPackage.Save();
                }

                stream.Position = 0;
                string excelName = "Data_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

                var fark = (DateTime.Now - baþ).TotalMilliseconds;
                //return File(stream, "application/octet-stream", excelName);  
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

            }
            catch
            {
                return RedirectToPage("Hata");
            }

        }

        public IActionResult OnDelete()
        {
            StringBuilder sb = new StringBuilder();
            List<Urun> list_Urun = new List<Urun>();
            List<UrunTip> list_UrunTip = new List<UrunTip>();
            List<T3_Istasyon> list_Istasyon = new List<T3_Istasyon>();
            List<HataUrun> list_Hata = new List<HataUrun>();

            var client = new HttpClient(clientHandler);
            var url = IpAdres_Api + "api/UrunTip/";
            var response = client.DeleteAsync(url).Result;


            return Page();
        }
    }
}
