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
    public class ClientController : ControllerBase
    {
        private readonly ILogger<ClientController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx10";

        public ClientController(XModel context, ILogger<ClientController> logger)
        {
            _logger = logger;
            db = context;
        }


        [HttpGet("IstasyonList")]
        public async Task<XReturn> GetIstasyonList()
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "02";

            try
            {
                var nesne = await db.T3_Lokasyon.Where(p => p.Kod != "MWS")
                                                .Include(p => p.T3_Istasyon)
                                                .OrderBy(p => p.Kod)
                                                .ToListAsync();

                List<object> list = new List<object>();

                foreach (var lok in nesne)
                    foreach (var ist in lok.T3_Istasyon)
                        list.Add(new
                        {
                            Id = ist.Id,
                            Kod = ist.Kod,
                            Istasyon = lok.Kod
                        });

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Istasyon Listesi Gönderildi.",
                    Obje = list,
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

        [HttpGet("Tabela/{id}")]
        public string GetTabela(string id)
        {
            var dizi = id.Split(",");
            string result = "";

            List<T3_Istasyon> istasyon_List = db.T3_Istasyon.Where(p => dizi.Contains(p.SiraNo.ToString()))
                                                            .Include(p => p.T3_Calisma.Where(x => x.Aktif))
                                                            .ToList(); 
            int snSorgu = 0;
            int reset = 0;
            string deger = "";
            bool resetVarMı = false;

            for (int i = 0; i < 3; i++)
            {
                if (i < dizi.Length)
                {
                    var istasyonVarmı = istasyon_List.Where(p => p.SiraNo == int.Parse(dizi[i])).FirstOrDefault();

                    if (istasyonVarmı != null)
                    {
                        snSorgu = istasyonVarmı.SnSorgu;
                        
                        reset = istasyonVarmı.Reset;

                        var çalışmaVarmı = istasyonVarmı.T3_Calisma.FirstOrDefault();

                        if (çalışmaVarmı != null)
                        {
                            string hedef = "," + çalışmaVarmı.Hedef.ToString().PadLeft(4, '}');  
                            string aktuel = çalışmaVarmı.Aktuel.ToString().PadLeft(4, '}');
                            string delta = çalışmaVarmı.Delta.ToString().PadLeft(4, '}');
                            
                            deger += hedef + aktuel + delta;
                        }
                        else
                            deger += ",}}}0}}}0}}}0";

                         
                        if (reset > 0)
                        {
                            istasyonVarmı.Reset = 0;
                            resetVarMı = true; 
                        }

                        db.T3_Tabela.RemoveRange(db.T3_Tabela.Where(p => p.Istasyon == int.Parse(dizi[i])));

                        db.T3_Tabela.Add(new T3_Tabela
                        {
                            Zaman = DateTime.Now,
                            Istasyon = int.Parse(dizi[i])
                        });

                        db.SaveChanges();
                    }
                    else
                        deger += ",}}}0}}}0}}}0";
                }
                else
                    deger += ",}}}0}}}0}}}0";
            }

            if (resetVarMı)
                db.SaveChanges();

            result = reset.ToString("D1") + "," + snSorgu.ToString("D2") + deger;
            result = "[" + result + "]";
            return result;
        }

        [HttpGet("Uretim/{id}")]
        public List<int> GetUretim(string id)
        {
            var zaman = DateTime.Now;

            int dönen = 0;

            var dizi = id.Split(",");

        tekrar:

            try
            {
                var istasyon = db.T3_Istasyon.Where(p => p.SiraNo == int.Parse(dizi[0]))
                                                 .Include(p => p.T3_Calisma.Where(x => x.Aktif))
                                                 .Include(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                 .ThenInclude(p => p.Vardiya)
                                                 .Include(p => p.T3_PlanliCalisma.Where(p => p.Baslangic > zaman).OrderBy(p => p.Baslangic))
                                                 .FirstOrDefault();
                if (istasyon != null)
                {
                    var çalışma = istasyon.T3_Calisma.FirstOrDefault();

                    if (çalışma != null)
                    {
                        Guid üretimId = Guid.NewGuid();
                         
                        db.T3_Uretim.Add(new T3_Uretim
                        {
                            Id = üretimId,
                            UrunId = new Guid(),
                            Barkod = "MWS",
                            Kod = "MWS",
                            IstasyonId = istasyon.Id,
                            Baslangic = zaman.AddSeconds(-1),
                            Bitis = zaman,
                            BitisHedef = zaman,
                            SureGercek = 0,
                            SureHedef = 0,
                            SureHedefG = 0,
                            Miktar = int.Parse(dizi[1]),
                        });

                        db.T3_UretimCalisma.Add(new T3_UretimCalisma
                        {
                            Id = Guid.NewGuid(),
                            CalismaId = çalışma.Id,
                            UretimId = üretimId
                        });

                        db.SaveChanges();
                        dönen = 1;
                    }
                    else
                    {
                        DateTime bitişZamanı = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

                        if (istasyon.T3_IstasyonVardiya != null)
                        {
                            T3_Vardiya vardiya = new T3_Vardiya
                            {
                                Id = new Guid(),
                            };

                            foreach (var vard in istasyon.T3_IstasyonVardiya.Where(p => p.Aktif).OrderBy(p => p.Vardiya.Baslangic))
                            {
                                var vardiyaBaşlangıç = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + vard.Vardiya.Baslangic.ToString("c"));
                                if (bitişZamanı > vardiyaBaşlangıç)
                                    bitişZamanı = vardiyaBaşlangıç;
                            }
                        }

                        if (istasyon.T3_PlanliCalisma != null)
                        {
                            var ilkPlanlıÇalışma = istasyon.T3_PlanliCalisma.FirstOrDefault();

                            if (ilkPlanlıÇalışma != null)
                                if (ilkPlanlıÇalışma.Baslangic.ToShortDateString() == zaman.ToShortDateString())
                                    if (bitişZamanı > ilkPlanlıÇalışma.Baslangic)
                                        bitişZamanı = ilkPlanlıÇalışma.Baslangic;
                        }

                        db.T3_Calisma.Add(new T3_Calisma
                        {
                            Id = Guid.NewGuid(),
                            Kod = "X",
                            Baslangic = zaman,
                            Bitis = bitişZamanı,
                            Aktif = true,
                            IstasyonId = istasyon.Id,
                            Hedef = 0,
                            Aktuel = 0,
                            Delta = 0,
                            Ortalama = 0,
                        });

                        db.SaveChanges();
                        goto tekrar;
                    }
                }
            }
            catch
            {
                dönen = 0;
            }

            return new List<int> { dönen };
        }

        [HttpGet("UretimCoklu/{gelen_mesaj}")]
        public List<int> GetUretimCoklu(string gelen_mesaj)
        {
            var zaman = DateTime.Now;

            List<int> result = new ();

            var dizi = gelen_mesaj.Split(",");


            for (int i = 0; i < 4; i++)
            {

        tekrar:
                int dönen = 0;
                try
                {
                    var istasyon = db.T3_Istasyon.Where(p => p.SiraNo == int.Parse(dizi[(i * 2)]))
                                                     .Include(p => p.T3_Calisma.Where(x => x.Aktif))
                                                     .Include(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                     .ThenInclude(p => p.Vardiya)
                                                     .Include(p => p.T3_PlanliCalisma.Where(p => p.Baslangic > zaman).OrderBy(p => p.Baslangic))
                                                     .FirstOrDefault();
                    if (istasyon != null)
                    {
                        var çalışma = istasyon.T3_Calisma.FirstOrDefault();

                        if (çalışma != null)
                        {
                            int miktar = int.Parse(dizi[(i * 2) + 1]);

                            if (miktar > 0)
                            {
                                 
                                Guid üretimId = Guid.NewGuid();

                                db.T3_Uretim.Add(new T3_Uretim
                                {
                                    Id = üretimId,
                                    UrunId = new Guid(),
                                    Barkod = "MWS",
                                    Kod = "MWS",
                                    IstasyonId = istasyon.Id,
                                    Baslangic = zaman.AddSeconds(-1),
                                    Bitis = zaman,
                                    BitisHedef = zaman,
                                    SureGercek = 0,
                                    SureHedef = 0,
                                    SureHedefG = 0,
                                    Miktar = miktar,
                                });

                                db.T3_UretimCalisma.Add(new T3_UretimCalisma
                                {
                                    Id = Guid.NewGuid(),
                                    CalismaId = çalışma.Id,
                                    UretimId = üretimId
                                });
                                db.SaveChanges();

                                dönen = miktar;
                            } 
                        }
                        else
                        {
                            DateTime bitişZamanı = DateTime.Parse(DateTime.Now.ToShortDateString() + " 23:59:59");

                            if (istasyon.T3_IstasyonVardiya != null)
                            {
                                T3_Vardiya vardiya = new T3_Vardiya
                                {
                                    Id = new Guid(),
                                };

                                foreach (var vard in istasyon.T3_IstasyonVardiya.Where(p => p.Aktif).OrderBy(p => p.Vardiya.Baslangic))
                                {
                                    var vardiyaBaşlangıç = DateTime.Parse(DateTime.Now.ToShortDateString() + " " + vard.Vardiya.Baslangic.ToString("c"));
                                    if (bitişZamanı > vardiyaBaşlangıç)
                                        bitişZamanı = vardiyaBaşlangıç;

                                }
                            }

                            if (istasyon.T3_PlanliCalisma != null)
                            {
                                var ilkPlanlıÇalışma = istasyon.T3_PlanliCalisma.FirstOrDefault();

                                if (ilkPlanlıÇalışma != null)
                                    if (ilkPlanlıÇalışma.Baslangic.ToShortDateString() == zaman.ToShortDateString())
                                        if (bitişZamanı > ilkPlanlıÇalışma.Baslangic)
                                            bitişZamanı = ilkPlanlıÇalışma.Baslangic;
                            }

                            if (bitişZamanı < zaman)
                                bitişZamanı.AddDays(1);

                            db.T3_Calisma.Add(new T3_Calisma
                            {
                                Id = Guid.NewGuid(),
                                Kod = "X",
                                Baslangic = zaman,
                                Bitis = bitişZamanı,
                                Aktif = true,
                                IstasyonId = istasyon.Id,
                                Hedef = 0,
                                Aktuel = 0,
                                Delta = 0,
                                Ortalama = 0,
                            });

                            db.SaveChanges();
                            goto tekrar;
                        }
                    }
                }
                catch
                {
                    dönen = 0;
                }

                result.Add(dönen);

            }

            return result;
        }

        [HttpGet("Ayar/")]
        public int GetAyar()
        {
            int dönen = 30;

            return dönen;
        }
    }
}
