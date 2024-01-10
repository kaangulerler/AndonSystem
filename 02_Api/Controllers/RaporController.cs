using _01_DbModel;
using _01_DbModel.Db;
using _02_Api.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.X509Certificates;
using static _02_Api.Controllers.DurusController;

namespace _02_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RaporController : ControllerBase
    {
        private readonly ILogger<RaporController> _logger;

        private readonly XModel db;

        readonly string _fonksiyon = "Fx10";

        public RaporController(XModel context, ILogger<RaporController> logger)
        {
            _logger = logger;
            db = context;
        }

        public class Input_Vardiya
        {
            public List<Guid> Istasyon_List { get; set; } = new();
            public string Tarih { get; set; } = String.Empty;
        }
        [HttpPost("Vardiya")]
        public async Task<XReturn> PostVardiya([FromBody] Input_Vardiya input)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "02";

            x = new()
            {
                Islem = false,
                Fonksiyon = fonksiyon,
                Kod = "İşlem Başarısız",
                Mesaj = "Lokasyona oluştu",
                Obje = ""
            };

            try
            {
                 
                DateTime Başlangıç = TarihÇevir(input.Tarih.Split(" - ")[0]);
                DateTime Bitiş = TarihÇevir(input.Tarih.Split(" - ")[1]).AddDays(1);

                var nesne = await db.T3_Calisma.Where(p => input.Istasyon_List.Contains(p.IstasyonId) &&
                                                           (p.Baslangic >= Başlangıç &&
                                                            p.Bitis <= Bitiş))
                                                .Include(p => p.Istasyon)
                                                .OrderBy(p => p.Baslangic)
                                                .ThenBy(p => p.Istasyon.Kod)
                                                .ToListAsync();

                int gün_sayısı = (int)(Bitiş - Başlangıç).TotalDays;

                List<object> chart = new();

                if (gün_sayısı > 1)
                {
                    chart.Add(new object[]
                    {
                        "Gün",
                        "Target",
                        "Actual",
                        //"Delta",
                    });

                    for (int i = 0; i < gün_sayısı; i++)
                    {
                        string gün = Başlangıç.AddDays(i).ToShortDateString();
                        int target = nesne.Where(p => p.Baslangic >= Başlangıç.AddDays(i) && p.Bitis <= Başlangıç.AddDays((i + 1))).Sum(p => p.Hedef);
                        int actuel = nesne.Where(p => p.Baslangic >= Başlangıç.AddDays(i) && p.Bitis <= Başlangıç.AddDays((i + 1))).Sum(p => p.Aktuel);
                        //int delta = actuel - target;

                        chart.Add(new object[]
                        {
                            gün,
                            target,
                            actuel,
                            //delta
                        });
                    }
                }
                else
                {
                    chart.Add(new object[]
                    {
                        "Istasyon",
                        "Target",
                        "Actual",
                        //"Delta",
                    });

                    foreach (var calisma in nesne)
                    {
                        chart.Add(new object[]
                        {
                            calisma.Istasyon.Kod,
                            calisma.Hedef,
                            calisma.Aktuel,
                            //calisma.Delta,
                        });
                    }
                }


                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Vardiya raporu Gönderildi.",
                    Obje = new
                    {
                        Tablo = nesne,
                        Chart = chart
                    },
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

        public class Input_Uretim
        {
            public List<Guid> Istasyon_List { get; set; } = new();
            public string Tarih { get; set; } = String.Empty;
            public string Barkod { get; set; } = String.Empty;
            public string ProjectId { get; set; } = String.Empty;
            public List<string> PanelType { get; set; } = new List<string>();
            public List<int> PanelWidth { get; set; } = new List<int>();
            public List<int> PanelCurr { get; set; } = new List<int>();
            public List<float> ShortCurr { get; set; } = new List<float>();

        }
        public class Output_Uretim_Tablo
        {
            public Guid Id { get; set; }
            public string Istasyon { get; set; } = String.Empty;
            public string UretimKod { get; set; } = String.Empty;
            public TimeSpan SureHedef { get; set; }
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan SureGercek { get; set; }
            public List<UretimAltItem> ItemList { get; set; } = new();
        }
        public class UretimAltItem
        {
            public Guid UretimId { get; set; }
            public int Tip { get; set; }
            public string Kod { get; set; } = string.Empty;
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan Zaman { get; set; }
        }
        [HttpPost("Uretim")]
        public async Task<XReturn> PostUretim([FromBody] Input_Uretim input)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "03";

            try
            {
                if (input.PanelCurr.Count == 0)
                    input.PanelCurr = await db.T3_UrunTip.Select(p => p.Panel_Curr).Distinct().ToListAsync();

                if (input.PanelType.Count == 0)
                    input.PanelType = await db.T3_UrunTip.Select(p => p.Panel_Type).Distinct().ToListAsync();

                if (input.PanelWidth.Count == 0)
                    input.PanelWidth = await db.T3_UrunTip.Select(p => p.Panel_Width).Distinct().ToListAsync();

                if (input.ShortCurr.Count == 0)
                    input.ShortCurr = await db.T3_UrunTip.Select(p => p.Shortc_Curr).Distinct().ToListAsync();

                DateTime Başlangıç = TarihÇevir(input.Tarih.Split(" - ")[0]);
                DateTime Bitiş = TarihÇevir(input.Tarih.Split(" - ")[1]).AddDays(1);

                List<T3_Uretim> üretimList = new List<T3_Uretim>();

                int projeId = 0;

                if (input.ProjectId != string.Empty)
                    projeId = int.Parse(input.ProjectId);
                 
                üretimList = await db.T3_Uretim.Where(p => input.Istasyon_List.Contains(p.IstasyonId) &&
                                                           ((p.Baslangic >= Başlangıç && p.Baslangic <= Bitiş) || (p.Bitis >= Başlangıç && p.Bitis <= Bitiş)))
                                               .Include(p => p.Urun)
                                               .ThenInclude(p => p.Tip)
                                               .Include(p => p.Istasyon)
                                               .Include(p => p.T3_UretimPersonel)
                                               .ThenInclude(p => p.Personel)
                                               .Include(p => p.T3_UretimDurus)
                                               .ToListAsync();
                if (projeId > 0)
                    üretimList = üretimList.Where(p => p.Urun.ProjectId == projeId)
                                           .ToList();
                if (input.Barkod != "")
                    üretimList = üretimList.Where(p => p.Urun.Barkod == input.Barkod)
                                                      .ToList();

                üretimList = üretimList.Where(p => input.PanelCurr.Contains(p.Urun.Tip.Panel_Curr) &&
                                                           input.PanelType.Contains(p.Urun.Tip.Panel_Type) &&
                                                           input.PanelWidth.Contains(p.Urun.Tip.Panel_Width) &&
                                                           input.ShortCurr.Contains(p.Urun.Tip.Shortc_Curr))
                                                .ToList();

                List<Output_Uretim_Tablo> list = new();

                foreach (var item in üretimList)
                {
                    var ItemList = new List<UretimAltItem>();

                    foreach (var altitem in item.T3_UretimPersonel)
                        ItemList.Add(new UretimAltItem
                        {
                            Tip = 1,
                            UretimId = item.Id,
                            Baslangic = altitem.Baslangic,
                            Bitis = altitem.Bitis,
                            Kod = altitem.Personel.Ad + " " + altitem.Personel.Soyad,
                            Zaman = (altitem.Bitis - altitem.Baslangic)
                        });

                    foreach (var altitem in item.T3_UretimDurus)
                        ItemList.Add(new UretimAltItem
                        {
                            Tip = 2,
                            UretimId = item.Id,
                            Baslangic = altitem.Baslangic,
                            Bitis = altitem.Bitis,
                            Kod = altitem.Kod,
                            Zaman = (altitem.Bitis - altitem.Baslangic)
                        });

                    list.Add(new()
                    {
                        Id = item.Id,
                        Istasyon = item.Istasyon.Kod,
                        UretimKod = item.Kod,
                        Baslangic = item.Baslangic,
                        Bitis = item.Bitis,
                        SureHedef = new TimeSpan(0, item.SureHedef, 0),
                        SureGercek = new TimeSpan(0, 0, (int)item.T3_UretimPersonel.Sum(p => (p.Bitis - p.Baslangic).TotalSeconds)),
                        ItemList = ItemList
                    });
                }

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Üretim raporu Gönderildi.",
                    Obje = new
                    {
                        Tablo = list,
                    },
                };

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

        public class Input_Durus
        {
            public List<Guid> Istasyon_List { get; set; } = new();
            public List<Guid> Durus_List { get; set; } = new();
            public string Tarih { get; set; } = String.Empty;
        }
        public class Output_Durus_Tablo
        {
            public Guid Id { get; set; }
            public string Durus { get; set; } = String.Empty;
            public string Istasyon { get; set; } = String.Empty;
            public string UretimKod { get; set; } = String.Empty;
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan Zaman { get; set; }
        }
        [HttpPost("Durus")]
        public async Task<XReturn> PostDurus([FromBody] Input_Durus input)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "03";

            try
            {
                DateTime Başlangıç = TarihÇevir(input.Tarih.Split(" - ")[0]);
                DateTime Bitiş = TarihÇevir(input.Tarih.Split(" - ")[1]).AddDays(1);

                List<T3_UretimDurus> list = new List<T3_UretimDurus>();

                list = await db.T3_UretimDurus.Where(p => input.Istasyon_List.Contains(p.Uretim.IstasyonId) &&
                                                           (p.Baslangic >= Başlangıç &&
                                                            p.Bitis <= Bitiş) &&
                                                           input.Durus_List.Contains(p.DurusTipId))
                                                .Include(p => p.Uretim)
                                                .ThenInclude(p => p.Istasyon)
                                                .OrderBy(p => p.Baslangic)
                                                .ToListAsync();



                List<Output_Durus_Tablo> tablo = new();

                foreach (var item in list)
                {
                    var bitiş = item.Bitis;

                    if (bitiş <= item.Baslangic)
                        bitiş = DateTime.Now;

                    tablo.Add(new()
                    {
                        Durus = item.Kod,
                        Istasyon = item.Uretim.Istasyon.Kod,
                        UretimKod = item.Uretim.Kod,
                        Baslangic = item.Baslangic,
                        Bitis = bitiş,
                        Zaman = (bitiş - item.Baslangic)
                    });
                }

                foreach(var item in await db.T3_IstasyonDurus.Where(p => input.Istasyon_List.Contains(p.IstasyonId) && (p.Baslangic >= Başlangıç && p.Bitis <= Bitiş) &&
                                                                           input.Durus_List.Contains(p.DurusTipId))
                                                            .Include(p=>p.Istasyon)
                                                            .ToListAsync())
                {
                    var bitiş = item.Bitis;

                    if (bitiş <= item.Baslangic)
                        bitiş = DateTime.Now;

                    tablo.Add(new()
                    {
                        Durus = item.Kod,
                        Istasyon = item.Istasyon.Kod,
                        UretimKod = "PANO YOK",
                        Baslangic = item.Baslangic,
                        Bitis = bitiş,
                        Zaman = (bitiş - item.Baslangic)
                    });
                }

                var chart_tip = tablo.GroupBy(p => p.Durus).Select(g => new
                {
                    Durus = g.Key,
                    Toplam = TimeSpan.FromTicks(g.Sum(s => s.Zaman.Ticks))
                })
                .ToList();

                List<object[]> Pie_List = new();

                long toplam_Durus = chart_tip.Sum(p => p.Toplam.Ticks);
                double oran = 100.0 / toplam_Durus;

                foreach (var durus in chart_tip)
                {
                    double yuzde = durus.Toplam.Ticks * oran;

                    Pie_List.Add(new object[]
                        {
                        durus.Durus + "\n % " + Math.Round(yuzde, 2),
                        durus.Toplam.Ticks,
                        pasta_Tooltip(durus.Durus, "a", (int)durus.Toplam.TotalSeconds , "danger")
                        }); 
                }



                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Duruş raporu Gönderildi.",
                    Obje = new
                    {
                        Tablo = tablo,
                        Chart = Pie_List
                    },
                };

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

        public class Input_Operator
        {
            public List<Guid> Istasyon_List { get; set; } = new();
            public List<Guid> Personel_List { get; set; } = new();
            public string Barkod { get; set; } = String.Empty;
            public string ProjeId { get; set; } = String.Empty;
            public string Tarih { get; set; } = String.Empty;
        }
        public class Output_Operator_Tablo
        {
            public Guid Id { get; set; }
            public string Personel { get; set; } = String.Empty;
            public string Istasyon { get; set; } = String.Empty;
            public int ProjectId { get; set; }
            public string UretimKod { get; set; } = String.Empty;
            public DateTime Baslangic { get; set; }
            public DateTime Bitis { get; set; }
            public TimeSpan Zaman { get; set; }
        }
        [HttpPost("Operator")]
        public async Task<XReturn> PostOperator([FromBody] Input_Operator input)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "02";

            x = new()
            {
                Islem = false,
                Fonksiyon = fonksiyon,
                Kod = "İşlem Başarısız",
                Mesaj = "Lokasyona oluştu",
                Obje = ""
            };

            try
            {
                DateTime Başlangıç = TarihÇevir(input.Tarih.Split(" - ")[0]);
                DateTime Bitiş = TarihÇevir(input.Tarih.Split(" - ")[1]).AddDays(1);

                var nesne = await db.T3_UretimPersonel.Where(p => input.Istasyon_List.Contains(p.Uretim.IstasyonId) && (p.Baslangic >= Başlangıç && p.Bitis <= Bitiş))
                                                       .Include(p => p.Uretim)
                                                       .ThenInclude(p => p.Istasyon)
                                                       .Include(p => p.Uretim)
                                                       .ThenInclude(p => p.Urun)
                                                       .Include(p => p.Personel)
                                                       .OrderBy(p => p.Baslangic)
                                                       .ToListAsync();
                if (input.ProjeId != string.Empty)
                    nesne = nesne.Where(p => p.Uretim.Urun.ProjectId == int.Parse(input.ProjeId)).ToList();

                if (input.Barkod != string.Empty)
                    nesne = nesne.Where(p => p.Uretim.Barkod == input.Barkod).ToList();

                List<Output_Operator_Tablo> tablo = new();

                foreach (var item in nesne)
                    tablo.Add(new Output_Operator_Tablo
                    {
                        Id = item.Id,
                        Istasyon = item.Uretim.Istasyon.Kod,
                        Personel = item.Personel.Ad + " " + item.Personel.Soyad,
                        ProjectId = item.Uretim.Urun.ProjectId,
                        UretimKod = item.Uretim.Kod,
                        Baslangic = item.Baslangic,
                        Bitis = item.Bitis,
                        Zaman = (item.Bitis - item.Baslangic)
                    });

                var chart_tip = tablo.GroupBy(p => p.Istasyon).Select(g => new
                {
                    Kod = g.Key,
                    Toplam = TimeSpan.FromTicks(g.Sum(s => s.Zaman.Ticks))
                })
                .ToList();

                List<object[]> Pie_Istasyon_List = new();

                long toplam_Durus = chart_tip.Sum(p => p.Toplam.Ticks);
                double oran = 100.0 / toplam_Durus;

                foreach (var item in chart_tip)
                {
                    double yuzde = item.Toplam.Ticks * oran;

                    Pie_Istasyon_List.Add(new object[]
                        {
                        item.Kod + "\n % " + Math.Round(yuzde, 2),
                        item.Toplam.Ticks,
                        pasta_Tooltip(item.Kod, "a", (int)item.Toplam.TotalSeconds , "success")
                        });
                }

                var chart_tip_personel = tablo.GroupBy(p => p.Personel).Select(g => new
                {
                    Kod = g.Key,
                    Toplam = TimeSpan.FromTicks(g.Sum(s => s.Zaman.Ticks))
                })
                .ToList();

                List<object[]> Pie_Personel_List = new();

                toplam_Durus = chart_tip_personel.Sum(p => p.Toplam.Ticks);
                oran = 100.0 / toplam_Durus;

                foreach (var item in chart_tip_personel)
                {
                    double yuzde = item.Toplam.Ticks * oran;

                    Pie_Personel_List.Add(new object[]
                        {
                        item.Kod + "\n % " + Math.Round(yuzde, 2),
                        item.Toplam.Ticks,
                        pasta_Tooltip(item.Kod, "a", (int)item.Toplam.TotalSeconds , "success")
                        });
                }

                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Vardiya raporu Gönderildi.",
                    Obje = new
                    {
                        Tablo = tablo,
                        Chart_Istasyon = Pie_Istasyon_List,
                        Chart_Personel = Pie_Personel_List
                    },
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
                    Mesaj = "Lokasyona oluştu",
                    Obje = ex.ToString(),
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        (x));
            }

            return x;
        }

        public class Input_Kull
        {
            public List<Guid> Istasyon_List { get; set; } = new();
            public List<Guid> Personel_List { get; set; } = new();
            public string Barkod { get; set; } = String.Empty;
            public string ProjeId { get; set; } = String.Empty;
            public string Tarih { get; set; } = String.Empty;
        }
        public class Output_Kull_Tablo
        {
            public Guid Id { get; set; }
            public string Tarih { get; set; } = String.Empty;
            public string Istasyon { get; set; } = String.Empty;
            public TimeSpan Planlanan { get; set; }
            public TimeSpan UretimToplam { get; set; }
            public double Kullanılabilirlik { get; set; }
        }
        
        [HttpPost("Kullanilabilirlik")]
        public async Task<XReturn> PostKullanilabilirlik([FromBody] Input_Operator input)
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "02";

            x = new()
            {
                Islem = false,
                Fonksiyon = fonksiyon,
                Kod = "İşlem Başarısız",
                Mesaj = "Lokasyona oluştu",
                Obje = ""
            };

            try
            {
                DateTime Başlangıç = TarihÇevir(input.Tarih.Split(" - ")[0]);
                DateTime Bitiş = TarihÇevir(input.Tarih.Split(" - ")[1]).AddDays(1);

                var nesne = await db.T3_Istasyon.Where(p => input.Istasyon_List.Contains(p.Id))
                                                .Include(p => p.T3_Calisma.Where(p => p.Baslangic >= Başlangıç && p.Bitis <= Bitiş))
                                                .Include(p => p.T3_Uretim.Where(p => p.Baslangic >= Başlangıç && p.Bitis <= Bitiş))
                                                .ThenInclude(p => p.T3_UretimPersonel)
                                                .Include(p=>p.T3_Calisma)
                                                .ThenInclude(p=>p.T3_CalismaPlanliDurus)
                                                .ToListAsync();

                List<Output_Kull_Tablo> tablo = new();

                foreach (var item in nesne)
                {
                    foreach (var çalışma in item.T3_Calisma)
                    {
                        var gun = çalışma.Baslangic.ToShortDateString();

                        var varmı = tablo.Where(p => p.Tarih == gun && p.Istasyon == item.Kod)
                                        .FirstOrDefault();
                        if (varmı == null)
                        {
                            var uretimToplam = item.T3_Uretim.Select(p => p.T3_UretimPersonel.Where(p => p.Baslangic.ToShortDateString() == gun)).Sum(p => p.Sum(p => (p.Bitis - p.Baslangic).Ticks));

                            var planlidurustoplam = çalışma.T3_CalismaPlanliDurus.Select(p => (p.Bitis - p.Baslangic).Ticks).Sum(p=>p);

                            var planlanan = (çalışma.Bitis - çalışma.Baslangic).Ticks - planlidurustoplam;
                            
                            double kullanılabilirlik = (uretimToplam * 100) / planlanan;

                            tablo.Add(new Output_Kull_Tablo
                            {
                                Id = Guid.NewGuid(),
                                Istasyon = item.Kod,
                                Tarih = gun,
                                Planlanan = TimeSpan.FromTicks(planlanan),
                                UretimToplam = TimeSpan.FromTicks(uretimToplam),
                                Kullanılabilirlik = kullanılabilirlik,
                            });
                        }
                    }
                }



                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Kullanılabilirlik Raporu Gönderildi.",
                    Obje = new
                    {
                        Tablo = tablo.OrderBy(p => p.Tarih).ThenBy(p => p.Istasyon),
                        //Chart = chart
                    },
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
                    Mesaj = "Lokasyona oluştu",
                    Obje = ex.ToString(),
                };
                _logger.LogError("Zaman : {Zaman} , İşlem : {x}",
                                        DateTime.Now,
                                        (x));
            }

            return x;
        }

        [HttpGet("Filtre")]
        public async Task<XReturn> GetFiltre()
        {
            XReturn x;
            string fonksiyon = _fonksiyon + "00";

            try
            {
                var tip = await db.T3_UrunTip.ToListAsync();
                var personel = await db.T3_Personel.ToListAsync();

                var istasyon = await db.T3_Lokasyon
                                                .Include(p => p.T3_Istasyon)
                                                .ThenInclude(p => p.T3_IstasyonVardiya.Where(p => p.Aktif))
                                                .Include(p => p.T3_Istasyon)
                                                .ThenInclude(p => p.T3_PlanliCalisma)
                                                .OrderBy(p => p.Kod).ToListAsync();

                istasyon = istasyon.ToList();

                var durusTip = await db.T3_DurusTip.ToListAsync();

                var rated_volt = tip.Select(p => p.Rated_Volt).Distinct();
                var panel_width = tip.Select(p => p.Panel_Width).Distinct();
                var panel_type = tip.Where(p => p.Panel_Type != " " && p.Panel_Type != "").Select(p => p.Panel_Type).Distinct();
                var panel_curr = tip.Select(p => p.Panel_Curr).Distinct();
                var short_curr = tip.Select(p => p.Shortc_Curr).Distinct();
                 
                x = new()
                {
                    Islem = true,
                    Fonksiyon = fonksiyon,
                    Kod = "İşlem Başarılı",
                    Mesaj = "Vardiya raporu Gönderildi.",
                    Obje = new
                    {
                        //Urun = await db.T3_Urun.ToListAsync(),
                        Tip = await db.T3_UrunTip.ToListAsync(),
                        Durus = await db.T3_DurusTip.OrderBy(p => p.Kod).ToListAsync(),
                        Personel = await db.T3_Personel.ToListAsync(),
                        Istasyon = istasyon,
                        DurusTip = durusTip,
                        RatedVolt = rated_volt,
                        PanelWidth = panel_width,
                        PanelType = panel_type,
                        PanelCurr = panel_curr,
                        ShortCurr = short_curr,
                    },
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

        string pasta_Tooltip(string başlık, string açıklama, int saniye, string tema)
        {
            string dönen = "";

            TimeSpan time = TimeSpan.FromSeconds(saniye);
            string zaman = time.ToString(@"hh\:mm\:ss");

            dönen = "<div class='card text-bg-" + tema + "' style='width: 8rem;'> " +
                        "<div class='card-header'>" + başlık + "</div>" +
                        "<div class='card-footer text-bg-light'>" + zaman + "</div>" +
                    "</div>";
            return dönen;
        }

        DateTime TarihÇevir(string tarih)
        {
            var dönen = new DateTime();
            string[] dizi = tarih.Split('.');
            int yıl = int.Parse(dizi[2]);
            int ay = int.Parse(dizi[1]);
            int gun = int.Parse(dizi[0]);
             
            dönen = new DateTime(yıl, ay, gun);
            
            return dönen;
        }
    }
}
