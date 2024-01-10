using _01_DbModel.Db;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]
    public class Rapor_VardiyaModel : PageModel
    {

        [BindProperty]
        public string List_Calisma { get; set; } = "";

        public void OnGet()
        {
        }

        public IActionResult OnPostIndir()
        { 
            var stream = new MemoryStream();

            if (List_Calisma != null)
            {
                var data = JsonConvert.DeserializeObject<List<T3_Calisma>>(List_Calisma);
                using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "Ýstasyon";
                    ws.Cells[1, 2].Value = "Vardiya";
                    ws.Cells[1, 3].Value = "Baþlangýç";
                    ws.Cells[1, 4].Value = "Bitiþ";
                    ws.Cells[1, 5].Value = "Target";
                    ws.Cells[1, 6].Value = "Actuel";
                    ws.Cells[1, 7].Value = "Delta";

                    int index = 2;
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            ws.Cells[index, 1].Value = item.Istasyon.Kod;
                            ws.Cells[index, 2].Value = item.Kod;
                            ws.Cells[index, 3].Value = item.Baslangic.ToString("dd.MM.yyyy");
                            ws.Cells[index, 4].Value = item.Bitis.ToString("dd.MM.yyyy");
                            ws.Cells[index, 5].Value = item.Hedef;
                            ws.Cells[index, 6].Value = item.Aktuel;
                            ws.Cells[index, 7].Value = item.Delta;
                            index++;
                        }
                    }

                    for (int i = 1; i < 8; i++)
                        ws.Columns[i].AutoFit();

                    objExcelPackage.Save();
                }
            }

            stream.Position = 0;
            string excelName = "Vardiya_Raporu_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";
 
            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
    }
}
