using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]
    public class Rapor_KullanabilirlikModel : PageModel
    {
        [BindProperty]
        public string List_Excel { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPostIndir()
        {
            var stream = new MemoryStream();

            if (List_Excel != null)
            {
                var data = JsonConvert.DeserializeObject<List<Output_Kull_Tablo>>(List_Excel);
                using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "Tarih";
                    ws.Cells[1, 2].Value = "Ýstasyon";
                    ws.Cells[1, 3].Value = "Planlanan Çalýþma Süresi";
                    ws.Cells[1, 4].Value = "Toplam Üretim Süresi";
                    ws.Cells[1, 5].Value = "Kullanýlabilirlik";

                    int index = 2;
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            ws.Cells[index, 1].Value = item.Tarih;
                            ws.Cells[index, 2].Value = item.Istasyon;
                            ws.Cells[index, 3].Value = item.Planlanan.Hours.ToString("D2") + ":" + item.Planlanan.Minutes.ToString("D2") + ":" + item.Planlanan.Seconds.ToString("D2");
                            ws.Cells[index, 4].Value = item.UretimToplam.Hours.ToString("D2") + ":" + item.UretimToplam.Minutes.ToString("D2") + ":" + item.UretimToplam.Seconds.ToString("D2");
                            ws.Cells[index, 5].Value = item.Kullanýlabilirlik;
                             
                            index++;
                        }
                    }

                    for (int i = 1; i < 8; i++)
                        ws.Columns[i].AutoFit();

                    objExcelPackage.Save();
                }
            }

            stream.Position = 0;
            string excelName = "Kullanýlabilirlik_Raporu_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public class Output_Kull_Tablo
        {
            public Guid Id { get; set; }
            public string Tarih { get; set; } = String.Empty;
            public string Istasyon { get; set; } = String.Empty;
            public TimeSpan Planlanan { get; set; }
            public TimeSpan UretimToplam { get; set; }
            public double Kullanýlabilirlik { get; set; }
        }

    }
}
