using System.Linq;
using System.Xml.Linq;
using _01_DbModel.AppModel;
using _01_DbModel.Db;
using _01_Servis.Helpers;
using _01_Servis.Model;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Newtonsoft.Json;

namespace _01_Servis
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private List<Guid> ListId = new List<Guid>();
        private string Hat = "";

        private HubConnection _connection;
        private IServiceProvider _serviceProvider;
        public List<OnlineModel> List_Online_Cihazlar { get; set; } = new List<OnlineModel>();
        public List<OnlineModel> List_Dashboard_Cihazlar { get; set; } = new List<OnlineModel>();

        public Worker(ILogger<Worker> logger, IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _logger = logger;

            string ipApi = configuration.GetSection("IpAdres:IpApi").Get<string>();

            Hat = configuration.GetSection("Hat").Get<string>();

            foreach (var id in configuration.GetSection(Hat).Get<string[]>())
                ListId.Add(new Guid(id));

            _connection = new HubConnectionBuilder()
                                         .WithUrl(ipApi + "soketHub", options =>
                                         {
                                             options.UseDefaultCredentials = true;
                                             options.HttpMessageHandlerFactory = (msg) =>
                                             {
                                                 if (msg is HttpClientHandler clientHandler)
                                                 {
                                                     // bypass SSL certificate
                                                     clientHandler.ServerCertificateCustomValidationCallback +=
                                                         (sender, certificate, chain, sslPolicyErrors) => { return true; };
                                                 }

                                                 return msg;
                                             };
                                         })
                                         .WithAutomaticReconnect()
                                         .Build();
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _connection.On<string, string>("Servis", async (id, mesaj) =>
            {
                try
                {
                    var güncelZaman = DateTime.Now;

                    var gid = Guid.Parse(id);

                    var onlineVarMý = List_Online_Cihazlar.Where(p => p.Id == gid).FirstOrDefault();

                    if (onlineVarMý == null)
                        List_Online_Cihazlar.Add(new OnlineModel
                        {
                            Id = gid,
                            Zaman = güncelZaman,
                        });
                    else
                        onlineVarMý.Zaman = güncelZaman;

                    await _connection.InvokeAsync("Send", "Servis", id, mesaj);
                }
                catch
                { }
            });

            while (!stoppingToken.IsCancellationRequested)
            {
                await SignalRBaglanAsync();

                var z1 = DateTime.Now;

                try
                {
                    OnlineKontrol();
                    OnlineDashboardKontrol();

                    DateTime z2 = DateTime.Now;

                    var güncelZaman = DateTime.Now;

                    var db = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<XModel>();

                    var istasyon_List = await db.T3_Istasyon.Where(p => ListId.Contains(p.Id))
                                                            .Include(p => p.T3_Calisma.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.T3_UretimCalisma)
                                                            .ThenInclude(p => p.Uretim)
                                                            .ThenInclude(p => p.T3_UretimDurus)
                                                            .Include(p => p.T3_Calisma.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.T3_UretimCalisma)
                                                            .ThenInclude(p => p.Uretim)
                                                            .ThenInclude(p => p.Urun)
                                                            .ThenInclude(p => p.Tip)
                                                            .Include(p => p.T3_Calisma.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.T3_UretimCalisma)
                                                            .ThenInclude(p => p.Uretim)
                                                            .ThenInclude(p => p.T3_UretimPersonel)
                                                            .ThenInclude(p => p.Personel)
                                                            .Include(p => p.T3_Calisma.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.T3_UretimCalisma)
                                                            .ThenInclude(p => p.Uretim)
                                                            .ThenInclude(p => p.T3_UretimPlanliDurus)
                                                            .Include(p => p.T3_Calisma.Where(p => p.Aktif))
                                                            .ThenInclude(p => p.T3_CalismaPlanliDurus)
                                                            .Include(p => p.T3_Calisma)
                                                            .ThenInclude(p => p.T3_CalismaIstasyonBosZaman)
                                                            .Include(p => p.Lokasyon)
                                                            .Include(p => p.T3_IstasyonDurus)
                                                            .OrderBy(p => p.Sira)
                                                            .ToListAsync();
                    //_logger.LogInformation("1 - " + (DateTime.Now - z1).TotalMilliseconds.ToString());

                    try
                    {
                        List<SayfaModel> dashboard_List = new();

                        List<Lojistik> lojistik_list = new();

                        DateTime z0 = new(DateTime.Now.Ticks);

                        foreach (var istasyon in istasyon_List)
                        {
                            if (istasyon != null)
                            {

                                if(istasyon.Kod == "ZS1-FA3")
                                {

                                }

                                var lojistik = new Lojistik
                                {
                                    Istasyon = istasyon.Kod,
                                    Wbs = String.Join(",", istasyon.T3_Uretim.Where(p => p.Baslangic == p.Bitis).Select(p => p.Urun.Switchgear)),
                                    Panel = String.Join(",", istasyon.T3_Uretim.Where(p => p.Baslangic == p.Bitis).Select(p => p.Urun.Panel_No)),
                                    Sira = istasyon.Sira
                                };

                                SayfaModel sayfa_Model = new();

                                var aktifÇalýþmasýVarMý = istasyon.T3_Calisma.Where(p => p.Aktif)
                                                                             .OrderByDescending(p => p.Baslangic)
                                                                             .FirstOrDefault();
                                if (aktifÇalýþmasýVarMý != null)
                                {
                                    var aktifÇalýþma = ÇalýþmaGöster(aktifÇalýþmasýVarMý);
                                     
                                    sayfa_Model = new SayfaModel
                                    {
                                        Online = true,
                                        DurumTip = 1,
                                        IstasyonId = istasyon.Id,
                                        Istasyon = istasyon.Kod,
                                        Durum = aktifÇalýþma.Calisma.Kod,
                                        AktifUretimModel = aktifÇalýþma.AktifUretimModel,
                                        DurusUretimModel = aktifÇalýþma.DurusUretimModel,
                                        Calisma = aktifÇalýþma.Calisma,
                                        ListUretim = aktifÇalýþma.ListUretim,
                                        ListUretimAktif = aktifÇalýþma.ListUretim.Where(p => p.Durum == 1 || p.Durum == 5).ToList()
                                    };

                                    if (aktifÇalýþma.ListUretim.Where(p => p.Durum == 5).Any())
                                        sayfa_Model.DurumTip = 2;

                                    string gönderilecekMesaj = JsonConvert.SerializeObject(sayfa_Model);

                                    T3_Mesaj mesaj = new()
                                    {
                                        Id = istasyon.Id,
                                        Fonksiyon = 0,
                                        Durum = true,
                                        Nesne = gönderilecekMesaj,
                                        Tip = false,
                                    };


                                    string json_mesaj = JsonConvert.SerializeObject(mesaj);
                                    await _connection.InvokeAsync("Send", "Servis", istasyon.Id, json_mesaj);

                                    var dbModel = Dashboard(aktifÇalýþmasýVarMý);

                                    dbModel.Panel = new Dashboard_Panel_Model
                                    {
                                        Kod = istasyon.Kod,
                                        Target = aktifÇalýþma.Calisma.Hedef,
                                        Aktuel = aktifÇalýþma.Calisma.Aktuel,
                                        Delta = aktifÇalýþma.Calisma.Delta,
                                    };

                                    if (istasyon.Lokasyon.Kod == "MWS")
                                    {
                                        dbModel = new DashboardModel
                                        {
                                            Panel = new Dashboard_Panel_Model
                                            {
                                                Kod = istasyon.Kod,
                                                Target = aktifÇalýþma.Calisma.Hedef,
                                                Aktuel = aktifÇalýþma.Calisma.Aktuel,
                                                Delta = aktifÇalýþma.Calisma.Delta,
                                            }
                                        };
                                    }

                                    mesaj = new()
                                    {
                                        Id = istasyon.Id,
                                        Fonksiyon = 0,
                                        Durum = true,
                                        Nesne = JsonConvert.SerializeObject(dbModel),
                                        Tip = false,
                                    };

                                    string kanal = "db_" + istasyon.Id.ToString();
                                    json_mesaj = JsonConvert.SerializeObject(mesaj);

                                    await _connection.InvokeAsync("Send", "Servis", kanal, json_mesaj);

                                }
                                else
                                {
                                    sayfa_Model = new SayfaModel
                                    {
                                        Online = true,
                                        DurumTip = 0,
                                        Durum = "Çalýþma Yok",
                                        IstasyonId = istasyon.Id,
                                        Istasyon = istasyon.Kod,
                                        AktifUretimModel = "",
                                        DurusUretimModel = "",
                                        Calisma = new(),
                                        ListUretim = new()
                                    };

                                    string gönderilecekMesaj = JsonConvert.SerializeObject(sayfa_Model);
                                    await _connection.InvokeAsync("Send", "Servis", istasyon.Id, gönderilecekMesaj);
                                }

                                if (aktifÇalýþmasýVarMý != null)
                                    dashboard_List.Add(sayfa_Model);

                                lojistik_list.Add(lojistik);
                            }
                        }
                        //_logger.LogInformation("2 - " + (DateTime.Now - z0).TotalMilliseconds.ToString());

                        string dashboard_Mesaj = JsonConvert.SerializeObject(dashboard_List);
                        await _connection.InvokeAsync("Send", "Servis", "dashboard", dashboard_Mesaj);

                        string lojistik_Mesaj = JsonConvert.SerializeObject(new
                        {
                            Hat = Hat,
                            Liste = lojistik_list.OrderBy(p => p.Sira),
                        });
                        await _connection.InvokeAsync("Send", "Servis", "lojistik", lojistik_Mesaj);

                    }
                    catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

                //}
                await Task.Delay(50, stoppingToken);
            }
        }

        public async Task SignalRBaglanAsync()
        {
            while (_connection.State == HubConnectionState.Disconnected)
            {
                try
                {
                    _connection.ServerTimeout = TimeSpan.FromSeconds(5);
                    await _connection.StartAsync();
                }
                catch
                { }

                if (_connection.State == HubConnectionState.Disconnected)
                    Thread.Sleep(500);
            }
        }
        public SayfaModel ÇalýþmaGöster(T3_Calisma Çalýþma)
        {
            int aktuel = 0;
            int delta = 0;
            DateTime zaman = DateTime.Now;

            List<UretimModel> list_Uretim = new();
            string aktifÜretimModel = "";
            string durusÜretimModel = "";

            try
            {
                foreach (var üretim in Çalýþma.Istasyon.T3_Uretim.Where(p => p.Baslangic == p.Bitis))
                {
                    int durum = 0;
                    string icerik = "";
                    string stil_Baslik = "text-bg-light";

                    var devamEdenDurusVarmý = üretim.T3_UretimDurus.Where(p => p.Zaman == 0).FirstOrDefault();

                    if (devamEdenDurusVarmý != null)
                        durum = 5;

                    if (durum == 0)
                        if (üretim.T3_UretimPlanliDurus.Where(p => p.Baslangic <= zaman && p.Bitis >= zaman).Any())
                            durum = 6;

                    if (durum == 0)
                        if (üretim.Baslangic == üretim.Bitis)
                        {
                            if (üretim.BitisHedef > zaman)
                                durum = 1;
                            else
                                durum = 2;
                        }

                    switch (durum)
                    {
                        case 1:
                            {
                                stil_Baslik = "text-bg-success";
                                break;
                            }
                        case 2:
                            {
                                stil_Baslik = "text-bg-warning";
                                break;
                            }
                        case 5:
                            {
                                stil_Baslik = "text-bg-danger";
                                break;
                            }
                        case 6:
                            {
                                stil_Baslik = "text-bg-info";
                                break;
                            }
                    }

                    var sn_PersonelÇalýþmaBiten = üretim.T3_UretimPersonel.Where(p => p.Baslangic != p.Bitis).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
                    var sn_PersonelÇalýþmaDevamEden = üretim.T3_UretimPersonel.Where(p => p.Baslangic == p.Bitis).Sum(p => (DateTime.Now - p.Baslangic).TotalSeconds);

                    string aktifPersoneller = String.Join(", ", üretim.T3_UretimPersonel.Where(p => p.Baslangic == p.Bitis).Select(p => (p.Personel.Ad + " " + p.Personel.Soyad)));
                    string aktifPersonellerPanel = "";
                    if (aktifPersoneller.Length > 0)
                    {
                        aktifPersonellerPanel = "<div class='p-0 ps-3 m-0 text-black'>  " +
                                                    "<svg width='24' height='24' fill='currentColor' class='bi bi-person-lines-fill' viewBox='0 0 16 16'>  <path d='M6 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6zm-5 6s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1H1zM11 3.5a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5zm.5 2.5a.5.5 0 0 0 0 1h4a.5.5 0 0 0 0-1h-4zm2 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1h-2zm0 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1h-2z'/></svg>" +
                                                    "<span class = 'ms-2'> " + aktifPersoneller + "</span></div>";
                    }

                    string aktifDuruslar = String.Join(", ", üretim.T3_UretimDurus.Where(p => p.Baslangic == p.Bitis).Select(p => p.Kod));
                    string aktifDuruslarPanel = "";
                    if (aktifDuruslar.Length > 0)
                    {
                        aktifDuruslarPanel = "<div class='p-0 ps-3 m-0 text-black'>  " +
                                                    "<svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' fill='currentColor' class='bi bi-exclamation-triangle-fill' viewBox='0 0 16 16'>  <path d='M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z'/></svg>" +
                                                    "<span class = 'ms-2'> " + aktifDuruslar + "</span></div>";
                    }



                    var sn_DurusBiten = üretim.T3_UretimDurus.Where(p => p.Baslangic != p.Bitis).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
                    var sn_DurusDevamEden = üretim.T3_UretimDurus.Where(p => p.Baslangic == p.Bitis).Sum(p => (DateTime.Now - p.Baslangic).TotalSeconds);

                    var sn_PlanlýDurusBiten = üretim.T3_UretimPlanliDurus.Where(p => p.Baslangic != p.Bitis && p.Bitis < zaman).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
                    var sn_PlanlýDurusDevamEden = üretim.T3_UretimPlanliDurus.Where(p => p.Baslangic != p.Bitis && p.Bitis > zaman).Sum(p => (zaman - p.Baslangic).TotalSeconds);


                    TimeSpan sn_planlýdurus = TimeSpan.FromSeconds(sn_PlanlýDurusBiten + sn_PlanlýDurusDevamEden);
                    TimeSpan sn_durus = TimeSpan.FromSeconds(sn_DurusBiten + sn_DurusDevamEden);
                    TimeSpan sn_çalýþma = TimeSpan.FromSeconds(sn_PersonelÇalýþmaBiten + sn_PersonelÇalýþmaDevamEden);

                    string stil_Calisma = "text-bg-light";
                    string stil_Durus = "text-bg-light";
                    string stil_Mola = "text-bg-light";

                    if (sn_PlanlýDurusDevamEden > 0) stil_Mola = "text-bg-info";
                    if (sn_DurusDevamEden > 0) stil_Durus = "text-bg-danger";
                    if (sn_PersonelÇalýþmaDevamEden > 0) stil_Calisma = "text-bg-primary";

                    icerik += "<div class='card  m-1 p-0'> " +
                                        "   <div class='card-header " + stil_Baslik + "'> " + üretim.Kod + "</div> " +
                                        "   <div class='card-body p-0'> " +
                                                aktifPersonellerPanel +
                                                aktifDuruslarPanel +
                                        "       <div class='row p-0 m-0'>  " +
                                        "           <div class='card text-bg-dark m-1 p-0 col'> " +
                                        "               <div class='card-header p-1'>Zaman</div> " +
                                        "               <div class='card-body p-0 ps-2 text-center'> " +
                                        "                   <i class='bi bi-align-start'></i> " +
                                        "                   " + üretim.Baslangic.ToString("HH:mm") + "   <br /> " +
                                        "                   <i class='bi bi-align-end'></i> " +
                                        "                   " + üretim.BitisHedef.ToString("HH:mm") +
                                        "               </div> " +
                                        "           </div>  " +
                                        "           <div class=' card " + stil_Calisma + " m-1 p-0 col-3'> " +
                                        "               <div class='card-header p-1'>Çalýþma</div> " +
                                        "               <div class='card-body p-0'> " +
                                        "                   <p class='text-center fs-1 m-0'> " + sn_çalýþma.ToString(@"hh\:mm\:ss", null) + " </p> " +
                                        "               </div> " +
                                        "           </div>  " +
                                        "           <div class=' card " + stil_Durus + "  m-1 p-0 col-3'> " +
                                        "               <div class='card-header p-1'>Duruþ</div> " +
                                        "               <div class='card-body p-0'> " +
                                        "                   <p class='text-center fs-1 m-0'>" + sn_durus.ToString(@"hh\:mm\:ss", null) + "</p> " +
                                        "               </div> " +
                                        "           </div>  " +
                                        "           <div class=' card " + stil_Mola + "  m-1 p-0 col-3'> " +
                                        "               <div class='card-header p-1'> Mola </div> " +
                                        "               <div class='card-body p-0'> " +
                                        "                   <p class='text-center fs-1 m-0'>" + sn_planlýdurus.ToString(@"hh\:mm\:ss", null) + "</p> " +
                                        "               </div> " +
                                        "           </div> " +
                                        "       </div> " +
                                        "   </div> " +
                                        "</div> ";

                    aktifÜretimModel += icerik;

                    list_Uretim.Add(new()
                    {
                        Kod = üretim.Kod,
                        Barkod = üretim.Barkod,
                        Baslangic = üretim.Baslangic,
                        Bitis = üretim.Bitis,
                        DkHedef = üretim.SureHedef,
                        DkGerçek = üretim.SureGercek,
                        DkHedefG = üretim.SureHedefG,
                        Product = üretim.Urun.Tip.Product,
                        Rated_Volt = üretim.Urun.Tip.Rated_Volt,
                        Panel_Width = üretim.Urun.Tip.Panel_Width,
                        Panel_Type = üretim.Urun.Tip.Panel_Type,
                        Panel_Curr = üretim.Urun.Tip.Panel_Curr,
                        Shortc_Curr = üretim.Urun.Tip.Shortc_Curr,
                        Ct = üretim.Urun.Tip.Ct,
                        Vt = üretim.Urun.Tip.Vt,
                        Vt_With = üretim.Urun.Tip.Vt_With,
                        Vt_Rem = üretim.Urun.Tip.Vt_Rem,
                        Vt_Fix = üretim.Urun.Tip.Vt_Fix,
                        Sa = üretim.Urun.Tip.Sa,
                        Cap_Volt_Ind = üretim.Urun.Tip.Cap_Volt_Ind,
                        Es_Present = üretim.Urun.Tip.Es_Present,
                        Es_Type_Subs = üretim.Urun.Tip.Es_Type_Subs,
                        Ct_Sec_Con = üretim.Urun.Tip.Ct_Sec_Con,
                        Vt_Sec_Con = üretim.Urun.Tip.Vt_Sec_Con,
                        ProjectId = üretim.Urun.ProjectId,
                        ProjectName = üretim.Urun.ProjectName,
                        Switchgear = üretim.Urun.Switchgear,
                        Panel_No = üretim.Urun.Panel_No,
                        Shortc_Time = üretim.Urun.Shortc_Time,
                        Discon = üretim.Urun.Discon,
                        Durum = durum,
                        DurumKod = aktifPersoneller + aktifDuruslar,
                    });
                }
                 
                aktuel = Çalýþma.Aktuel;
                delta = Çalýþma.Delta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var istasyonDuruþListesi = Çalýþma.Istasyon.T3_IstasyonDurus.Where(p => p.Baslangic == p.Bitis).ToList();

            if (istasyonDuruþListesi.Count > 0)
            {
                aktifÜretimModel = "<div class='card m-1 p-0'> " +
                                        "   <div class='card-body p-0'> " +
                                        "       <div class='row p-0 m-0'>  ";

                foreach (var durus in istasyonDuruþListesi)
                {
                    TimeSpan sn_durus = (zaman - durus.Baslangic);

                    string icerik = "<div class='card  text-bg-danger m-1 p-0 col'> " +
                             "   <div class='card-header p-1'>" + durus.Kod + "</div> " +
                             "   <div class='card-body p-0'> " +
                             "          <p class='text-center fs-1 m-0'>" + sn_durus.ToString(@"hh\:mm\:ss", null) + "</p> " +
                             "   </div> " +
                             "</div>  ";

                    aktifÜretimModel += icerik;
                }
                aktifÜretimModel += "       </div> " +
                                    "   </div> " +
                                    "</div> ";
            }


            bool online = List_Online_Cihazlar.Where(p => p.Id == Çalýþma.IstasyonId).Any();

            SayfaModel model = new()
            {
                Istasyon = Çalýþma.Istasyon.Kod,
                Durum = "",
                Online = online,
                AktifUretimModel = aktifÜretimModel,
                DurusUretimModel = durusÜretimModel,
                Calisma = new CalismaModel
                {
                    Kod = Çalýþma.Kod,
                    Bas =
                          Çalýþma.Baslangic.ToShortTimeString(),
                    Bit =
                          Çalýþma.Bitis.ToShortTimeString(),
                    Hedef = Çalýþma.Hedef,
                    Aktuel = aktuel,
                    Delta = delta,
                },
                ListUretim = list_Uretim,
            };

            return model;
        }
        public DashboardModel Dashboard(T3_Calisma çalýþma)
        {
            DateTime Zaman = DateTime.Now;
            var dashboard = new DashboardModel();

            var list_TimeLine = new List<string[]>();

            var çalýþma_baþlangýç = çalýþma.Baslangic;
            var çalýþma_bitiþ = çalýþma.Bitis;

            foreach (var mola in çalýþma.T3_CalismaPlanliDurus.OrderBy(p => p.Baslangic))
            {
                var öncesindeki_mola = çalýþma.T3_CalismaPlanliDurus.Where(p => p.Id != mola.Id && p.Baslangic < mola.Baslangic)
                                                                    .OrderByDescending(p => p.Baslangic)
                                                                    .FirstOrDefault();
                if (öncesindeki_mola != null)
                    çalýþma_baþlangýç = öncesindeki_mola.Bitis;

                list_TimeLine.Add(new string[] { "Planlý Çalýþma", çalýþma.Kod, "", "darkgreen", çalýþma_baþlangýç.ToString("yyyy-MM-dd HH:mm:ss"), mola.Baslangic.ToString("yyyy-MM-dd HH:mm:ss") });
                list_TimeLine.Add(new string[] { "Planlý Çalýþma", mola.Kod, "", "orange", mola.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), mola.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            var sonmola = çalýþma.T3_CalismaPlanliDurus.OrderByDescending(p => p.Baslangic)
                                                       .FirstOrDefault();
            if (sonmola != null)
                list_TimeLine.Add(new string[] { "Planlý Çalýþma", çalýþma.Kod, "", "darkgreen", sonmola.Bitis.ToString("yyyy-MM-dd HH:mm:ss"), çalýþma.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });
            else
                list_TimeLine.Add(new string[] { "Planlý Çalýþma", çalýþma.Kod, "", "darkgreen", çalýþma.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), çalýþma.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });

            çalýþma_baþlangýç = çalýþma.Baslangic;
            çalýþma_bitiþ = çalýþma.Bitis;

            List<UretimOlay> list_Olay = new List<UretimOlay>();
            List<UretimOlay> list_Istasyon_Bos = new List<UretimOlay>();

            foreach (var boþta in çalýþma.T3_CalismaIstasyonBosZaman)
            {
                var boþ_bitiþ = boþta.Bitis;
                if (boþta.Baslangic == boþta.Bitis)
                    boþ_bitiþ = Zaman;

                list_Istasyon_Bos.Add(new UretimOlay
                {
                    Id = boþta.Id,
                    Baslangic = boþta.Baslangic,
                    Bitis = boþ_bitiþ,
                    Kod = "Boþ",
                    Tip = 10
                });
                list_TimeLine.Add(new string[] { "Ýstasyon Boþ Zaman", "Boþ", "", "darkgray", boþta.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), boþ_bitiþ.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            foreach (var üretim in çalýþma.Istasyon.T3_Uretim.OrderBy(p => p.Bitis))
            {

                var üretim_baþlangýç = üretim.Baslangic;
                var üretim_bitiþ = üretim.Bitis;

                List<UretimOlay> list_Olay_Uretim = new List<UretimOlay>();
                List<UretimOlay> list_Olay_Personel = new List<UretimOlay>();
                foreach (var personel_Çalýþma in üretim.T3_UretimPersonel.Where(p => p.Baslangic >= çalýþma_baþlangýç).OrderBy(p => p.Baslangic))
                {
                    var personel_Çalýþma_Bitiþ = personel_Çalýþma.Bitis;
                    if (personel_Çalýþma.Baslangic == personel_Çalýþma.Bitis)
                        personel_Çalýþma_Bitiþ = Zaman;

                    list_Olay_Personel.Add(new UretimOlay
                    {
                        Id = personel_Çalýþma.Id,
                        UretimId = üretim.Id,
                        Tip = 1,
                        TipKod = üretim.Kod,
                        Renk = "blue",
                        Kod = personel_Çalýþma.Personel.Ad + " " + personel_Çalýþma.Personel.Soyad,
                        Baslangic = personel_Çalýþma.Baslangic,
                        Bitis = personel_Çalýþma_Bitiþ,
                    });
                }

                if (list_Olay_Personel.Count == 0)
                    list_Olay_Uretim.Add(new()
                    {
                        Id = üretim.Id,
                        UretimId = üretim.Id,
                        Kod = üretim.Kod,
                        Baslangic = üretim.Baslangic,
                        Bitis = üretim.Bitis,
                        Renk = "blue",
                        Tip = 1,
                        TipKod = üretim.Kod,
                    });
                else
                    list_Olay_Uretim.AddRange(list_Olay_Personel.Merge());

                List<UretimOlay> list_Olay_Durus = new();
                foreach (var durus in üretim.T3_UretimDurus.Where(p => p.Baslangic >= çalýþma_baþlangýç).OrderBy(p => p.Baslangic))
                {
                    var durus_Bitiþ = durus.Bitis;
                    if (durus.Baslangic == durus.Bitis)
                        durus_Bitiþ = Zaman;

                    list_Olay_Durus.Add(new UretimOlay
                    {
                        Id = durus.Id,
                        UretimId = üretim.Id,
                        Tip = 2,
                        TipKod = üretim.Kod,
                        Renk = "red",
                        Kod = durus.Kod,
                        Baslangic = durus.Baslangic,
                        Bitis = durus_Bitiþ,
                    });
                }
                list_Olay_Uretim.AddRange(list_Olay_Durus.Merge());

                List<UretimOlay> list_Olay_PlanlýDurus = new();
                foreach (var planlidurus in üretim.T3_UretimPlanliDurus.OrderBy(p => p.Baslangic))
                {
                    var durus_Bitiþ = planlidurus.Bitis;
                    if (planlidurus.Baslangic == planlidurus.Bitis)
                        durus_Bitiþ = Zaman;

                    list_Olay_PlanlýDurus.Add(new UretimOlay
                    {
                        Id = planlidurus.Id,
                        UretimId = üretim.Id,
                        Tip = 3,
                        TipKod = üretim.Kod,
                        Renk = "orange",
                        Kod = planlidurus.Kod,
                        Baslangic = planlidurus.Baslangic,
                        Bitis = durus_Bitiþ
                    });
                }

                list_Olay_Uretim.AddRange(list_Olay_PlanlýDurus.Merge());

                foreach (var item in list_Olay_Uretim)
                {
                    list_TimeLine.Add(new string[] { item.TipKod, item.Kod, item.Tooltip, item.Renk, item.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), item.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });
                }

                list_Olay.AddRange(list_Olay_Uretim);
            }

            dashboard.TimeLine = list_TimeLine;
            dashboard.UretimSayisi = çalýþma.Istasyon.T3_Uretim.Count;

            if (çalýþma_bitiþ > Zaman)
                çalýþma_bitiþ = Zaman;

            foreach (var personel_çalýþma in list_Olay.Where(p => p.Tip == 1))
            {
                var personel_çalýþma_bitiþ = personel_çalýþma.Bitis;

                if (personel_çalýþma.Baslangic == personel_çalýþma.Bitis)
                    personel_çalýþma_bitiþ = Zaman;

                int saniye = 0;
                saniye = (int)(personel_çalýþma_bitiþ - personel_çalýþma.Baslangic).TotalSeconds;
                var varmý = dashboard.Pie_Personel_Calisma.Where(p => p[0].ToString() == personel_çalýþma.Kod).FirstOrDefault();
                if (varmý == null)
                {
                    dashboard.Pie_Personel_Calisma.Add(new object[]
                    {
                        personel_çalýþma.Kod,
                        saniye,
                        pasta_Tooltip(personel_çalýþma.Kod, "a", saniye, "primary")
                    });
                }
                else
                {
                    varmý[1] = ((int)varmý[1]) + saniye;
                    varmý[2] = pasta_Tooltip(personel_çalýþma.Kod, "a", (int)varmý[1], "primary");
                }
            }

            foreach (var durus in list_Olay.Where(p => p.Tip == 2))
            {
                int saniye = 0;
                var varmý = dashboard.Pie_Durus.Where(p => p[0].ToString() == durus.Kod).FirstOrDefault();
                if (varmý == null)
                {
                    saniye = (int)(durus.Bitis - durus.Baslangic).TotalSeconds;
                    dashboard.Pie_Durus.Add(new object[]
                    {
                        durus.Kod,
                        saniye,
                        pasta_Tooltip(durus.Kod, "a", saniye, "danger")
                    });
                }
                else
                {
                    varmý[1] = ((int)varmý[1]) + saniye;
                    varmý[2] = pasta_Tooltip(durus.Kod, "a", saniye, "danger");
                }
            }

            int toplam_personel_calisma = (int)list_Olay.Where(p => p.Tip == 1).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
            int toplam_durus = (int)list_Olay.Where(p => p.Tip == 2).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
            int toplam_planli_durus = (int)list_Olay.Where(p => p.Tip == 3).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
            int geçen_zaman = (int)(çalýþma_bitiþ - çalýþma_baþlangýç).TotalSeconds * dashboard.UretimSayisi;
            int istasyon_bos = list_Istasyon_Bos.Where(p => p.Tip == 10).Sum(p => (int)(p.Bitis - p.Baslangic).TotalSeconds);

            dashboard.Pie = new()
            { 
                //new object[] { "Toplam Zaman", geçen_zaman },  
                new object[] { "Boþ Zaman", istasyon_bos, pasta_Tooltip("Boþ Zaman", "a", istasyon_bos, "secondary")  },
                new object[] { "Çalýþma", toplam_personel_calisma, pasta_Tooltip("Çalýþma", "a", toplam_personel_calisma, "primary") },
                new object[] { "Duruþ", toplam_durus , pasta_Tooltip("Duruþ", "a", toplam_durus, "danger")},
                new object[] { "Planlý Duruþ", toplam_planli_durus , pasta_Tooltip("Planlý Duruþ", "a", toplam_planli_durus, "warning")},
            };

            return dashboard;
        }
        public void OnlineKontrol()
        {
            var silinecekler = new List<Guid>();
            foreach (var item in List_Online_Cihazlar)
                if (item.Zaman.AddMinutes(5) < DateTime.Now)
                    silinecekler.Add(item.Id);

            foreach (var silinecek in silinecekler)
            {
                var silinecekItem = List_Online_Cihazlar.Where(p => p.Id == silinecek).FirstOrDefault();
                if (silinecekItem != null)
                    List_Online_Cihazlar.Remove(silinecekItem);
            }
        }
        public void OnlineDashboardKontrol()
        {
            var silinecekler = new List<Guid>();
            foreach (var item in List_Dashboard_Cihazlar)
                if (item.Zaman.AddMinutes(5) < DateTime.Now)
                    silinecekler.Add(item.Id);

            foreach (var silinecek in silinecekler)
            {
                var silinecekItem = List_Dashboard_Cihazlar.Where(p => p.Id == silinecek).FirstOrDefault();
                if (silinecekItem != null)
                    List_Dashboard_Cihazlar.Remove(silinecekItem);
            }
        }
        public string pasta_Tooltip(string baþlýk, string açýklama, int saniye, string tema)
        {
            string dönen = "";

            TimeSpan time = TimeSpan.FromSeconds(saniye);
            string zaman = time.ToString(@"hh\:mm\:ss");

            dönen = "<div class='card text-bg-" + tema + "' style='width: 8rem;'> " +
                        "<div class='card-header'>" + baþlýk + "</div>" +
                        "<div class='card-footer text-bg-light'>" + zaman + "</div>" +
                    "</div>";
            return dönen;
        }
    }
}