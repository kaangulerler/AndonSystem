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
    public class LokasyonController : ControllerBase
    {
        private readonly ILogger<LokasyonController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx01";

        public LokasyonController(XModel context, ILogger<LokasyonController> logger)
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
                Mesaj = "Lokasyona oluştu",
                Obje = "",
            };

            try
            {
                IEnumerable<T3_Lokasyon>? nesne = null;
                if (db.T3_Lokasyon != null)
                {
                    nesne = await db.T3_Lokasyon.Include(p => p.InverseUst)
                                                .Include(p => p.T3_Istasyon)
                                                .ThenInclude(p => p.T3_IstasyonVardiya)
                                                .Include(p => p.T3_Istasyon)
                                                .ThenInclude(p => p.T3_PlanliCalisma)
                                                .ToListAsync();

                    if (nesne != null)
                    {
                        nesne = nesne.Where(p => p.Ust == null).ToList();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Lokasyon Listesi Gönderildi.",
                            Obje = nesne,
                        };


                        _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                                DateTime.Now,
                                                XSabit.XSerialize(x)
                                                );
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Lokasyona oluştu",
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
                T3_Lokasyon? Istasyon = null;
                if (db.T3_Lokasyon != null)
                {
                    Istasyon = await db.T3_Lokasyon.Where(p => p.Id == id)
                                                   .Include(p => p.InverseUst)
                                                   .Include(p => p.T3_Istasyon.OrderBy(p=>p.Sira))
                                                   .FirstOrDefaultAsync();
                }
                if (Istasyon == null)
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Lokasyon bulunamadı.",
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
                        Mesaj = "Lokasyon gönderildi.",
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

        [HttpGet("Tree")]
        public async Task<XReturn> GetTree()
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "02";

            try
            {

                var nesne = await db.T3_Lokasyon.Include(p => p.T3_Istasyon.OrderBy(p=>p.Sira))
                                                .ThenInclude(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                .Include(p => p.T3_Istasyon)
                                                .ThenInclude(p => p.T3_PlanliCalisma)
                                                .OrderBy(p => p.Kod).ToListAsync();

                nesne = nesne.Where(p => p.Ust == null).ToList();

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Lokasyon Listesi Gönderildi.",
                    Obje = nesne,
                };


                _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x)
                                        );

            }
            catch (Exception ex)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Lokasyona oluştu",
                    Obje = ex.ToString(),
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        (x));
            }

            return x;
        }


        public class InputLokasyon
        {
            public Guid LokasyonId { get; set; } = new Guid();
            public string Kod { get; set; } = String.Empty;
        }



        [HttpPost]
        public async Task<XReturn> Post([FromBody] InputLokasyon input)
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
                T3_Lokasyon? Lokasyon = null;
                if (db.T3_Lokasyon != null)
                {
                    Lokasyon = await db.T3_Lokasyon.Where(p => p.Kod == input.Kod)
                                                 .FirstOrDefaultAsync();
                }
                if (Lokasyon == null)
                {
                    try
                    {
                        var value = new T3_Lokasyon
                        {
                            Id = Guid.NewGuid(),
                            UstId = input.LokasyonId,
                            Kod = input.Kod,
                        };

                        if (input.LokasyonId == new Guid())
                            value.UstId = null;

                        if (db.T3_Lokasyon != null)
                        {
                            await db.T3_Lokasyon.AddAsync(value);
                            await db.SaveChangesAsync();
                        }
                        else
                            throw new Exception("Manual hata. Satır 188. Dosya LokasyonController.cs");


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
                        Mesaj = "Lokasyon zaten daha önce tanımlanmış.",
                        Obje = Lokasyon,
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
        public async Task<XReturn> Put(Guid id, [FromBody] InputLokasyon value)
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
                    Mesaj = "Lokasyon nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }
            T3_Lokasyon? Lokasyon = null;
            if (db.T3_Lokasyon != null)
            {
                Lokasyon = await db.T3_Lokasyon.Where(p => p.Id == id)
                                             .FirstOrDefaultAsync();
            }
            if (Lokasyon == null)
            {
                try
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Düzenlemek istediğiniz Lokasyon bulunamadı.",
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
                T3_Lokasyon? başkaVarMı = null;
                if (db.T3_Lokasyon != null)
                {
                    başkaVarMı = await db.T3_Lokasyon.Where(p => p.Id != id && p.Kod == value.Kod)
                                                    .FirstOrDefaultAsync();
                }

                if (başkaVarMı == null)
                {
                    try
                    {
                        Lokasyon.Kod = value.Kod;

                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Lokasyon düzenlemesi yapıldı.",
                            Obje = Lokasyon,
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
                        Mesaj = "Lokasyon zaten daha önce tanımlanmış.",
                        Obje = Lokasyon,
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
            T3_Lokasyon? Lokasyon = null;
            if (db.T3_Lokasyon != null)
            {
                Lokasyon = await db.T3_Lokasyon.Where(p => p.Id == id)
                                               .Include(p => p.InverseUst)
                                               .FirstOrDefaultAsync();
            }

            if (Lokasyon == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Obje = "Silmek istediğiniz Lokasyon bulunamadı.",
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
            }
            else
            {
                try
                {
                    if (db.T3_Lokasyon != null)
                        db.T3_Lokasyon.Remove(Lokasyon);

                    await db.SaveChangesAsync();

                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Lokasyon silme yapıldı.",
                        Obje = Lokasyon,
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

        public class LokasyonTasi
        {
            public Guid Id1 { get; set; }
            public Guid Id2 { get; set; }
        }

        [HttpPost("Tasi/")]
        public async Task<XReturn> Tasi([FromBody] LokasyonTasi value)
        {
            XReturn x;
            string fonksiyon = "Fx0205";

            if (value == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Lokasyon nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }

            var taşınan = db.T3_Lokasyon.Where(p => p.Id == value.Id1).FirstOrDefault();
            var bırakılan = db.T3_Lokasyon.Where(p => p.Id == value.Id2).FirstOrDefault();
            string bırakılanKod = "Ana Lokasyon";
            if (bırakılan != null)
                bırakılanKod = bırakılan.Kod;

            if ((taşınan != null && bırakılan != null) || (taşınan != null && value.Id2 == new Guid()))
            {
                if (value.Id2 != new Guid())
                    taşınan.UstId = value.Id2;
                else
                    taşınan.UstId = null;

                await db.SaveChangesAsync();
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Lokasyon taşıma başarılı. " + taşınan.Kod + " => " + bırakılanKod,
                    Obje = new { Taşınan = taşınan, Bırakılan = bırakılan },
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));

            }
            else
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Lokasyon bulunamadı.",
                    Obje = new { Taşınan = taşınan, Bırakılan = bırakılan },
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
            }

            return x;
        }
    }
}