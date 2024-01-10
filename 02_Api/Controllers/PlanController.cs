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
    public class PlanController : ControllerBase
    {
        private readonly ILogger<PlanController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx04";

        public class PlanliCalisma
        {
            public Guid Id { get; set; }
            public bool Tip { get; set; } = false; // False İstasyon ; True Lokasyon
            public string Aciklama { get; set; } = "";
            public int Hedef { get; set; }
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
        }

        public class PlanliCalismaDataItem
        {
            public Guid id { get; set; }
            public string istasyon { get; set; } = "";
            public string kod { get; set; } = "";
            public string bas { get; set; } = "";
            public string bit { get; set; } = "";
            public int hedef { get; set; }
            public int zaman { get; set; }
            public string buton { get; set; } = "";
        }

        public PlanController(XModel context, ILogger<PlanController> logger)
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

            List<PlanliCalismaDataItem> dataList = new List<PlanliCalismaDataItem>();

            var lokasyon = await db.T3_Lokasyon.Where(p => p.Id == id)
                                               .Include(p => p.T3_Istasyon)
                                               .ThenInclude(p => p.T3_PlanliCalisma)
                                               .FirstOrDefaultAsync();

            if (lokasyon != null)
            {
                foreach (var istasyon in lokasyon.T3_Istasyon.ToList())
                {
                    foreach (var calisma in istasyon.T3_PlanliCalisma)
                        dataList.Add(new PlanliCalismaDataItem
                        {
                            id = calisma.Id,
                            istasyon = istasyon.Kod,
                            kod = calisma.Kod,
                            bas = calisma.Baslangic.ToShortDateString() + " " + calisma.Baslangic.ToShortTimeString(),
                            bit = calisma.Bitis.ToShortDateString() + " " + calisma.Bitis.ToShortTimeString(),
                            hedef = calisma.Hedef,
                            zaman = calisma.Zaman,
                            buton = "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Plan_Sil('" + calisma.Id + "')><i class='bi bi-trash'></i></button>",
                            
                        });
                }
            }
            else
            {
                var istasyon = await db.T3_Istasyon.Where(p => p.Id == id)
                                               .Include(p => p.T3_PlanliCalisma)
                                               .FirstOrDefaultAsync();
                if (istasyon != null)
                    foreach (var calisma in istasyon.T3_PlanliCalisma)
                        dataList.Add(new PlanliCalismaDataItem
                        {
                            id = calisma.Id,
                            istasyon = istasyon.Kod,
                            kod = calisma.Kod,
                            bas = calisma.Baslangic.ToShortDateString() + " " + calisma.Baslangic.ToShortTimeString(),
                            bit = calisma.Bitis.ToShortDateString() + " " + calisma.Bitis.ToShortTimeString(),
                            hedef = calisma.Hedef,
                            zaman = calisma.Zaman,
                            buton = "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Plan_Sil('" + calisma.Id + "')><i class='bi bi-trash'></i></button>",
                            
                        });
            }


            try
            {
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Planlı çalışma Listesi Gönderildi.",
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
        public async Task<XReturn> Post([FromBody] PlanliCalisma input)
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
                if (!input.Tip)
                {
                    List<string> listHata = new List<string>();

                    var istasyonVarmı = await db.T3_Istasyon.Where(p => p.Id == input.Id)
                                                            .Include(p => p.T3_PlanliCalisma)
                                                            .Include(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.Vardiya)
                                                            .FirstOrDefaultAsync();
                    if (istasyonVarmı != null)
                    {
                        foreach (var çalışma in istasyonVarmı.T3_PlanliCalisma)
                        {
                            if ((input.Baslangic >= çalışma.Baslangic && input.Baslangic < çalışma.Bitis) || (input.Bitis <= çalışma.Bitis && input.Bitis > çalışma.Baslangic) || (input.Baslangic == çalışma.Baslangic && input.Bitis == çalışma.Bitis))
                                listHata.Add("Planlı Çalışma" + çalışma.Kod + " ( " + çalışma.Baslangic + " - " + çalışma.Bitis + " ) ");
                        }

                        foreach (var vardiya in istasyonVarmı.T3_IstasyonVardiya.Select(p => p.Vardiya))
                        {
                            if (vardiya != null)
                            {
                                var vardiyaBaş = DateTime.Parse(input.Baslangic.ToShortDateString() + " " + vardiya.Baslangic.ToString("c"));
                                var vardiyaBit = DateTime.Parse(input.Baslangic.ToShortDateString() + " " + vardiya.Bitis.ToString("c"));

                                if (vardiyaBaş > vardiyaBit)
                                    vardiyaBit = vardiyaBit.AddDays(1);

                                if (vardiyaBaş > input.Baslangic)
                                {
                                    vardiyaBaş = vardiyaBaş.AddDays(-1);
                                    vardiyaBit = vardiyaBit.AddDays(-1);
                                }
                                 
                                if ((input.Baslangic >= vardiyaBaş && input.Baslangic < vardiyaBit) || (input.Bitis <= vardiyaBit && input.Bitis > vardiyaBaş) || (input.Baslangic == vardiyaBaş && input.Bitis == vardiyaBit))
                                     listHata.Add("Vardiya : " + vardiya.Kod);
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
                    }

                    if (istasyonVarmı != null)
                    {
                        await db.T3_PlanliCalisma.AddAsync(new T3_PlanliCalisma
                        {
                            Id = Guid.NewGuid(),
                            IstasyonId = input.Id,
                            Baslangic = input.Baslangic,
                            Bitis = input.Bitis,
                            Kod = input.Aciklama,
                            Zaman = (int)((input.Bitis - input.Baslangic).TotalMinutes),
                            Istasyon = istasyonVarmı,
                            Hedef = input.Hedef
                        });

                        await db.SaveChangesAsync();
                          
                        x = new XReturn
                        {
                            Fonksiyon = fonksiyon,
                            Islem = true,
                            Kod = "İşlem Başarılı",
                            Mesaj = "Planlanmış Çalışma Ekleme Başarılı.",
                            Obje = istasyonVarmı,
                        };

                        _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        XSabit.XSerialize(x));


                    }
                }
                else
                {

                    List<string> listHata = new List<string>();

                    var lokasyonVarmı = await db.T3_Lokasyon.Where(p => p.Id == input.Id)
                                                            .Include(p=>p.T3_Istasyon)
                                                            .ThenInclude(p => p.T3_PlanliCalisma)
                                                            .Include(p => p.T3_Istasyon)
                                                            .ThenInclude(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.Vardiya)
                                                            .FirstOrDefaultAsync();
                    if (lokasyonVarmı != null)
                    {

                        foreach (var istasyon in lokasyonVarmı.T3_Istasyon)
                        {
                            foreach (var çalışma in istasyon.T3_PlanliCalisma)
                            {
                                if ((input.Baslangic >= çalışma.Baslangic && input.Baslangic < çalışma.Bitis) || (input.Bitis <= çalışma.Bitis && input.Bitis > çalışma.Baslangic) || (input.Baslangic == çalışma.Baslangic && input.Bitis == çalışma.Bitis))
                                    listHata.Add("İstasyon (" + istasyon.Kod + ") Planlı Çalışma : " + çalışma.Kod + " ( " + çalışma.Baslangic + " - " + çalışma.Bitis + " ) ");
                            }

                            foreach (var vardiya in istasyon.T3_IstasyonVardiya.Select(p => p.Vardiya))
                            {
                                if (vardiya != null)
                                {
                                    var vardiyaBaş = DateTime.Parse(input.Baslangic.ToShortDateString() + " " + vardiya.Baslangic.ToString("c"));
                                    var vardiyaBit = DateTime.Parse(input.Baslangic.ToShortDateString() + " " + vardiya.Bitis.ToString("c"));

                                    if (vardiyaBaş > vardiyaBit)
                                        vardiyaBit = vardiyaBit.AddDays(1);

                                    if (vardiyaBaş > input.Baslangic)
                                    {
                                        vardiyaBaş = vardiyaBaş.AddDays(-1);
                                        vardiyaBit = vardiyaBit.AddDays(-1);
                                    }
                                      
                                    if ((input.Baslangic >= vardiyaBaş && input.Baslangic < vardiyaBit) || (input.Bitis <= vardiyaBit && input.Bitis > vardiyaBaş) || (input.Baslangic == vardiyaBaş && input.Bitis == vardiyaBit))
                                        listHata.Add("İstasyon (" + istasyon.Kod + ") Vardiya : " + vardiya.Kod);
                                }
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
                    }

                    if (lokasyonVarmı != null)
                    {
                        foreach (var istasyon in lokasyonVarmı.T3_Istasyon)
                        {
                            await db.T3_PlanliCalisma.AddAsync(new T3_PlanliCalisma
                            {
                                Id = Guid.NewGuid(),
                                IstasyonId = input.Id,
                                Baslangic = input.Baslangic,
                                Bitis = input.Bitis,
                                Kod = input.Aciklama,
                                Zaman = (int)((input.Bitis - input.Baslangic).TotalMinutes),
                                Istasyon = istasyon,
                               
                            });

                            await db.SaveChangesAsync();

                            x = new XReturn
                            {
                                Fonksiyon = fonksiyon,
                                Islem = true,
                                Kod = "İşlem Başarılı",
                                Mesaj = "Planlanmış Çalışma Ekleme Başarılı.",
                                Obje = lokasyonVarmı,
                            };

                            _logger.LogInformation("Zaman : {Zaman} , İşlem : {x}",
                                            DateTime.Now,
                                            XSabit.XSerialize(x));

                        }
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

            var varmı = await db.T3_PlanliCalisma.Where(p => p.Id == id)
                                                .Include(p => p.Istasyon)
                                                .FirstOrDefaultAsync();
            if (varmı != null)
            {
                db.T3_PlanliCalisma.Remove(varmı);
                await db.SaveChangesAsync();

                var liste = await db.T3_PlanliCalisma.Where(p => p.IstasyonId == varmı.IstasyonId).ToListAsync();

                List<object> list_PlanlıÇalışma = new List<object>();

                foreach (var item in liste)
                {
                    list_PlanlıÇalışma.Add(new
                    {
                        id = item.Id,
                        istasyon = varmı.Istasyon.Kod,
                        bas = item.Baslangic,
                        bit = item.Bitis,
                        kod = item.Kod,
                        zaman = item.Zaman,
                        buton = "<button style='max-height:50px;min-width:80px; height:50px' type='button' class='btn btn-danger btn-block' onclick=Plan_Sil('" + item.Id + "')><i class='bi bi-trash'></i></button>"
                    });
                }

                x = new XReturn
                {
                    Fonksiyon = fonksiyon,
                    Islem = true,
                    Kod = "İşlem Başarılı",
                    Mesaj = varmı.Kod + " başarılı şekilde silindi.",
                    Obje = list_PlanlıÇalışma,
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
