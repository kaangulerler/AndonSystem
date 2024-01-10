using _01_DbModel.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]
    public class Rapor_DurusModel : PageModel
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
                var data = JsonConvert.DeserializeObject<List<Output_Durus_Tablo>>(List_Excel);
                using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "Duruþ Tipi";
                    ws.Cells[1, 2].Value = "Ýstasyon";
                    ws.Cells[1, 3].Value = "Üretim";
                    ws.Cells[1, 4].Value = "Baþlangýç";
                    ws.Cells[1, 5].Value = "Bitiþ";
                    ws.Cells[1, 6].Value = "Süre";

                    int index = 2;
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            ws.Cells[index, 1].Value = item.Durus;
                            ws.Cells[index, 2].Value = item.Istasyon;
                            ws.Cells[index, 3].Value = item.UretimKod;
                            ws.Cells[index, 4].Value = item.Baslangic.ToString("dd.MM.yyyy HH:mm:ss");
                            ws.Cells[index, 5].Value = item.Bitis.ToString("dd.MM.yyyy HH:mm:ss");

                            string süre = item.Zaman.ToString("c");
                            string[] dizi = süre.Split(".");
                            ws.Cells[index, 6].Value = dizi[dizi.Length - 2];
                            index++;
                        }
                    }

                    for (int i = 1; i < 8; i++)
                        ws.Columns[i].AutoFit();

                    objExcelPackage.Save();
                }
            }

            stream.Position = 0;
            string excelName = "Durus_Raporu_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        public class Output_Durus_Tablo
        {
            public string Durus { get; set; } = String.Empty;
            public string Istasyon { get; set; } = String.Empty;
            public string UretimKod { get; set; } = String.Empty;
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan Zaman { get; set; }
        }

    }
}
