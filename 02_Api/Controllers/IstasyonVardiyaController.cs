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
    public class IstasyonVardiyaController : ControllerBase
    {
        private readonly ILogger<IstasyonVardiyaController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx04";

        public IstasyonVardiyaController(XModel context, ILogger<IstasyonVardiyaController> logger)
        {
            _logger = logger;
            db = context;
        }

        public class IstasyonVardiyaInput
        {
            public Guid VardiyaId { get; set; } = new Guid();
            public int Hedef { get; set; }
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


            try
            {
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "İstasyon Listesi Gönderildi.",
                    Obje = await db.T3_IstasyonVardiya.Where(p => p.IstasyonId == id && p.Aktif).Include(p => p.Vardiya).ToListAsync(),
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

        [HttpPost("{id}")]
        public async Task<XReturn> Post(Guid id, [FromBody] List<IstasyonVardiyaInput> input)
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
                var istasyon = await db.T3_Istasyon.Where(p => p.Id == id)
                                                   .Include(p => p.T3_IstasyonVardiya)
                                                   .Include(p => p.T3_PlanliCalisma)
                                                   .FirstOrDefaultAsync();

                var vardiyaList = await db.T3_Vardiya.Include(p => p.T3_VardiyaMola).ToListAsync();

                if (istasyon != null)
                {
                    if (input.Count == 0)
                    {
                        foreach (var aktifÇalışma in await db.T3_Calisma.Where(p => p.IstasyonId == istasyon.Id &&
                                                                                    p.Aktif).ToListAsync())
                        {
                            aktifÇalışma.Aktif = false;
                        }

                        await db.SaveChangesAsync();
                    }

                    foreach (var item in istasyon.T3_IstasyonVardiya)
                        item.Aktif = false;

                    List<string> listHata = new List<string>();

                    foreach (var item in input)
                    {
                        var vardiya = vardiyaList.Where(p => p.Id == item.VardiyaId).FirstOrDefault();

                        if (vardiya != null)
                            foreach (var calisma in istasyon.T3_PlanliCalisma.Where(p => p.Baslangic >= DateTime.Now))
                            {
                                var vardiyaBaş = DateTime.Parse(calisma.Baslangic.ToShortDateString() + " " + vardiya.Baslangic.ToString("c"));
                                var vardiyaBit = DateTime.Parse(calisma.Bitis.ToShortDateString() + " " + vardiya.Bitis.ToString("c"));

                                if (vardiyaBaş > calisma.Baslangic)
                                {
                                    calisma.Baslangic = calisma.Baslangic.AddDays(1);
                                    calisma.Bitis = calisma.Bitis.AddDays(1);
                                }

                                if (vardiyaBaş > vardiyaBit)
                                    vardiyaBit = vardiyaBit.AddDays(1);

                                if ((calisma.Baslangic >= vardiyaBaş && calisma.Baslangic < vardiyaBit) || (calisma.Bitis <= vardiyaBit && calisma.Bitis > vardiyaBaş) || (calisma.Baslangic == vardiyaBaş && calisma.Bitis == vardiyaBit))
                                    listHata.Add("Planlı Çalışma : " + vardiya.Kod + " ( " + calisma.Baslangic + " - " + calisma.Bitis + " )");
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


                    foreach (var item in input)
                    {
                        var vardiya = vardiyaList.Where(p => p.Id == item.VardiyaId).FirstOrDefault();

                        if (vardiya != null)
                        {
                            db.T3_IstasyonVardiya.Add(new T3_IstasyonVardiya
                            {
                                Id = Guid.NewGuid(),
                                VardiyaId = item.VardiyaId,
                                Aktif = true,
                                Hedef = item.Hedef,
                                IstasyonId = id,
                                Zaman = DateTime.Now,
                                Istasyon = istasyon,
                                Vardiya = vardiya,
                            });

                            var aktifÇalışma = await db.T3_Calisma.Where(p => p.Aktif && p.IstasyonId == id).FirstOrDefaultAsync();

                            if (aktifÇalışma != null)
                            {
                                var vardiyaBaşlangıcı = DateTime.Parse(DateTime.Now.ToShortDateString()).AddTicks(vardiya.Baslangic.Ticks);
                                var vardiyaBitiş = DateTime.Parse(DateTime.Now.ToShortDateString()).AddTicks(vardiya.Bitis.Ticks);

                                if (aktifÇalışma.Baslangic == vardiyaBaşlangıcı && aktifÇalışma.Bitis == vardiyaBitiş)
                                {
                                    aktifÇalışma.Hedef = item.Hedef;
                                    aktifÇalışma.Ortalama = ((vardiya.Bitis - vardiya.Baslangic).TotalSeconds - (vardiya.T3_VardiyaMola.Sum(p => p.Zaman) * 60)) / item.Hedef;
                                    await db.SaveChangesAsync();
                                } 
                            }
                        }
                    }
                }
                else
                {
                    var lokasyon = await db.T3_Lokasyon.Where(p => p.Id == id)
                                                       .Include(p => p.T3_Istasyon)
                                                       .ThenInclude(p => p.T3_IstasyonVardiya)
                                                       .Include(p => p.T3_Istasyon)
                                                       .ThenInclude(p => p.T3_PlanliCalisma)
                                                       .FirstOrDefaultAsync();
                    if (lokasyon != null)
                        foreach (var xistasyon in lokasyon.T3_Istasyon)
                        {
                            if (xistasyon != null)
                            {
                                foreach (var item in xistasyon.T3_IstasyonVardiya)
                                    item.Aktif = false;

                                List<string> listHata = new List<string>();

                                foreach (var item in input)
                                {
                                    var vardiya = vardiyaList.Where(p => p.Id == item.VardiyaId).FirstOrDefault();

                                    if (vardiya != null)
                                        foreach (var calisma in xistasyon.T3_PlanliCalisma.Where(p => p.Baslangic >= DateTime.Now))
                                        {
                                            var vardiyaBaş = DateTime.Parse(calisma.Baslangic.ToShortDateString() + " " + vardiya.Baslangic.ToString("c"));
                                            var vardiyaBit = DateTime.Parse(calisma.Bitis.ToShortDateString() + " " + vardiya.Bitis.ToString("c"));

                                            if (vardiyaBaş > calisma.Baslangic)
                                            {
                                                calisma.Baslangic = calisma.Baslangic.AddDays(1);
                                                calisma.Bitis = calisma.Bitis.AddDays(1);
                                            }

                                            if (vardiyaBaş > vardiyaBit)
                                                vardiyaBit = vardiyaBit.AddDays(1);

                                            if ((calisma.Baslangic >= vardiyaBaş && calisma.Baslangic < vardiyaBit) || (calisma.Bitis <= vardiyaBit && calisma.Bitis > vardiyaBaş) || (calisma.Baslangic == vardiyaBaş && calisma.Bitis == vardiyaBit))
                                                listHata.Add("Planlı Çalışma : " + vardiya.Kod + " ( " + calisma.Baslangic + " - " + calisma.Bitis + " )");
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

                                foreach (var item in input)
                                {
                                    var vardiya = vardiyaList.Where(p => p.Id == item.VardiyaId).FirstOrDefault();

                                    if (vardiya != null)
                                        db.T3_IstasyonVardiya.Add(new T3_IstasyonVardiya
                                        {
                                            Id = Guid.NewGuid(),
                                            VardiyaId = item.VardiyaId,
                                            Aktif = true,
                                            Hedef = item.Hedef,
                                            IstasyonId = id,
                                            Zaman = DateTime.Now,
                                            Istasyon = xistasyon,
                                            Vardiya = vardiya,
                                        });
                                }
                            }
                        }
                }

                await db.SaveChangesAsync();

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Liste Gönderildi.",
                    Obje = "",
                };
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

    }
}
