using _01_DbModel;
using _01_DbModel.AppModel;
using _01_DbModel.Db;
using _02_Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _02_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UrunController : ControllerBase
    {
        private readonly ILogger<UrunController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx04";

        public UrunController(XModel context, ILogger<UrunController> logger)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        public async Task<XReturn> Get()
        {
            string fonksiyon = _fonksiyon + "01";

            XReturn x = new()
            {
                Islem = false,
                Fonksiyon = fonksiyon,
                Kod = "İşlem Başarısız",
                Mesaj = "Hata oluştu.",
                Obje = "",
            };

            try
            {
                var zaman = DateTime.Now;

                var obje = await db.T3_Urun.Where(p=>p.Id != new Guid()).Include(p => p.Tip)
                                           .ThenInclude(p => p.T3_UrunTipIstasyon)
                                           .ThenInclude(p => p.Istasyon)
                                           .OrderBy(p=>p.ProjectId)
                                           .ThenBy(p=>p.Switchgear)
                                           .ThenBy(p=>p.Panel_No)
                                           .ToListAsync();

                var kontrol = (DateTime.Now - zaman).TotalMilliseconds;

                List<Urun> list = new List<Urun>();

                foreach (var urun in obje)
                {
                    var item = urun.Tip;

                    List<UrunTipIstasyon> list_sure = new List<UrunTipIstasyon>();

                    foreach (var sure in urun.Tip.T3_UrunTipIstasyon)
                    {
                        string istasyon = "";
                        if (sure.Istasyon != null)
                            istasyon = sure.Istasyon.Kod ?? "";

                        list_sure.Add(new UrunTipIstasyon
                        {
                            Id = sure.Id,
                            TipId = sure.TipId,
                            IstasyonId = sure.IstasyonId,
                            Istasyon = istasyon,
                            Zaman = sure.Zaman
                        });
                    }

                    var urun_Tip = (new UrunTip
                    {
                        Id = item.Id,
                        Product = item.Product,
                        Rated_Volt = item.Rated_Volt,
                        Panel_Width = item.Panel_Width,
                        Panel_Type = item.Panel_Type,
                        Panel_Curr = item.Panel_Curr,
                        Shortc_Curr = item.Shortc_Curr,
                        Ct = item.Ct,
                        Vt = item.Vt,
                        Vt_With = item.Vt_With,
                        Vt_Rem = item.Vt_Rem,
                        Vt_Fix = item.Vt_Fix,
                        Sa = item.Sa,
                        Cap_Volt_Ind = item.Cap_Volt_Ind,
                        Es_Present = item.Es_Present,
                        Es_Type_Subs = item.Es_Type_Subs,
                        Ct_Sec_Con = item.Ct_Sec_Con,
                        Vt_Sec_Con = item.Vt_Sec_Con,
                        Liste = list_sure,
                    });

                    list.Add(new Urun
                    {
                        Id = urun.Id,
                        TipId = urun.TipId,
                        ProjectId = urun.ProjectId,
                        ProjectName = urun.ProjectName,
                        BomNo = urun.Bom,
                        Switchgear = urun.Switchgear,
                        Panel_No = urun.Panel_No,
                        Shortc_Time = urun.Shortc_Time,
                        Discon = urun.Discon,
                        Kod = "<button style='max-height:50px;min-width:30px; height:40px' class='btn btn-dark' data-bs-toggle='modal' data-bs-target='#modal_Barkod_Goster' onclick=Urun_Seç(" + '"' + urun.Id + '"' + ")><i class='bi bi-upc-scan m-2'></i></button>",
                              //"<button style='max-height:50px;min-width:30px; height:40px' class='btn btn-primary' onclick=BarkodIndir(" + '"' + urun.Id + '"' + ")><i class='bi bi-cloud-arrow-down-fill m-2'></i></button>",
                        Tip = urun_Tip,
                        Barkod = urun.Barkod,
                    });

                }

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "İstasyon Listesi Gönderildi.",
                    Obje = XSabit.XSerialize(list),
                };

                //_logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                //                        DateTime.Now,
                //                        XSabit.XSerialize(x));
            }
            catch (Exception ex)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Hata oluştu.",
                    Obje = ex.ToString(),
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));

                return x;
            }
            return x;
        }

        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<XReturn> Post([FromBody] List<Urun> input)
        {
            string fonksiyon = _fonksiyon + "03";

            XReturn x = new()
            {
                Islem = false,
                Fonksiyon = fonksiyon,
                Kod = "İşlem Başarısız",
                Mesaj = "Hata oluştu.",
                Obje = "",
            };
             
            try
            {
                var list_Mevcut = await db.T3_Urun.ToListAsync();

                foreach (var item in input)
                {
                    var mevcuttaVarMı = list_Mevcut.Where(p => p.Id == item.Id)
                                                   .FirstOrDefault();

                    if (mevcuttaVarMı == null)
                    {
                        var tip = await db.T3_UrunTip.Where(p => p.Id == item.TipId)
                                                     .FirstOrDefaultAsync();
                        if (tip != null)
                        {
                            var id = Guid.NewGuid();
                            var barkod = "F2" + id.ToString().ToUpper().Split("-")[0];

                            db.T3_Urun.Add(new T3_Urun
                            {
                                Id = id,
                                TipId = item.TipId,
                                ProjectId = item.ProjectId,
                                ProjectName = item.ProjectName,
                                Bom = item.BomNo,
                                Switchgear = item.Switchgear,
                                Panel_No = item.Panel_No,
                                Shortc_Time = item.Shortc_Time,
                                Discon = item.Discon,
                                Kod = item.Kod,
                                Tip = tip,
                                Barkod = barkod,
                            });
                        }
                    }
                }

                await db.SaveChangesAsync();

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Ürünler eklendi",
                    Obje = "",
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));

            }
            catch (Exception ex)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Hata oluştu.",
                    Obje = ex.ToString(),
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));

                return x;
            }
            return x;
        }

        [HttpDelete]
        public async Task<XReturn> Delete()
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "05";

            var ürünler = await db.T3_Urun.ToListAsync();

            try
            {
                db.T3_Urun.RemoveRange(ürünler);
                await db.SaveChangesAsync();
                x = new XReturn
                {
                    Fonksiyon = fonksiyon,
                    Islem = true,
                    Kod = "Başarılı",
                    Mesaj = "Bütün ürünler silindi.",
                    Obje = ""
                };
            }
            catch (Exception ex)
            {
                x = new XReturn
                {
                    Fonksiyon = _fonksiyon,
                    Islem = false,
                    Kod = "Başarısız",
                    Mesaj = "Hata Oluştu",
                    Obje = ex.Message
                };
            }

            return x;
        }
    }
}
