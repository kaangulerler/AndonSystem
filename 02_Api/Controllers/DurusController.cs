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
    public class DurusController : ControllerBase
    {
        private readonly ILogger<DurusController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx01";

        public DurusController(XModel context, ILogger<DurusController> logger)
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
                Mesaj = "Durusa oluştu",
                Obje = "",
            };

            try
            {
                IEnumerable<T3_DurusTip>? nesne = null;
                if (db.T3_DurusTip != null)
                {
                    nesne = await db.T3_DurusTip.Include(p => p.InverseDurusTip)
                                                .ToListAsync();

                    if (nesne != null)
                    {
                        nesne = nesne.Where(p => p.DurusTip == null).ToList();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Durus Listesi Gönderildi.",
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
                    Mesaj = "Durusa oluştu",
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
                T3_DurusTip? Istasyon = null;
                if (db.T3_DurusTip != null)
                {
                    Istasyon = await db.T3_DurusTip.Where(p => p.Id == id)
                                                   .Include(p => p.InverseDurusTip)
                                                   .FirstOrDefaultAsync();
                }
                if (Istasyon == null)
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Durus bulunamadı.",
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
                        Mesaj = "Durus gönderildi.",
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

                var nesne = await db.T3_DurusTip.Include(p => p.InverseDurusTip).OrderBy(p => p.Kod).ToListAsync();

                nesne = nesne.Where(p => p.DurusTip == null).ToList();

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Durus Listesi Gönderildi.",
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
                    Mesaj = "Durusa oluştu",
                    Obje = ex.ToString(),
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        (x));
            }

            return x;
        }


        public class InputDurus
        {
            public Guid DurusTipId { get; set; } = new Guid();
            public string Kod { get; set; } = String.Empty;
        }



        [HttpPost]
        public async Task<XReturn> Post([FromBody] InputDurus input)
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
                var Durus = await db.T3_DurusTip.Where(p => p.Kod == input.Kod)
                                                .FirstOrDefaultAsync(); 
                if (Durus == null)
                {
                    try
                    {
                        var id = Guid.NewGuid();
                        var barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

                        var value = new T3_DurusTip
                        {
                            Id = id,
                            DurusTipId = input.DurusTipId,
                            Kod = input.Kod,
                            Barkod = barkod,
                            DurusTipTree = "",
                        };

                        if (input.DurusTipId == new Guid())
                            value.DurusTipId = null;

                        if (db.T3_DurusTip != null)
                        {
                            await db.T3_DurusTip.AddAsync(value);
                            await db.SaveChangesAsync();
                        }
                        else
                            throw new Exception("Manual hata. Satır 188. Dosya DurusController.cs");


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
                        Mesaj = "Durus zaten daha önce tanımlanmış.",
                        Obje = Durus,
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
        public async Task<XReturn> Put(Guid id, [FromBody] InputDurus value)
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
                    Mesaj = "Durus nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }
            T3_DurusTip? Durus = null;
            if (db.T3_DurusTip != null)
            {
                Durus = await db.T3_DurusTip.Where(p => p.Id == id)
                                             .FirstOrDefaultAsync();
            }
            if (Durus == null)
            {
                try
                {
                    x = new()
                    {
                        Islem = false,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarısız",
                        Obje = "Düzenlemek istediğiniz Durus bulunamadı.",
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
                T3_DurusTip? başkaVarMı = null;
                if (db.T3_DurusTip != null)
                {
                    başkaVarMı = await db.T3_DurusTip.Where(p => p.Id != id && p.Kod == value.Kod)
                                                    .FirstOrDefaultAsync();
                }

                if (başkaVarMı == null)
                {
                    try
                    {
                        Durus.Kod = value.Kod;

                        await db.SaveChangesAsync();

                        x = new()
                        {
                            Islem = true,
                            Fonksiyon = fonksiyon,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Durus düzenlemesi yapıldı.",
                            Obje = Durus,
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
                        Mesaj = "Durus zaten daha önce tanımlanmış.",
                        Obje = Durus,
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
            T3_DurusTip? Durus = null;
            if (db.T3_DurusTip != null)
            {
                Durus = await db.T3_DurusTip.Where(p => p.Id == id)
                                               .Include(p => p.InverseDurusTip)
                                               .FirstOrDefaultAsync();
            }

            if (Durus == null)
            {
                x = new()
                {
                    Islem = false,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarısız",
                    Obje = "Silmek istediğiniz Durus bulunamadı.",
                };

                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
            }
            else
            {
                try
                {
                    if (db.T3_DurusTip != null)
                        db.T3_DurusTip.Remove(Durus);

                    await db.SaveChangesAsync();

                    x = new()
                    {
                        Islem = true,
                        Fonksiyon = fonksiyon,
                        Kod = "İşlem Başarılı",
                        Mesaj = "Durus silme yapıldı.",
                        Obje = Durus,
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

        public class DurusTasi
        {
            public Guid Id1 { get; set; }
            public Guid Id2 { get; set; }
        }
        [HttpPost("Tasi/")]
        public async Task<XReturn> Tasi([FromBody] DurusTasi value)
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
                    Mesaj = "Durus nesnesi yanlış gönderildi.",
                    Obje = "",
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                return x;
            }

            var taşınan = db.T3_DurusTip.Where(p => p.Id == value.Id1).FirstOrDefault();
            var bırakılan = db.T3_DurusTip.Where(p => p.Id == value.Id2).FirstOrDefault() ?? new T3_DurusTip();
            string bırakılanKod = bırakılan.Kod ?? "Ana Durus";

            if ((taşınan != null && bırakılan != null) || (taşınan != null && value.Id2 == new Guid()))
            {
                if (value.Id2 != new Guid())
                    taşınan.DurusTipId = value.Id2;
                else
                    taşınan.DurusTipId = null;

                await db.SaveChangesAsync();
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Durus taşıma başarılı. " + taşınan.Kod + " => " + bırakılanKod,
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
                    Mesaj = "Durus bulunamadı.",
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