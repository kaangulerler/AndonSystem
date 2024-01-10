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
    public class IstasyonController : ControllerBase
    {
        private readonly ILogger<IstasyonController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx04";

        public class InputIstasyon
        {
            public Guid LokasyonId { get; set; } = new Guid();
            public string Kod { get; set; } = String.Empty;
        }

        public IstasyonController(XModel context, ILogger<IstasyonController> logger)
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
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "İstasyon Listesi Gönderildi.",
                    Obje = XSabit.XSerialize(await db.T3_Istasyon.OrderBy(p => p.Sira)
                                                                 .ToListAsync()),
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
         

        [HttpPost] 
        public async Task<XReturn> Post([FromBody] InputIstasyon input)
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
                var Istasyon = await db.T3_Istasyon.Where(p => p.Kod == input.Kod)
                                                   .FirstOrDefaultAsync();
                if (Istasyon == null)
                {
                    var Lokasyon = await db.T3_Lokasyon.Where(p => p.Id == input.LokasyonId)
                                                       .FirstOrDefaultAsync();

                    if (Lokasyon != null)
                    {
                        try
                        {
                            var id = Guid.NewGuid();
                            var barkod = "F1" + id.ToString().ToUpper().Split("-")[0];

                            var value = new T3_Istasyon
                            {
                                Id = id,
                                Kod = input.Kod,
                                LokasyonId = input.LokasyonId,
                                //Lokasyon = Lokasyon,
                                IpAdres = "",
                                SiraNo = 0,
                                Sira = 0,
                                Barkod = barkod
                            };

                            await db.T3_Istasyon.AddAsync(value);
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
                }
                else
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Mesaj = "Istasyon zaten daha önce tanımlanmış.",
                        Obje = Istasyon,
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
        public async Task<XReturn> Put(Guid id, [FromBody] InputIstasyon value)
        {
            XReturn x;
            string fonksiyon = "Fx0304";

            if (value == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Istasyon nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }

            var Istasyon = await db.T3_Istasyon.Where(p => p.Id == id)
                                     .FirstOrDefaultAsync();
            
            if (Istasyon == null)
            {
                try
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Düzenlemek istediğiniz Istasyon bulunamadı.",
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
                var başkaVarMı = await db.T3_Istasyon.Where(p => p.Id != id && p.Kod == value.Kod)
                                                        .FirstOrDefaultAsync();

                if (başkaVarMı == null)
                {
                    try
                    {
                        Istasyon.Kod = value.Kod;
                        Istasyon.SiraNo = 0;
                        Istasyon.IpAdres = "";
                        Istasyon.LokasyonId = value.LokasyonId;

                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Istasyon düzenlemesi yapıldı.",
                            Obje = Istasyon,
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
                        Mesaj = "Istasyon zaten daha önce tanımlanmış.",
                        Obje = Istasyon,
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
            string fonksiyon = "Fx0305";

            var Istasyon = await db.T3_Istasyon.Where(p => p.Id == id)
                                                .FirstOrDefaultAsync();
            if (Istasyon == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Obje = "Silmek istediğiniz Istasyon bulunamadı.",
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
            }
            else
            {
                try
                {
                    db.T3_Istasyon.Remove(Istasyon);

                    await db.SaveChangesAsync();

                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Istasyon silme yapıldı.",
                        Obje = Istasyon,
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
