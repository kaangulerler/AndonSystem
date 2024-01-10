using _01_DbModel.AppModel;
using _01_DbModel.Db;
using _02_Api.Controllers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _02_Api.Hubs
{
    public class SoketHub : Hub
    {
        private readonly XModel db;
        private readonly ILogger<SoketHub> _logger;

        public SoketHub(XModel context, ILogger<SoketHub> logger)
        {
            db = context;
            _logger = logger;
        }

        public async Task Send(string gonderici, string alici, string message)
        {
            if (message != "Online")
            {
                try
                {
                    T3_Mesaj? mesaj = JsonConvert.DeserializeObject<T3_Mesaj>(message);
                    if (mesaj != null)
                    {
                        if (mesaj.Fonksiyon == 2)
                        {

                        }


                        if (alici == "Servis")
                        {
                            await MesajGeldi(gonderici, message);
                        }
                        else
                        {
                            await Clients.All.SendAsync(alici, gonderici, message);

                        }
                    }
                }
                catch { }
            }
            else
                await Clients.All.SendAsync(alici, gonderici, message);

            if (gonderici == "Servis" && alici == "dashboard")
            {
                await Clients.All.SendAsync(alici, gonderici, message);

            }
        }

        public async Task MesajGeldi(string id, string mesaj)
        {
            var gelen_Mesaj = JsonConvert.DeserializeObject<T3_Mesaj>(mesaj);
            DateTime zaman = DateTime.Now;

            var giden_Mesaj = new T3_Mesaj();

            if (gelen_Mesaj != null)
            {
                switch (gelen_Mesaj.Fonksiyon)
                {
                    case 1:
                        {
                            string barkod = gelen_Mesaj.Nesne;
                            switch (barkod.Substring(0, 2))
                            {
                                case "F0":
                                    {
                                        var personel = await db.T3_Personel.Where(p => p.Barkod == barkod)
                                                                           .FirstOrDefaultAsync();
                                        if (personel != null)
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = true,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Personel Barkodu",
                                                    Mesaj = personel.Ad + " " + personel.Soyad + " personel barkodu okutuldu.",
                                                    PanelTip = 0,
                                                    Barkod = barkod,
                                                    BarkodTip = 1,
                                                }),
                                                Tip = true,
                                            };
                                        else
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = false,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Personel Barkodu",
                                                    Mesaj = "Barkod tanımlı değil.",
                                                    PanelTip = 1,
                                                    Barkod = barkod,
                                                    BarkodTip = 1,
                                                }),
                                                Tip = true,
                                            };

                                        await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));

                                        break;
                                    }
                                case "F2":
                                    {
                                        var pano = await db.T3_Urun.Where(p => p.Barkod == barkod)
                                                                   .FirstOrDefaultAsync();
                                        if (pano != null)
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = true,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Pano Barkodu",
                                                    Mesaj = pano.Switchgear + pano.Panel_No + " pano barkodu okutuldu.",
                                                    PanelTip = 0,
                                                    Barkod = barkod,
                                                    BarkodTip = 2,
                                                }),
                                                Tip = true,
                                            };
                                        else
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = false,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Pano Barkodu",
                                                    Mesaj = "Barkod tanımlı değil.",
                                                    PanelTip = 1,
                                                    Barkod = barkod,
                                                    BarkodTip = 2,
                                                }),
                                                Tip = true,
                                            };

                                        await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));

                                        break;
                                    }
                                case "F3":
                                    {
                                        var duruş = await db.T3_DurusTip.Where(p => p.Barkod == barkod)
                                                                        .FirstOrDefaultAsync();
                                        if (duruş != null)
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = true,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Duruş Barkodu",
                                                    Mesaj = duruş.Kod + " duruş barkodu okutuldu.",
                                                    PanelTip = 0,
                                                    Barkod = barkod,
                                                    BarkodTip = 3,
                                                }),
                                                Tip = true,
                                            };
                                        else
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = false,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Duruş Barkodu",
                                                    Mesaj = "Barkod tanımlı değil.",
                                                    PanelTip = 1,
                                                    Barkod = barkod,
                                                    BarkodTip = 3,
                                                }),
                                                Tip = true,
                                            };

                                        await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));

                                        break;
                                    }
                            }

                            break;
                        }
                    case 2:
                        {
                            var input = JsonConvert.DeserializeObject<InputModel>(gelen_Mesaj.Nesne);
                            if (input != null)
                            {
                                var ürün = await db.T3_Urun.Where(p => p.Barkod == input.Pano)
                                                           .FirstOrDefaultAsync();

                                var durus_Tip = await db.T3_DurusTip.Where(p => p.Barkod == input.Durus)
                                                                        .FirstOrDefaultAsync();
                                if (durus_Tip != null)
                                {
                                    if (ürün != null)
                                    {

                                        var üretim_VarMı = await db.T3_Uretim.Where(p => p.IstasyonId == Guid.Parse(input.Rulo) &&
                                                                                         p.UrunId == ürün.Id &&
                                                                                        (p.Baslangic == p.Bitis))
                                                                             .Include(p => p.T3_UretimDurus)
                                                                             .FirstOrDefaultAsync();
                                        if (üretim_VarMı != null)
                                        {
                                            var durus = üretim_VarMı.T3_UretimDurus.Where(p => p.DurusTipId == durus_Tip.Id &&
                                                                                               p.Baslangic == p.Bitis)
                                                                                   .FirstOrDefault();
                                            if (durus == null)
                                            {
                                                var yeni_durus = new T3_UretimDurus
                                                {
                                                    Id = Guid.NewGuid(),
                                                    DurusTipId = durus_Tip.Id,
                                                    Kod = durus_Tip.Kod,
                                                    UretimId = üretim_VarMı.Id,
                                                    Zaman = 0,
                                                    Baslangic = zaman,
                                                    Bitis = zaman,
                                                    DurusTip = durus_Tip.Kod,
                                                };

                                                await db.T3_UretimDurus.AddAsync(yeni_durus);

                                                try
                                                {
                                                    await db.SaveChangesAsync();
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logger.LogInformation(ex.Message);
                                                }

                                            }
                                            else
                                            {
                                                int sn = (int)(zaman - durus.Baslangic).TotalSeconds;

                                                if (sn > 4)
                                                {
                                                    durus.Bitis = zaman;
                                                    durus.Zaman = sn;

                                                    try
                                                    {
                                                        await db.SaveChangesAsync();

                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _logger.LogInformation(ex.Message);
                                                    }
                                                }
                                            }
                                        }

                                    }
                                    else
                                    {
                                        var üretim_Listesi = await db.T3_Uretim.Where(p => p.IstasyonId == Guid.Parse(input.Rulo) &&
                                                                                           (p.Baslangic == p.Bitis))
                                                                                 .Include(p => p.T3_UretimDurus)
                                                                                 .ToListAsync();

                                        if (üretim_Listesi.Count == 1)
                                        {
                                            var üretim_VarMı = üretim_Listesi.FirstOrDefault();
                                            if (üretim_VarMı != null)
                                            {
                                                var durus = üretim_VarMı.T3_UretimDurus.Where(p => p.DurusTipId == durus_Tip.Id &&
                                                                                                   p.Baslangic == p.Bitis)
                                                                                       .FirstOrDefault();
                                                if (durus == null)
                                                {
                                                    var yeni_durus = new T3_UretimDurus
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        DurusTipId = durus_Tip.Id,
                                                        Kod = durus_Tip.Kod,
                                                        UretimId = üretim_VarMı.Id,
                                                        Zaman = 0,
                                                        Baslangic = zaman,
                                                        Bitis = zaman,
                                                        DurusTip = durus_Tip.Kod,
                                                    };

                                                    await db.T3_UretimDurus.AddAsync(yeni_durus);

                                                    try
                                                    {
                                                        await db.SaveChangesAsync();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _logger.LogInformation(ex.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    int sn = (int)(zaman - durus.Baslangic).TotalSeconds;

                                                    if (sn > 4)
                                                    {
                                                        durus.Bitis = zaman;
                                                        durus.Zaman = sn;

                                                        try
                                                        {
                                                            await db.SaveChangesAsync();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            _logger.LogInformation(ex.Message);
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        if (üretim_Listesi.Count > 1)
                                        {
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = false,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Hata",
                                                    Mesaj = "Lütfen önce Pano barkodunu okutun.",
                                                    PanelTip = 1,
                                                    Barkod = "",
                                                    BarkodTip = 9,
                                                }),
                                                Tip = true,
                                            };
                                            await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));
                                        }

                                        if (üretim_Listesi.Count == 0)
                                        {
                                            var istasyonDuruşuVarmı = await db.T3_IstasyonDurus.Where(p => p.IstasyonId == Guid.Parse(input.Rulo) &&
                                                                                                           p.DurusTipId == durus_Tip.Id &&
                                                                                                           p.Baslangic == p.Bitis)
                                                                                               .FirstOrDefaultAsync();
                                            if (istasyonDuruşuVarmı == null)
                                            {
                                                var yeniIstasyonDuruş = new T3_IstasyonDurus
                                                {
                                                    Id = Guid.NewGuid(),
                                                    IstasyonId = Guid.Parse(input.Rulo),
                                                    DurusTipId = durus_Tip.Id,
                                                    DurusTip = durus_Tip.Kod,
                                                    Baslangic = zaman,
                                                    Bitis = zaman,
                                                    Kod = durus_Tip.Kod,
                                                    Zaman = 0,
                                                };

                                                await db.T3_IstasyonDurus.AddAsync(yeniIstasyonDuruş);
                                                await db.SaveChangesAsync();
                                            }
                                            else
                                            {
                                                int sn = (int)(zaman - istasyonDuruşuVarmı.Baslangic).TotalSeconds;

                                                if (sn > 4)
                                                {
                                                    istasyonDuruşuVarmı.Bitis = zaman;
                                                    istasyonDuruşuVarmı.Zaman = (int)(zaman - istasyonDuruşuVarmı.Baslangic).TotalSeconds;
                                                    await db.SaveChangesAsync();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 7:
                        {
                            var input = JsonConvert.DeserializeObject<InputModel>(gelen_Mesaj.Nesne);
                            if (input != null)
                            {

                                var personel_Çalışma = await db.T3_Personel.Where(p => p.Barkod == input.Personel)
                                                                           .Include(p => p.T3_UretimPersonel.Where(p => p.Baslangic == p.Bitis))
                                                                           .FirstOrDefaultAsync();
                                if (personel_Çalışma != null)
                                {
                                    foreach (var çalışma in personel_Çalışma.T3_UretimPersonel)
                                    {
                                        çalışma.Bitis = zaman;
                                        çalışma.Zaman = (int)(zaman - çalışma.Baslangic).TotalMinutes;
                                    }

                                    try
                                    {
                                        await db.SaveChangesAsync();
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogError(ex.Message);
                                    }
                                }
                                else
                                {
                                    giden_Mesaj = new T3_Mesaj
                                    {
                                        Id = gelen_Mesaj.Id,
                                        Fonksiyon = 1,
                                        Durum = false,
                                        Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                        {
                                            Baslik = "Hata",
                                            Mesaj = "Personelin bu istasyonda aktif çalışması yok.",
                                            PanelTip = 1,
                                            Barkod = "",
                                            BarkodTip = 9,
                                        }),
                                        Tip = true,
                                    };

                                    await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));
                                }
                            }
                            break;
                        }

                    case 8:
                        {
                            var input = JsonConvert.DeserializeObject<InputModel>(gelen_Mesaj.Nesne);
                            if (input != null)
                            {
                                var ürün = await db.T3_Urun.Where(p => p.Barkod == input.Pano)
                                                           .FirstOrDefaultAsync();
                                if (ürün != null)
                                {
                                    var istasyon = await db.T3_Istasyon.Where(p => p.Id == Guid.Parse(input.Rulo))
                                                                       .FirstOrDefaultAsync();
                                    if (istasyon != null)
                                    {
                                        var personel = await db.T3_Personel.Where(p => p.Barkod == input.Personel).FirstOrDefaultAsync();
                                        if (personel != null)
                                        {

                                            var çalışma = await db.T3_Calisma.Where(p => p.IstasyonId == istasyon.Id &&
                                                                                         p.Aktif && 
                                                                                         p.Baslangic <= DateTime.Now &&
                                                                                         p.Bitis > DateTime.Now)
                                                                             .FirstOrDefaultAsync();
                                            if (çalışma != null)
                                            {
                                                var aktif_istasyon_Duruşu = await db.T3_IstasyonDurus.Where(p => p.IstasyonId == istasyon.Id &&
                                                                                                                 p.Baslangic == p.Bitis).ToListAsync();
                                                if (aktif_istasyon_Duruşu.Count > 0)
                                                {
                                                    foreach (var duruş in aktif_istasyon_Duruşu)
                                                    {
                                                        duruş.Bitis = zaman;
                                                        duruş.Zaman = (int)(zaman - duruş.Baslangic).TotalSeconds;
                                                    }
                                                }
                                                var ürünId = ürün.Id;

                                                var üretim = await db.T3_Uretim.Where(p => p.IstasyonId == istasyon.Id &&
                                                                                           p.UrunId == ürünId)
                                                                                .FirstOrDefaultAsync();


                                                if (üretim == null)
                                                {
                                                    var uretimId = Guid.NewGuid();
                                                    await db.T3_Uretim.AddAsync(new T3_Uretim
                                                    {
                                                        Id = uretimId,
                                                        IstasyonId = istasyon.Id,
                                                        UrunId = ürünId,
                                                        Kod = ürün.ProjectName + "_" + ürün.Switchgear + "_" + ürün.Panel_No,
                                                        Barkod = input.Pano,
                                                        Baslangic = zaman,
                                                        Bitis = zaman,
                                                        BitisHedef = zaman,
                                                        Miktar = 1,
                                                        SureHedef = 0,
                                                        SureGercek = 0,
                                                        SureHedefG = 0,
                                                    });

                                                    try
                                                    {
                                                        await db.SaveChangesAsync();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _logger.LogInformation(ex.Message);
                                                    }

                                                    await db.T3_UretimCalisma.AddAsync(new T3_UretimCalisma
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        CalismaId = çalışma.Id,
                                                        UretimId = uretimId,
                                                    });

                                                    try
                                                    {
                                                        await db.SaveChangesAsync();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _logger.LogInformation(ex.Message);
                                                    }

                                                    await db.T3_UretimPersonel.AddAsync(new T3_UretimPersonel
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        Baslangic = zaman,
                                                        Bitis = zaman,
                                                        PersonelId = personel.Id,
                                                        UretimId = uretimId,
                                                        Zaman = 0,
                                                    });

                                                    try
                                                    {
                                                        await db.SaveChangesAsync();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _logger.LogInformation(ex.Message);
                                                    }
                                                }
                                                else
                                                {

                                                    Guid uretimId = üretim.Id;

                                                    var üretimÇalışmaVarmı = await db.T3_UretimCalisma.Where(p => p.CalismaId == çalışma.Id && p.UretimId == uretimId)
                                                                                                      .FirstOrDefaultAsync();
                                                    if (üretimÇalışmaVarmı == null)
                                                    {
                                                        await db.T3_UretimCalisma.AddAsync(new T3_UretimCalisma
                                                        {
                                                            Id = Guid.NewGuid(),
                                                            CalismaId = çalışma.Id,
                                                            UretimId = uretimId,
                                                        });

                                                        try
                                                        {
                                                            await db.SaveChangesAsync();
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            _logger.LogInformation(ex.Message);
                                                        }
                                                    }

                                                    await db.T3_UretimPersonel.AddAsync(new T3_UretimPersonel
                                                    {
                                                        Id = Guid.NewGuid(),
                                                        Baslangic = zaman,
                                                        Bitis = zaman,
                                                        PersonelId = personel.Id,
                                                        UretimId = uretimId,
                                                        Zaman = 0,
                                                    });

                                                    try
                                                    {
                                                        await db.SaveChangesAsync();
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        _logger.LogInformation(ex.Message);
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    case 9:
                        {
                            InputModel? input = JsonConvert.DeserializeObject<InputModel>(gelen_Mesaj.Nesne);

                            if (input != null)
                            {
                                string _pano = input.Pano;
                                string _istasyon = input.Rulo;

                                if (_pano != "")
                                {
                                    var ürün = await db.T3_Urun.Where(p => p.Barkod == _pano).FirstOrDefaultAsync();

                                    if (ürün != null)
                                    {
                                        var istasyon = await db.T3_Istasyon.Where(p => p.Id == Guid.Parse(_istasyon))
                                                                           .FirstOrDefaultAsync();
                                        if (istasyon != null)
                                        {
                                            bool hata = true;
                                            
                                            while (hata)
                                            {
                                                var bitirilecek_Üretim = await db.T3_Uretim.Where(p => p.IstasyonId == istasyon.Id &&
                                                                                                   p.Baslangic == p.Bitis &&
                                                                                                   p.UrunId == ürün.Id)
                                                                                          .FirstOrDefaultAsync();
                                                if (bitirilecek_Üretim != null)
                                                {
                                                    try
                                                    {
                                                        bitirilecek_Üretim.Bitis = DateTime.Now;
                                                        db.SaveChanges();
                                                        hata = false;
                                                    }
                                                    catch (Exception ex)
                                                    {
                                                        hata = true;
                                                        _logger.LogError(ex.Message);
                                                    }
                                                }
                                                else
                                                {
                                                    giden_Mesaj = new T3_Mesaj
                                                    {
                                                        Id = gelen_Mesaj.Id,
                                                        Fonksiyon = 1,
                                                        Durum = false,
                                                        Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                        {
                                                            Baslik = "Hata",
                                                            Mesaj = "Lütfen hatta üretimi olan bir panonun barkodunu okutun.",
                                                            PanelTip = 1,
                                                            Barkod = "",
                                                            BarkodTip = 9,
                                                        }),
                                                        Tip = true,
                                                    };

                                                    await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));
                                                    hata = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var istasyon = await db.T3_Istasyon.Where(p => p.Id == Guid.Parse(_istasyon))
                                                                           .FirstOrDefaultAsync();

                                    if (istasyon != null)
                                    {
                                        var bitirilecek_Üretimler = await db.T3_Uretim.Where(p => p.IstasyonId == istasyon.Id &&
                                                                                                   p.Baslangic == p.Bitis)
                                                                                      .ToListAsync();
                                        if (bitirilecek_Üretimler.Count == 1)
                                        {
                                            var bitirileceküretim = bitirilecek_Üretimler.FirstOrDefault();

                                            if (bitirileceküretim != null)
                                                try
                                                {
                                                    bitirileceküretim.Bitis = DateTime.Now;
                                                    db.SaveChanges();
                                                }
                                                catch (Exception ex)
                                                {
                                                    _logger.LogError(ex.Message);
                                                }
                                        }

                                        if (bitirilecek_Üretimler.Count > 1)
                                        {
                                            giden_Mesaj = new T3_Mesaj
                                            {
                                                Id = gelen_Mesaj.Id,
                                                Fonksiyon = 1,
                                                Durum = false,
                                                Nesne = JsonConvert.SerializeObject(new T3_MesajBarkod
                                                {
                                                    Baslik = "Hata",
                                                    Mesaj = "Lütfen önce pano barkodunu okutun.",
                                                    PanelTip = 1,
                                                    Barkod = "",
                                                    BarkodTip = 9,
                                                }),
                                                Tip = true,
                                            };

                                            await Send("Servis", id, JsonConvert.SerializeObject(giden_Mesaj));
                                        }

                                    }
                                }
                            }
                            break;
                        }
                }
            }
        }

        public class InputModel
        {
            public string Personel { get; set; } = "";
            public string Rulo { get; set; } = "";
            public string Pano { get; set; } = "";
            public string Durus { get; set; } = "";
        }
    }
}
