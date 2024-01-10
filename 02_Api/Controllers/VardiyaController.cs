using _01_DbModel;
using _01_DbModel.Db;
using _02_Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _02_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VardiyaController : ControllerBase
    {
        private readonly ILogger<VardiyaController> _logger;

        private readonly XModel db;

        string _fonksiyon = "Fx02";

        public VardiyaController(XModel context, ILogger<VardiyaController> logger)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet]
        public async Task<XReturn> Get()
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "01";

            try
            {
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Vardiya Listesi Gönderildi.",
                    Obje = await db.T3_Vardiya.Include(p => p.T3_VardiyaMola)
                                              .OrderBy(p => p.Kod)
                                              .ToListAsync(),
                };

                _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
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
                    Mesaj = "Hata oluştu",
                    Obje = ex.ToString(),
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        (x));
            }

            return x;
        }

        [HttpGet("{id}")]
        public async Task<XReturn> Get(Guid id)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "02";

            try
            {
                var Istasyon = await db.T3_Vardiya.Where(p => p.Id == id)
                                                     .Include(p => p.T3_VardiyaMola)
                                                     .FirstOrDefaultAsync();
                if (Istasyon == null)
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Vardiya bulunamadı.",
                    };
                    _logger.LogWarning("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                }
                else
                {
                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Vardiya gönderildi.",
                        Obje = Istasyon,
                    };
                    _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                }
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
            }

            return x;
        }

        public class InputVardiya
        {
            public string Kod { get; set; } = String.Empty;
            public string Baslangic { get; set; } = "00:00";
            public string Bitis { get; set; } = "00:00";
            public string[] gunler_List = new string[0];
        }

        [HttpPost]
        public async Task<XReturn> Post([FromBody] InputVardiya input)
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
                var Vardiya = await db.T3_Vardiya.Where(p => p.Kod == input.Kod)
                                                 .FirstOrDefaultAsync();
                if (Vardiya == null)
                {
                    try
                    {
                        var value = new T3_Vardiya
                        {
                            Id = Guid.NewGuid(),
                            Kod = input.Kod,
                            Baslangic = TimeSpan.Parse(input.Baslangic),
                            Bitis = TimeSpan.Parse(input.Bitis),
                        };

                        for (int i = 0; i < input.gunler_List.Length; i++)
                        {
                            string item = input.gunler_List[i];
                            if (item.Equals("Pazartesi"))
                            {
                                value.GunPazartesi = true;
                            }
                            else if (item.Equals("Salı"))
                            {
                                value.GunSali = true;
                            }
                            else if (item.Equals("Çarşamba"))
                            {
                                value.GunCarsamba = true;
                            }
                            else if (item.Equals("Perşembe"))
                            {
                                value.GunPersembe = true;
                            }
                            else if (item.Equals("Cuma"))
                            {
                                value.GunCuma = true;
                            }
                            else if (item.Equals("Cumartesi"))
                            {
                                value.GunCumartesi = true;
                            }
                            else if (item.Equals("Pazar"))
                            {
                                value.GunPazar = true;
                            }
                        }


                        await db.T3_Vardiya.AddAsync(value);
                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Obje = value,
                        };

                        _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
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
                }
                else
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Mesaj = "Vardiya zaten daha önce tanımlanmış.",
                        Obje = Vardiya,
                    };
                    _logger.LogWarning("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));

                    return x;
                }
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

        [HttpPut("{id}")]
        public async Task<XReturn> Put(Guid id, [FromBody] InputVardiya value)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "04";
             
            if (value == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Vardiya nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }

            var Vardiya = await db.T3_Vardiya.Where(p => p.Id == id)
                                             .Include(p => p.T3_VardiyaMola)
                                             .FirstOrDefaultAsync();

            if (Vardiya == null)
            {
                try
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Düzenlemek istediğiniz Vardiya bulunamadı.",
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
                }
            }
            else
            {

                var başlangıç = TimeSpan.Parse(value.Baslangic);
                var bitiş = TimeSpan.Parse(value.Bitis);

                foreach (var mola in Vardiya.T3_VardiyaMola)
                {
                    if (mola.Baslangic < başlangıç || mola.Baslangic > bitiş ||
                       mola.Bitis < başlangıç || mola.Bitis > bitiş)
                    {
                        x = new()
                        {
                            Islem = false,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarısız",
                            Obje = "Vardiyanın saatleri " + mola.Kod + " molası ile uyuşmuyor.",
                        };
                        _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                            DateTime.Now,
                                            XSabit.XSerialize(x));

                        return x;
                    }
                }

                var başkaVarMı = await db.T3_Vardiya.Where(p => p.Id != id && p.Kod == value.Kod)
                                                    .FirstOrDefaultAsync();

                if (başkaVarMı == null)
                {
                    try
                    {
                        Vardiya.Kod = value.Kod;
                        Vardiya.Baslangic = başlangıç;
                        Vardiya.Bitis = bitiş;
                        Vardiya.GunPazartesi = false;
                        Vardiya.GunSali = false;
                        Vardiya.GunCarsamba = false;
                        Vardiya.GunCuma = false;
                        Vardiya.GunCumartesi = false;
                        Vardiya.GunPazar = false;

                        for (int i = 0; i < value.gunler_List.Length; i++)
                        {
                            string item = value.gunler_List[i];
                            if (item.Equals("Pazartesi"))
                            {
                                Vardiya.GunPazartesi = true;
                            }
                            else if (item.Equals("Salı"))
                            {
                                Vardiya.GunSali = true;
                            }
                            else if (item.Equals("Çarşamba"))
                            {
                                Vardiya.GunCarsamba = true;
                            }
                            else if (item.Equals("Perşembe"))
                            {
                                Vardiya.GunPersembe = true;
                            }
                            else if (item.Equals("Cuma"))
                            {
                                Vardiya.GunCuma = true;
                            }
                            else if (item.Equals("Cumartesi"))
                            {
                                Vardiya.GunCumartesi = true;
                            }
                            else if (item.Equals("Pazar"))
                            {
                                Vardiya.GunPazar = true;
                            }
                        }
                        db.T3_VardiyaMola.RemoveRange(Vardiya.T3_VardiyaMola.ToList());

                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Vardiya düzenlemesi yapıldı.",
                            Obje = Vardiya,
                        };

                        _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                        return x;

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

                }
                else
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Mesaj = "Vardiya zaten daha önce tanımlanmış.",
                        Obje = Vardiya,
                    };

                    _logger.LogWarning("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                    return x;
                }
            }
            return x;
        }

        [HttpDelete("{id}")]
        public async Task<XReturn> Delete(Guid id)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "05";

            var Vardiya = await db.T3_Vardiya.Where(p => p.Id == id)
                                              .Include(p => p.T3_IstasyonVardiya)
                                              .Include(p => p.T3_VardiyaMola)
                                              .FirstOrDefaultAsync();
            if (Vardiya == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Obje = "Silmek istediğiniz Vardiya bulunamadı.",
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
            }
            else
            {
                try
                {
                    db.T3_Vardiya.Remove(Vardiya);

                    await db.SaveChangesAsync();

                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Vardiya silme yapıldı.",
                        Obje = Vardiya,
                    };
                    _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                    return x;
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
                }
            }
            return x;
        }
    }
}
