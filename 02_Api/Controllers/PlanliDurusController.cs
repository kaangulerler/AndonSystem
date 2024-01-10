using _01_DbModel.AppModel;
using _01_DbModel.Db;
using _01_DbModel;
using _02_Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _02_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlanliDurusController : ControllerBase
    {
        private readonly ILogger<PlanliDurusController> _logger;
        private readonly XModel db;
        readonly string _fonksiyon = "Fx05";

        public class PlanliDurus
        {
            public Guid Id { get; set; }
            public bool Tip { get; set; } = false;
            public string Kod { get; set; } = "";
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
        }

        public class PlanliDurusDataItem
        {
            public Guid Id { get; set; }
            public string Istasyon { get; set; } = "";
            public string Kod { get; set; } = "";
            public string Bas { get; set; } = "";
            public string Bit { get; set; } = "";
            public int Zaman { get; set; }
            public string Buton { get; set; } = "";
        }
        public PlanliDurusController(XModel context, ILogger<PlanliDurusController> logger)
        {
            _logger = logger;
            db = context;
        }

        [HttpGet("{id}")]
        public async Task<XReturn> Get(Guid id)
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
            List<PlanliDurusDataItem> dataList = new List<PlanliDurusDataItem>();

            var lokasyon = await db.T3_Lokasyon.Where(p => p.Id == id)
                                               .Include(p => p.T3_Istasyon)
                                               .ThenInclude(p => p.T3_PlanliDurus)
                                               .FirstOrDefaultAsync();
            if (lokasyon != null)
            {
                foreach (var istasyon in lokasyon.T3_Istasyon.ToList())
                {
                    foreach (var durus in istasyon.T3_PlanliDurus)
                        dataList.Add(new PlanliDurusDataItem
                        {
                            Id = durus.Id,
                            Istasyon = istasyon.Kod,
                            Kod = durus.Kod,
                            Bas = durus.Baslangic.ToShortDateString() + " " + durus.Baslangic.ToShortTimeString(),
                            Bit = durus.Bitis.ToShortDateString() + " " + durus.Bitis.ToShortTimeString(),
                            Zaman = durus.Zaman,
                            Buton = "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Planli_Durus_Sil('" + durus.Id + "')><i class='bi bi-trash'></i></button>",
                        });
                }
            }
            else
            {
                var istasyon = await db.T3_Istasyon.Where(p => p.Id == id)
                                               .Include(p => p.T3_PlanliDurus)
                                               .FirstOrDefaultAsync();
                if (istasyon != null)
                    foreach (var durus in istasyon.T3_PlanliDurus)
                        dataList.Add(new PlanliDurusDataItem
                        {
                            Id = durus.Id,
                            Istasyon = istasyon.Kod,
                            Kod = durus.Kod,
                            Bas = durus.Baslangic.ToShortDateString() + " " + durus.Baslangic.ToShortTimeString(),
                            Bit = durus.Bitis.ToShortDateString() + " " + durus.Bitis.ToShortTimeString(),
                            Zaman = durus.Zaman,
                            Buton = "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Planli_Durus_Sil('" + durus.Id + "')><i class='bi bi-trash'></i></button>",
                        });
            }
            try
            {

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Planlı Duruş Listesi Gönderildi.",
                    Obje = dataList,
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
        [DisableRequestSizeLimit]
        [HttpPost]
        public async Task<XReturn> Post([FromBody] PlanliDurus input)
        {
            string fonksiyon = _fonksiyon + "06";
            XReturn x = new()
            {
                Islem = false,
                Fonksiyon = fonksiyon,
                Kod = "İşlem Başarısız",
                Mesaj = "Hata oluştu",
                Obje = "",
            };
            try
            {
                if (!input.Tip)
                {
                    List<string> listHata = new List<string>();
                    var istasyonVarmı = await db.T3_Istasyon.Where(p => p.Id == input.Id)
                                                            .Include(p => p.T3_PlanliDurus)
                                                            .FirstOrDefaultAsync();
                    if (istasyonVarmı != null)
                    {
                        foreach (var durus in istasyonVarmı.T3_PlanliDurus)
                        {
                            if ((input.Baslangic >= durus.Baslangic && input.Baslangic < durus.Bitis) || (input.Bitis <= durus.Bitis && input.Bitis > durus.Baslangic) || (input.Baslangic == durus.Baslangic && input.Bitis == durus.Bitis))
                                listHata.Add("Planlı Duruş" + durus.Kod + " ( " + durus.Baslangic + " - " + durus.Bitis + " ) ");
                        }
                    }

                    if (listHata.Count > 0)
                        return x = new XReturn
                        {
                            Fonksiyon = fonksiyon,
                            Islem = false,
                            Kod = "İşlem Başarısız",
                            Mesaj = "Girilen saatler arasında sisteme kayıtlı planlı çalışmalar mevcut.",
                            Obje = listHata
                        };

                    if (istasyonVarmı != null)
                    {
                        await db.T3_PlanliDurus.AddAsync(new T3_PlanliDurus
                        {
                            Id = Guid.NewGuid(),
                            IstasyonId = input.Id,
                            Baslangic = input.Baslangic,
                            Bitis = input.Bitis,
                            Kod = input.Kod,
                            Zaman = (int)((input.Bitis - input.Baslangic).TotalMinutes),
                            Istasyon = istasyonVarmı
                        });
                        await db.SaveChangesAsync();

                        x = new XReturn
                        {
                            Fonksiyon = fonksiyon,
                            Islem = true,
                            Kod = "İşlem Başırılı",
                            Mesaj = "Planlanmış Duruş Ekleme Başarılı",
                            Obje = istasyonVarmı,
                        };

                        _logger.LogInformation("Zaman : {Zaman}, İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));
                    }
                }
                else
                {
                    List<string> listHata = new List<string>();
                    var lokasyonVarmı = await db.T3_Lokasyon.Where(p => p.Id == input.Id)
                                                            .Include(p => p.T3_Istasyon)
                                                            .ThenInclude(p => p.T3_PlanliCalisma)
                                                            .Include(p => p.T3_Istasyon)
                                                            .ThenInclude(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.Vardiya)
                                                            .FirstOrDefaultAsync();

                    return x;
                }
            }
            catch
            {
                return x;
            }
            return x;
        }

        [HttpDelete("{id}")]
        public async Task<XReturn> Delete( Guid Id)
        {

            XReturn x;
            string fonksiyon = _fonksiyon + "05";

            var varmı = await db.T3_PlanliDurus.Where(p => p.Id == Id)
                                                .Include(p => p.Istasyon)
                                                .FirstOrDefaultAsync();
            if (varmı != null)
            {
                db.T3_PlanliDurus.Remove(varmı);
                await db.SaveChangesAsync();

                var liste = await db.T3_PlanliDurus.Where(p => p.IstasyonId == varmı.IstasyonId).ToListAsync();

                List<object> list_PlanlıDurus = new List<object>();

                foreach (var item in liste)
                {
                    list_PlanlıDurus.Add(new
                    {
                        id = item.Id,
                        istasyon = varmı.Istasyon.Kod,
                        bas = item.Baslangic,
                        bit = item.Bitis,
                        kod = item.Kod,
                        zaman = item.Zaman,
                        buton = "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Planli_Durus_Sil('" + item.Id + "')><i class='bi bi-trash'></i></button>"
                    });
                }

                x = new XReturn
                {
                    Fonksiyon = fonksiyon,
                    Islem = true,
                    Kod = "İşlem Başarılı",
                    Mesaj = varmı.Kod + " başarılı şekilde silindi.",
                    Obje = list_PlanlıDurus,
                };
            }
            else
            {
                x = new XReturn
                {
                    Fonksiyon = fonksiyon,
                    Islem = false,
                    Kod = "İşlem Başarısız",
                    Mesaj = "Çalışma bulunamadı",
                    Obje = "",
                };
            }

            return x;
        }
    }
}
