using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]
    public class Rapor_OperatorUretimModel : PageModel
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
                var data = JsonConvert.DeserializeObject<List<Output_Operator_Tablo>>(List_Excel);
                using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "Personel";
                    ws.Cells[1, 2].Value = "Ýstasyon";
                    ws.Cells[1, 3].Value = "ProjectId";
                    ws.Cells[1, 4].Value = "Ürün";
                    ws.Cells[1, 5].Value = "Baþlangýç";
                    ws.Cells[1, 6].Value = "Bitiþ";
                    ws.Cells[1, 7].Value = "Süre";

                    int index = 2;
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            ws.Cells[index, 1].Value = item.Personel;
                            ws.Cells[index, 2].Value = item.Istasyon;
                            ws.Cells[index, 3].Value = item.ProjectId;
                            ws.Cells[index, 4].Value = item.UretimKod;
                            ws.Cells[index, 5].Value = item.Baslangic.ToString("dd.MM.yyyy HH:mm:ss"); ;
                            ws.Cells[index, 6].Value = item.Bitis.ToString("dd.MM.yyyy HH:mm:ss"); ;
                            ws.Cells[index, 7].Value = item.Zaman.Hours.ToString("D2") + ":" + item.Zaman.Minutes.ToString("D2") + ":" + item.Zaman.Seconds.ToString("D2");

                            index++;
                        }
                    }

                    for (int i = 1; i < 8; i++)
                        ws.Columns[i].AutoFit();

                    objExcelPackage.Save();
                }
            }

            stream.Position = 0;
            string excelName = "Operator_Uretim_Raporu_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }

        public class Output_Operator_Tablo
        {
            public Guid Id { get; set; }
            public string Personel { get; set; } = String.Empty;
            public string Istasyon { get; set; } = String.Empty;
            public int ProjectId { get; set; }
            public string UretimKod { get; set; } = String.Empty;
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan Zaman { get; set; }
        }

    }
}
