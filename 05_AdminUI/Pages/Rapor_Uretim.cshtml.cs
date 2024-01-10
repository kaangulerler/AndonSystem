using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using OfficeOpenXml;

namespace _05_AdminUI.Pages
{
    [Authorize(Policy = "RaporPolicy")]
    public class Rapor_UretimModel : PageModel
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
                var data = JsonConvert.DeserializeObject<List<Output_Uretim_Tablo>>(List_Excel);
                using (ExcelPackage objExcelPackage = new ExcelPackage(stream))
                {
                    ExcelWorksheet ws = objExcelPackage.Workbook.Worksheets.Add("Sheet1");
                    ws.Cells[1, 1].Value = "�stasyon";
                    ws.Cells[1, 2].Value = "�r�n";
                    ws.Cells[1, 3].Value = "Hedef";
                    ws.Cells[1, 4].Value = "Ba�lang��";
                    ws.Cells[1, 5].Value = "Biti�";
                    ws.Cells[1, 6].Value = "S�re";

                    int index = 2;
                    if (data != null)
                    {
                        foreach (var item in data)
                        {
                            ws.Cells[index, 1].Value = item.Istasyon;
                            ws.Cells[index, 2].Value = item.UretimKod;

                            string hedef = item.SureHedef.ToString("c");
                            ws.Cells[index, 3].Value = hedef; 
                            ws.Cells[index, 4].Value = item.Baslangic.ToString("dd.MM.yyyy HH:mm");
                            ws.Cells[index, 5].Value = item.Bitis.ToString("dd.MM.yyyy HH:mm");

                            string s�re = item.SureGercek.ToString("c");
                            ws.Cells[index, 6].Value = s�re;
                            index++;
                        }
                    }

                    for (int i = 1; i < 8; i++)
                        ws.Columns[i].AutoFit();

                    objExcelPackage.Save();
                }
            }

            stream.Position = 0;
            string excelName = "Uretim_Raporu_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".xlsx";

            return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
        }
        public class Output_Uretim_Tablo
        {
            public Guid Id { get; set; }
            public string Istasyon { get; set; } = String.Empty;
            public string UretimKod { get; set; } = String.Empty;
            public TimeSpan SureHedef { get; set; }
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan SureGercek { get; set; }
            public List<UretimAltItem> ItemList { get; set; } = new();
        }
        public class UretimAltItem
        {
            public Guid UretimId { get; set; }
            public int Tip { get; set; }
            public string Kod { get; set; } = string.Empty;
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan Zaman { get; set; }
        }
    }
}