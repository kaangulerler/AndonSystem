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
    public class UrunTipController : ControllerBase
    {
        private readonly ILogger<IstasyonController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx04";

        public UrunTipController(XModel context, ILogger<IstasyonController> logger)
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

                var obje = await db.T3_UrunTip.Where(p=>p.Id != new Guid())
                                                .Include(p => p.T3_UrunTipIstasyon)
                                                .ThenInclude(p => p.Istasyon)
                                                .OrderBy(p => p.Product)
                                                .ThenBy(p => p.Rated_Volt)
                                                .ThenBy(p => p.Panel_Width)
                                                .ToListAsync();

                var kontrol = (DateTime.Now - zaman).TotalMilliseconds;

                List<object> list = new List<object>();

                foreach (var item in obje)
                {
                    List<UrunTipIstasyon> list_sure = new List<UrunTipIstasyon>();

                    foreach (var sure in item.T3_UrunTipIstasyon)
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


                    list.Add(new UrunTip
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
        public async Task<XReturn> Post([FromBody] List<UrunTip> input)
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
                var list_Istasyon = await db.T3_Istasyon.ToListAsync();

                var list_Mevcut = await db.T3_UrunTip.Include(p => p.T3_UrunTipIstasyon)
                                                     .ToListAsync();

                foreach (var item in input)
                {
                    var mevcuttaVarMı = list_Mevcut.Where(p => p.Product == item.Product &&
                                                              p.Cap_Volt_Ind == item.Cap_Volt_Ind &&
                                                              p.Ct == item.Ct &&
                                                              p.Ct_Sec_Con == item.Ct_Sec_Con &&
                                                              p.Es_Present == item.Es_Present &&
                                                              p.Es_Type_Subs == item.Es_Type_Subs &&
                                                              p.Panel_Curr == item.Panel_Curr &&
                                                              p.Panel_Width == item.Panel_Width &&
                                                              p.Panel_Type == item.Panel_Type &&
                                                              p.Rated_Volt == item.Rated_Volt &&
                                                              p.Sa == item.Sa &&
                                                              p.Shortc_Curr == item.Shortc_Curr &&
                                                              p.Vt == item.Vt &&
                                                              p.Vt_With == item.Vt_With &&
                                                              p.Vt_Fix == item.Vt_Fix &&
                                                              p.Vt_Rem == item.Vt_Rem &&
                                                              p.Vt_Sec_Con == item.Vt_Sec_Con)

                                                       .FirstOrDefault();
                    if (mevcuttaVarMı == null)
                    {
                        item.Id = Guid.NewGuid();

                        var yeniUrunTip = new T3_UrunTip
                        {
                            Id = item.Id,
                            Product = item.Product,
                            Cap_Volt_Ind = item.Cap_Volt_Ind,
                            Ct = item.Ct,
                            Ct_Sec_Con = item.Ct_Sec_Con,
                            Es_Present = item.Es_Present,
                            Es_Type_Subs = item.Es_Type_Subs,
                            Panel_Curr = item.Panel_Curr,
                            Panel_Type = item.Panel_Type,
                            Panel_Width = item.Panel_Width,
                            Rated_Volt = item.Rated_Volt,
                            Sa = item.Sa,
                            Shortc_Curr = item.Shortc_Curr,
                            Vt = item.Vt,
                            Vt_Fix = item.Vt_Fix,
                            Vt_Rem = item.Vt_Rem,
                            Vt_Sec_Con = item.Vt_Sec_Con,
                            Vt_With = item.Vt_With,
                            T3_UrunTipIstasyon = new List<T3_UrunTipIstasyon>()
                        };

                        foreach (var w in item.Liste)
                        {
                            yeniUrunTip.T3_UrunTipIstasyon.Add(new T3_UrunTipIstasyon
                            {
                                Id = Guid.NewGuid(),
                                IstasyonId = w.IstasyonId,
                                TipId = yeniUrunTip.Id,
                                Zaman = w.Zaman,
                            });
                        }

                        db.T3_UrunTip.Add(yeniUrunTip);
                    }
                    else
                    {
                        db.T3_UrunTipIstasyon.RemoveRange(mevcuttaVarMı.T3_UrunTipIstasyon);

                        foreach (var tip_ist in item.Liste)
                            tip_ist.TipId = mevcuttaVarMı.Id;

                        foreach (var w in item.Liste)
                        {
                            mevcuttaVarMı.T3_UrunTipIstasyon.Add(new T3_UrunTipIstasyon
                            {
                                Id = Guid.NewGuid(),
                                IstasyonId = w.IstasyonId,
                                TipId = mevcuttaVarMı.Id,
                                Zaman = w.Zaman,
                            });
                        }

                        db.Update(mevcuttaVarMı);
                    }

                }

                await db.SaveChangesAsync();

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

            var ürün_Tipleri = await db.T3_UrunTip.Include(p => p.T3_UrunTipIstasyon)
                                                   .Include(p => p.T3_Urun)
                                                   .ToListAsync();

            try
            {
                db.T3_UrunTip.RemoveRange(ürün_Tipleri);
                await db.SaveChangesAsync();
                x = new XReturn
                {
                    Fonksiyon = fonksiyon,
                    Islem = true,
                    Kod = "Başarılı",
                    Mesaj = "Bütün ürün tipleri silindi.",
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
