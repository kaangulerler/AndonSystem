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
    [Authorize(Policy = "UrunTipPolicy")]
    public class UrunTipModel : PageModel
    {
        HttpClientHandler clientHandler = new HttpClientHandler();

        [BindProperty]
        public IFormFile? Dosya { get; set; }

        private readonly IConfiguration _configuration;

        public IpAdres? IpAdres { get; set; }
        public string IpAdres_Api = "";

        public UrunTipModel(IConfiguration iConfig)
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
            StringBuilder sb = new();

            List<T3_Istasyon> list_Istasyon = new();

            var client = new HttpClient(clientHandler);
            var url = IpAdres_Api + "api/Istasyon/";
            var response = client.GetStringAsync(url).Result;

            var mesaj = JsonConvert.DeserializeObject<XReturn>(response);

            if (mesaj != null)
            {
                var obje = mesaj.Obje.ToString() ?? "[]";
                list_Istasyon = JsonConvert.DeserializeObject<List<T3_Istasyon>>(obje.ToString()) ?? new List<T3_Istasyon>();
            }

            List<UrunTip> list = new();

            try
            {
                if (Dosya != null)
                {
                    IFormFile file = Dosya;

                    if (file.Length > 0)
                    {
                        MemoryStream ms = new();
                        file.CopyTo(ms);
                        ExcelPackage package = new(ms);

                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int colCount = worksheet.Dimension.End.Column;  //get Column Count
                        int rowCount = worksheet.Dimension.End.Row;     //get row count

                        if (colCount > 17)
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                string PRODUCT = worksheet.Cells[row, 1].Value?.ToString() ?? "";
                                float RATED_VOLT = float.Parse((worksheet.Cells[row, 2].Value?.ToString() ?? "").Replace(".", ","));
                                int PANEL_WIDTH = int.Parse((worksheet.Cells[row, 3].Value?.ToString() ?? "").Replace(".", ","));
                                string PANEL_TYPE = worksheet.Cells[row, 4].Value?.ToString() ?? "";
                                int PANEL_CURR = int.Parse((worksheet.Cells[row, 5].Value?.ToString() ?? "").Replace(".", ","));
                                float SHORTC_CURR = float.Parse((worksheet.Cells[row, 6].Value?.ToString() ?? "").Replace(".", ","));
                                int CT = int.Parse((worksheet.Cells[row, 7].Value?.ToString() ?? "").Replace(".", ","));
                                int VT = int.Parse((worksheet.Cells[row, 8].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_WITH = int.Parse((worksheet.Cells[row, 9].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_REM = int.Parse((worksheet.Cells[row, 10].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_FIX = int.Parse((worksheet.Cells[row, 11].Value?.ToString() ?? "").Replace(".", ","));
                                int CT_SEC_CON = int.Parse((worksheet.Cells[row, 12].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_SEC_CON = int.Parse((worksheet.Cells[row, 13].Value?.ToString() ?? "").Replace(".", ","));
                                int SA = int.Parse((worksheet.Cells[row, 14].Value?.ToString() ?? "").Replace(".", ","));
                                string CAP_VOLT_IND = worksheet.Cells[row, 15].Value?.ToString() ?? "";
                                string ES_PRESENT = worksheet.Cells[row, 16].Value?.ToString() ?? "";
                                string ES_TYPE_SUBS = worksheet.Cells[row, 17].Value?.ToString() ?? "";

                                var item = new UrunTip
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
                                    Liste = new List<UrunTipIstasyon>()
                                };

                                var mevcutta_Varmý = list.Where(p => p.Product == item.Product &&
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

                                if (mevcutta_Varmý == null)
                                {
                                    for (int col = 18; col <= colCount; col++)
                                    {
                                        string istasyonKod = worksheet.Cells[1, col].Value?.ToString() ?? "";
                                        var istasyon = list_Istasyon.Where(p => p.Kod == istasyonKod)
                                                                    .FirstOrDefault();
                                        if (istasyon != null)
                                        {
                                            float dk = int.Parse(worksheet.Cells[row, col].Value?.ToString() ?? "0");

                                            if (istasyon.Id == Guid.Parse("00000000-0000-0000-0000-000000000104"))
                                            {
                                                if (item.Ct > 0)
                                                {
                                                    dk = 30 + (item.Ct * item.Ct_Sec_Con * 1.25F);
                                                }

                                                if (item.Vt > 0)
                                                {
                                                    dk += (15 + (item.Vt * item.Vt_Sec_Con * 1.25F));
                                                }
                                            }

                                            if (istasyon.Id == Guid.Parse("00000000-0000-0000-0000-000000000108"))
                                            {
                                                dk = ((item.Ct * item.Ct_Sec_Con) + (item.Vt * item.Vt_Sec_Con)) * 2.5F;
                                            }


                                            if (dk > 0)
                                            {
                                                item.Liste.Add(new UrunTipIstasyon
                                                {
                                                    Id = Guid.NewGuid(),
                                                    IstasyonId = istasyon.Id,
                                                    TipId = item.Id,
                                                    Zaman = dk,
                                                    Istasyon = "",
                                                });
                                            }
                                        }
                                    }
                                    list.Add(item);
                                }
                            }
                        }

                        url = IpAdres_Api + "api/UrunTip/";

                        var gelen = client.PostAsJsonAsync(url, list).Result;
                        mesaj = gelen.Content.ReadFromJsonAsync<XReturn>().Result;

                    }
                }
            }
            catch
            {
                return RedirectToPage("Hata");
            }

            return Page();
        }

        public IActionResult OnPostCevir()
        {
            List<T3_UrunTip> list_Mevcut = new();
            List<T3_UrunTip> list = new();

            StringBuilder sb = new();

            var client = new HttpClient(clientHandler);
            var url = IpAdres_Api + "api/UrunTip/";
            var response = client.GetStringAsync(url).Result;

            var mesaj = JsonConvert.DeserializeObject<XReturn>(response);

            if (mesaj != null)
            {
                var obje = mesaj.Obje.ToString() ?? "[]";
                list_Mevcut = JsonConvert.DeserializeObject<List<T3_UrunTip>>(obje.ToString()) ?? new List<T3_UrunTip>();
            }

            try
            {
                if (Dosya != null)
                {
                    DateTime baþ = DateTime.Now;
                    IFormFile file = Dosya;

                    if (file.Length > 0)
                    {
                        MemoryStream ms = new();
                        file.CopyTo(ms);
                        ExcelPackage package = new(ms);
                        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                        int colCount = worksheet.Dimension.End.Column;  //get Column Count
                        int rowCount = worksheet.Dimension.End.Row;     //get row count

                        if (colCount == 24)
                        {
                            for (int row = 2; row <= rowCount; row++)
                            {
                                string PRODUCT = worksheet.Cells[row, 5].Value?.ToString() ?? "";
                                float RATED_VOLT = float.Parse((worksheet.Cells[row, 6].Value?.ToString() ?? "").Replace(".", ","));
                                int PANEL_WIDTH = int.Parse((worksheet.Cells[row, 8].Value?.ToString() ?? "").Replace(".", ","));
                                string PANEL_TYPE = worksheet.Cells[row, 9].Value?.ToString() ?? "";
                                int PANEL_CURR = int.Parse((worksheet.Cells[row, 10].Value?.ToString() ?? "").Replace(".", ","));
                                float SHORTC_CURR = float.Parse((worksheet.Cells[row, 11].Value?.ToString() ?? "").Replace(".", ","));
                                int CT = int.Parse((worksheet.Cells[row, 13].Value?.ToString() ?? "").Replace(".", ","));
                                int VT = int.Parse((worksheet.Cells[row, 14].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_WITH = int.Parse((worksheet.Cells[row, 15].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_REM = int.Parse((worksheet.Cells[row, 16].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_FIX = int.Parse((worksheet.Cells[row, 17].Value?.ToString() ?? "").Replace(".", ","));
                                int CT_SEC_CON = int.Parse((worksheet.Cells[row, 18].Value?.ToString() ?? "").Replace(".", ","));
                                int VT_SEC_CON = int.Parse((worksheet.Cells[row, 19].Value?.ToString() ?? "").Replace(".", ","));
                                int SA = int.Parse((worksheet.Cells[row, 21].Value?.ToString() ?? "").Replace(".", ","));
                                string CAP_VOLT_IND = worksheet.Cells[row, 22].Value?.ToString() ?? "";
                                string ES_PRESENT = worksheet.Cells[row, 23].Value?.ToString() ?? "";
                                string ES_TYPE_SUBS = worksheet.Cells[row, 24].Value?.ToString() ?? "";

                                var item = new T3_UrunTip
                                {
                                    Product = PRODUCT,
                                    Rated_Volt = RATED_VOLT,
                                    Panel_Width = PANEL_WIDTH,
                                    Panel_Type = PANEL_TYPE,
                                    Panel_Curr = PANEL_CURR,
                                    Shortc_Curr = SHORTC_CURR,
                                    Ct = CT,
                                    Vt = VT,
                                    Vt_With = VT_WITH,
                                    Vt_Rem = VT_REM,
                                    Vt_Fix = VT_FIX,
                                    Ct_Sec_Con = CT_SEC_CON,
                                    Vt_Sec_Con = VT_SEC_CON,
                                    Sa = SA,
                                    Cap_Volt_Ind = CAP_VOLT_IND,
                                    Es_Present = ES_PRESENT,
                                    Es_Type_Subs = ES_TYPE_SUBS,
                                };

                                var mevcutta_Varmý = list_Mevcut.Where(p => p.Product == item.Product &&
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

                                if (mevcutta_Varmý == null)
                                {
                                    var listede_Varmý = list.Where(p => p.Product == item.Product &&
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
                                    if (listede_Varmý == null)
                                        list.Add(item);
                                }
                                else
                                    list.Add(mevcutta_Varmý);
                            }
                        }
                    }

                    List<T3_Istasyon> list_Istasyon = new();

                    client = new HttpClient(clientHandler);
                    url = IpAdres_Api + "api/Istasyon/";
                    response = client.GetStringAsync(url).Result;

                    mesaj = JsonConvert.DeserializeObject<XReturn>(response);

                    if (mesaj != null)
                    {
                        var obje = mesaj.Obje.ToString() ?? "[]";
                        list_Istasyon = JsonConvert.DeserializeObject<List<T3_Istasyon>>(obje.ToString()) ?? new List<T3_Istasyon>();
                        list_Istasyon = list_Istasyon.Where(p => !p.Kod.Contains("MWS")).ToList();
                    }

                    var stream = new MemoryStream();

                    using (ExcelPackage objExcelPackage = new(stream))
                    {
                        ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                        ws.Cells[1, 1].Value = "PRODUCT";
                        ws.Cells[1, 2].Value = "RATED_VOLT";
                        ws.Cells[1, 3].Value = "PANEL_WIDTH";
                        ws.Cells[1, 4].Value = "PANEL_TYPE";
                        ws.Cells[1, 5].Value = "PANEL_CURR";
                        ws.Cells[1, 6].Value = "SHORTC_CURR";
                        ws.Cells[1, 7].Value = "CT";
                        ws.Cells[1, 8].Value = "VT";
                        ws.Cells[1, 9].Value = "VT_WITH";
                        ws.Cells[1, 10].Value = "VT_REM";
                        ws.Cells[1, 11].Value = "VT_FIX";
                        ws.Cells[1, 12].Value = "CT_SEC_CON";
                        ws.Cells[1, 13].Value = "VT_SEC_CON";
                        ws.Cells[1, 14].Value = "SA";
                        ws.Cells[1, 15].Value = "CAP_VOLT_IND";
                        ws.Cells[1, 16].Value = "ES_PRESENT";
                        ws.Cells[1, 17].Value = "ES_TYPE_SUBS";

                        int index = 18;
                        string[] dizi_Istasyon = new string[list_Istasyon.Count];

                        foreach (var ist in list_Istasyon)
                        {
                            dizi_Istasyon[index - 18] = ist.Kod ?? "";
                            ws.Cells[1, index].Value = ist.Kod;
                            ws.Cells[1, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[1, index].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                            index++;
                        }

                        index = 2;

                        list = list.OrderBy(p => p.Product).ThenBy(p => p.Rated_Volt).ThenBy(p => p.Panel_Type).ToList();

                        foreach (var item in list)
                        {
                            ws.Cells[index, 1].Value = item.Product;
                            ws.Cells[index, 2].Value = Math.Round(item.Rated_Volt, 2);
                            ws.Cells[index, 3].Value = item.Panel_Width;
                            ws.Cells[index, 4].Value = item.Panel_Type;
                            ws.Cells[index, 5].Value = item.Panel_Curr;
                            ws.Cells[index, 6].Value = Math.Round(item.Shortc_Curr, 2);
                            ws.Cells[index, 7].Value = item.Ct;
                            ws.Cells[index, 8].Value = item.Vt;
                            ws.Cells[index, 9].Value = item.Vt_With;
                            ws.Cells[index, 10].Value = item.Vt_Rem;
                            ws.Cells[index, 11].Value = item.Vt_Fix;
                            ws.Cells[index, 12].Value = item.Ct_Sec_Con;
                            ws.Cells[index, 13].Value = item.Vt_Sec_Con;
                            ws.Cells[index, 14].Value = item.Sa;
                            ws.Cells[index, 15].Value = item.Cap_Volt_Ind;
                            ws.Cells[index, 16].Value = item.Es_Present;
                            ws.Cells[index, 17].Value = item.Es_Type_Subs;

                            //Önce sýfýrla
                            for (int i = 0; i < dizi_Istasyon.Length; i++)
                                ws.Cells[index, i + 18].Value = 0;

                            foreach (var ist in item.T3_UrunTipIstasyon)
                            {
                                if (ist.Istasyon != null)
                                {
                                    int sütunBul = Array.IndexOf(dizi_Istasyon, ist.Istasyon.Kod);
                                    ws.Cells[index, sütunBul + 18].Value = ist.Zaman;
                                }
                            }

                            index++;
                        }

                        for (int i = 1; i < dizi_Istasyon.Length + 18; i++)
                            ws.Columns[i].AutoFit();

                        objExcelPackage.Save();
                    }

                    stream.Position = 0;
                    string excelName = "Data_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

                    var fark = (DateTime.Now - baþ).TotalMilliseconds;
                    //return File(stream, "application/octet-stream", excelName);  
                    return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);

                }
            }
            catch
            {
                return RedirectToPage("Hata");
            }

            return this.Content(sb.ToString());
        }

        public IActionResult OnPostIndir()
        {
            List<T3_UrunTip> list = new();

            StringBuilder sb = new();

            try
            {
                DateTime baþ = DateTime.Now;

                List<T3_Istasyon> list_Istasyon = new();

                var client = new HttpClient(clientHandler);
                var url = IpAdres_Api + "api/Istasyon/";
                var response = client.GetStringAsync(url).Result;

                var mesaj = JsonConvert.DeserializeObject<XReturn>(response);

                if (mesaj != null)
                {
                    var obje = mesaj.Obje.ToString() ?? "[]";
                    list_Istasyon = JsonConvert.DeserializeObject<List<T3_Istasyon>>(obje.ToString()) ?? new List<T3_Istasyon>();
                    list_Istasyon = list_Istasyon.Where(p => p.LokasyonId != Guid.Parse("00000000-0000-0000-0000-000000000003"))
                                                 .ToList();
                }


                url = IpAdres_Api + "api/UrunTip/";
                response = client.GetStringAsync(url).Result;

                mesaj = JsonConvert.DeserializeObject<XReturn>(response);

                if (mesaj != null)
                {
                    var obje = mesaj.Obje.ToString() ?? "[]";
                    list = JsonConvert.DeserializeObject<List<T3_UrunTip>>(obje.ToString()) ?? new List<T3_UrunTip>();
                }

                var stream = new MemoryStream();

                using (ExcelPackage objExcelPackage = new(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "PRODUCT";
                    ws.Cells[1, 2].Value = "RATED_VOLT";
                    ws.Cells[1, 3].Value = "PANEL_WIDTH";
                    ws.Cells[1, 4].Value = "PANEL_TYPE";
                    ws.Cells[1, 5].Value = "PANEL_CURR";
                    ws.Cells[1, 6].Value = "SHORTC_CURR";
                    ws.Cells[1, 7].Value = "CT";
                    ws.Cells[1, 8].Value = "VT";
                    ws.Cells[1, 9].Value = "VT_WITH";
                    ws.Cells[1, 10].Value = "VT_REM";
                    ws.Cells[1, 11].Value = "VT_FIX";
                    ws.Cells[1, 12].Value = "CT_SEC_CON";
                    ws.Cells[1, 13].Value = "VT_SEC_CON";
                    ws.Cells[1, 14].Value = "SA";
                    ws.Cells[1, 15].Value = "CAP_VOLT_IND";
                    ws.Cells[1, 16].Value = "ES_PRESENT";
                    ws.Cells[1, 17].Value = "ES_TYPE_SUBS";

                    int index = 18;
                    string[] dizi_Istasyon = new string[list_Istasyon.Count];

                    foreach (var ist in list_Istasyon)
                    {
                        dizi_Istasyon[index - 18] = ist.Kod ?? "";
                        ws.Cells[1, index].Value = ist.Kod;
                        ws.Cells[1, index].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[1, index].Style.Fill.BackgroundColor.SetColor(Color.Yellow);
                        index++;
                    }

                    index = 2;

                    foreach (var item in list)
                    {
                        ws.Cells[index, 1].Value = item.Product;
                        ws.Cells[index, 2].Value = Math.Round(item.Rated_Volt, 2);
                        ws.Cells[index, 3].Value = item.Panel_Width;
                        ws.Cells[index, 4].Value = item.Panel_Type;
                        ws.Cells[index, 5].Value = item.Panel_Curr;
                        ws.Cells[index, 6].Value = Math.Round(item.Shortc_Curr, 2);
                        ws.Cells[index, 7].Value = item.Ct;
                        ws.Cells[index, 8].Value = item.Vt;
                        ws.Cells[index, 9].Value = item.Vt_With;
                        ws.Cells[index, 10].Value = item.Vt_Rem;
                        ws.Cells[index, 11].Value = item.Vt_Fix;
                        ws.Cells[index, 12].Value = item.Ct_Sec_Con;
                        ws.Cells[index, 13].Value = item.Vt_Sec_Con;
                        ws.Cells[index, 14].Value = item.Sa;
                        ws.Cells[index, 15].Value = item.Cap_Volt_Ind;
                        ws.Cells[index, 16].Value = item.Es_Present;
                        ws.Cells[index, 17].Value = item.Es_Type_Subs;

                        //Önce sýfýrla
                        for (int i = 0; i < dizi_Istasyon.Length; i++)
                            ws.Cells[index, i + 18].Value = 0;

                        foreach (var ist in item.T3_UrunTipIstasyon)
                        {
                            if (ist.Istasyon != null)
                            {
                                int sütunBul = Array.IndexOf(dizi_Istasyon, ist.Istasyon.Kod);
                                ws.Cells[index, sütunBul + 18].Value = ist.Zaman;
                            }
                        }

                        index++;
                    }

                    for (int i = 1; i < dizi_Istasyon.Length + 18; i++)
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

    }
}
