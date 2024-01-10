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
    public class MolaController : ControllerBase
    {
        private readonly ILogger<MolaController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx03";

        public MolaController(XModel context, ILogger<MolaController> logger)
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
                    Mesaj = "Mola Listesi Gönderildi.",
                    Obje = await db.T3_VardiyaMola.ToListAsync(),
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
                var molaList = await db.T3_VardiyaMola.Where(p => p.VardiyaId == id)
                                               .ToListAsync();
                if (molaList == null)
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Mola bulunamadı.",
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
                        Mesaj = "Mola gönderildi.",
                        Obje = molaList,
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

        public class InputMola
        {
            public Guid VardiyaId { get; set; } = new Guid();
            public string Kod { get; set; } = string.Empty;
            public string Baslangic { get; set; } = "00:00";
            public string Bitis { get; set; } = "00:00";
        }

        [HttpPost]
        public async Task<XReturn> Post([FromBody] InputMola input)
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
                var başlangıç = TimeSpan.Parse(input.Baslangic);
                var bitiş = TimeSpan.Parse(input.Bitis);
                 
                var Vardiya = await db.T3_Vardiya.Where(p => p.Id == input.VardiyaId)
                                                 .Include(p => p.T3_VardiyaMola)
                                                 .FirstOrDefaultAsync();

                var vardiyaBas = new TimeSpan(0);
                var vardiyaBit = new TimeSpan(0);


                if (Vardiya != null)
                {
                    vardiyaBas = new TimeSpan(Vardiya.Baslangic.Ticks);
                    vardiyaBit = new TimeSpan(Vardiya.Bitis.Ticks);

                    if (vardiyaBas > vardiyaBit)
                        vardiyaBit = vardiyaBit.Add(TimeSpan.FromDays(1));

                    bool aralıkDışı = (vardiyaBas >= başlangıç) || (vardiyaBit <= başlangıç) ||
                                      (vardiyaBas >= bitiş) || (vardiyaBit <= bitiş);

                    if (aralıkDışı)
                        return x = new()
                        {
                            Islem = false,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarısız",
                            Mesaj = "Mola saatleri vardiya saatleri dışında.",
                            Obje = "",
                        };

                    foreach (var mola in Vardiya.T3_VardiyaMola)
                    {
                        bool çakışma = (başlangıç >= mola.Baslangic && bitiş <= mola.Bitis) ||
                                       (başlangıç >= mola.Baslangic && bitiş <= mola.Bitis);
                        if (çakışma)
                            return x = new()
                            {
                                Islem = false,
                                Fonksiyon = fonksiyon,
                                Kod = "İşlem Başarısız",
                                Mesaj = "Vardiyanın diğer molasıyla saatleri çakıştı. Mola : " + mola.Kod,
                                Obje = mola,
                            };
                    }
                }
                else
                {
                    return x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Mesaj = "Vardiya bulunamadı.",
                        Obje = "",
                    };
                }

                var Mola = await db.T3_VardiyaMola.Where(p => p.Kod == input.Kod && p.VardiyaId == input.VardiyaId)
                                                  .FirstOrDefaultAsync();
                if (Mola == null)
                {
                    try
                    {
                        var value = new T3_VardiyaMola
                        {
                            Id = Guid.NewGuid(),
                            VardiyaId = input.VardiyaId,
                            Kod = input.Kod,
                            Baslangic = başlangıç,
                            Bitis = bitiş,
                            Zaman = (int)((bitiş - başlangıç).TotalMinutes)
                        };

                        await db.T3_VardiyaMola.AddAsync(value);
                        await db.SaveChangesAsync();

                        var değer = await db.T3_VardiyaMola.Where(p => p.Id == value.Id)
                                                    .FirstOrDefaultAsync();
                        if (değer != null)
                            x = new()
                            {
                                Islem = true,
                                Fonksiyon = fonksiyon,
                                Kod = "İşlem Başarılı",
                                Obje = değer,
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
                        Mesaj = "Mola zaten daha önce tanımlanmış.",
                        Obje = Mola,
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

        [HttpDelete("{id}")]
        public async Task<XReturn> Delete(Guid id)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "05";

            var Mola = await db.T3_VardiyaMola.Where(p => p.Id == id)
                                       .FirstOrDefaultAsync();
            if (Mola == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Obje = "Silmek istediğiniz Mola bulunamadı.",
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
            }
            else
            {
                try
                {
                    db.T3_VardiyaMola.Remove(Mola);

                    await db.SaveChangesAsync();

                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Mola silme yapıldı.",
                        Obje = Mola,
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

        [HttpDelete]
        public async Task<XReturn> DeleteAll([FromBody] List<String> liste)//FromBody: Yapılan isteğin gövdesinde veriyi arar. https://docs.microsoft.com/tr-tr/aspnet/core/mvc/models/model-binding?view=aspnetcore-6.0
        {
            XReturn x;

            string fonksiyon = _fonksiyon + "05";
            try
            {
                foreach (var item in liste)
                {
                    db.T3_VardiyaMola.Remove(db.T3_VardiyaMola.Where(z => z.Id.ToString() == item).First());
                }

                await db.SaveChangesAsync();
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Mola silme yapıldı.",
                    Obje = liste,
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
            return x;

        }
    }
}
