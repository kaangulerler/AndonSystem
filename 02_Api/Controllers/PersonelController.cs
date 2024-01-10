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
    public class PersonelController : ControllerBase
    {
        private readonly ILogger<PersonelController> _logger;
        private readonly XModel db;
        readonly string _fonksiyon = "Fx05";
        
        public class InputPersonel
        {
            public Guid PersonelId { get; set; } = new Guid();

            public string Kod { get; set; } = string.Empty;
            public string Ad { get; set; } = string.Empty;
            public string Soyad { get; set; } = string.Empty;
        }

        public PersonelController(XModel context, ILogger<PersonelController> logger)
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
                    Mesaj = "Personel Listesi Gönderildi.",
                    Obje = await db.T3_Personel.ToListAsync(),
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
                var Personel = await db.T3_Personel.Where(p => p.Id == id).FirstOrDefaultAsync();
                if (Personel == null)
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Personel bulunamadı.",
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
                        Mesaj = "Personel gönderildi.",
                        Obje = Personel,
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
        [HttpPost]
        public async Task<XReturn> Post([FromBody] InputPersonel input)
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


            var personelVarmı = await db.T3_Personel.Where(p => p.Kod == input.Kod).FirstOrDefaultAsync();

            try
            {
                if (personelVarmı == null)
                {
                    try
                    {
                        var id = Guid.NewGuid();
                        var barkod = "F0" + id.ToString().ToUpper().Split("-")[0];

                        await db.T3_Personel.AddAsync(new T3_Personel
                        {
                            Id = id,
                            Kod = input.Kod,
                            Ad = input.Ad,
                            Soyad = input.Soyad,
                            Barkod = barkod,
                        });
                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = input.Kod + " personeli sisteme eklendi.",
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
                }
                else
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Mesaj = "Personel zaten daha önce tanımlanmış.",
                        Obje = "",
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
        public async Task<XReturn> Put(Guid id, [FromBody] InputPersonel value)
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
                    Mesaj = "Personel nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }

            var Personel = await db.T3_Personel.Where(p => p.Id == id).FirstOrDefaultAsync();
             
            if (Personel == null)
            {
                try
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Düzenlemek istediğiniz personel bulunamadı.",
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
                var başkaVarMı = await db.T3_Personel.Where(p => p.Id != id && p.Kod == value.Kod).FirstOrDefaultAsync();

                if (başkaVarMı == null)
                {
                    try
                    {
                        Personel.Kod = value.Kod;
                        Personel.Ad = value.Ad;
                        Personel.Soyad = value.Soyad;

                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Personel düzenlemesi yapıldı.",
                            Obje = Personel,
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
                        Mesaj = "Personel zaten daha önce tanımlanmış.",
                        Obje = Personel,
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

            var personela = await db.T3_Personel.Where(p => p.Id == id).FirstOrDefaultAsync();
            if (personela == null)
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
                    db.T3_Personel.Remove(personela);

                    await db.SaveChangesAsync();

                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Vardiya silme yapıldı.",
                        Obje = personela,
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
