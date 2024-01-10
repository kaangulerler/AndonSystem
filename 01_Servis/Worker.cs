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
                    var g�ncelZaman = DateTime.Now;

                    var gid = Guid.Parse(id);

                    var onlineVarM� = List_Online_Cihazlar.Where(p => p.Id == gid).FirstOrDefault();

                    if (onlineVarM� == null)
                        List_Online_Cihazlar.Add(new OnlineModel
                        {
                            Id = gid,
                            Zaman = g�ncelZaman,
                        });
                    else
                        onlineVarM�.Zaman = g�ncelZaman;

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

                    var g�ncelZaman = DateTime.Now;

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

                                var aktif�al��mas�VarM� = istasyon.T3_Calisma.Where(p => p.Aktif)
                                                                             .OrderByDescending(p => p.Baslangic)
                                                                             .FirstOrDefault();
                                if (aktif�al��mas�VarM� != null)
                                {
                                    var aktif�al��ma = �al��maG�ster(aktif�al��mas�VarM�);
                                     
                                    sayfa_Model = new SayfaModel
                                    {
                                        Online = true,
                                        DurumTip = 1,
                                        IstasyonId = istasyon.Id,
                                        Istasyon = istasyon.Kod,
                                        Durum = aktif�al��ma.Calisma.Kod,
                                        AktifUretimModel = aktif�al��ma.AktifUretimModel,
                                        DurusUretimModel = aktif�al��ma.DurusUretimModel,
                                        Calisma = aktif�al��ma.Calisma,
                                        ListUretim = aktif�al��ma.ListUretim,
                                        ListUretimAktif = aktif�al��ma.ListUretim.Where(p => p.Durum == 1 || p.Durum == 5).ToList()
                                    };

                                    if (aktif�al��ma.ListUretim.Where(p => p.Durum == 5).Any())
                                        sayfa_Model.DurumTip = 2;

                                    string g�nderilecekMesaj = JsonConvert.SerializeObject(sayfa_Model);

                                    T3_Mesaj mesaj = new()
                                    {
                                        Id = istasyon.Id,
                                        Fonksiyon = 0,
                                        Durum = true,
                                        Nesne = g�nderilecekMesaj,
                                        Tip = false,
                                    };


                                    string json_mesaj = JsonConvert.SerializeObject(mesaj);
                                    await _connection.InvokeAsync("Send", "Servis", istasyon.Id, json_mesaj);

                                    var dbModel = Dashboard(aktif�al��mas�VarM�);

                                    dbModel.Panel = new Dashboard_Panel_Model
                                    {
                                        Kod = istasyon.Kod,
                                        Target = aktif�al��ma.Calisma.Hedef,
                                        Aktuel = aktif�al��ma.Calisma.Aktuel,
                                        Delta = aktif�al��ma.Calisma.Delta,
                                    };

                                    if (istasyon.Lokasyon.Kod == "MWS")
                                    {
                                        dbModel = new DashboardModel
                                        {
                                            Panel = new Dashboard_Panel_Model
                                            {
                                                Kod = istasyon.Kod,
                                                Target = aktif�al��ma.Calisma.Hedef,
                                                Aktuel = aktif�al��ma.Calisma.Aktuel,
                                                Delta = aktif�al��ma.Calisma.Delta,
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
                                        Durum = "�al��ma Yok",
                                        IstasyonId = istasyon.Id,
                                        Istasyon = istasyon.Kod,
                                        AktifUretimModel = "",
                                        DurusUretimModel = "",
                                        Calisma = new(),
                                        ListUretim = new()
                                    };

                                    string g�nderilecekMesaj = JsonConvert.SerializeObject(sayfa_Model);
                                    await _connection.InvokeAsync("Send", "Servis", istasyon.Id, g�nderilecekMesaj);
                                }

                                if (aktif�al��mas�VarM� != null)
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
        public SayfaModel �al��maG�ster(T3_Calisma �al��ma)
        {
            int aktuel = 0;
            int delta = 0;
            DateTime zaman = DateTime.Now;

            List<UretimModel> list_Uretim = new();
            string aktif�retimModel = "";
            string durus�retimModel = "";

            try
            {
                foreach (var �retim in �al��ma.Istasyon.T3_Uretim.Where(p => p.Baslangic == p.Bitis))
                {
                    int durum = 0;
                    string icerik = "";
                    string stil_Baslik = "text-bg-light";

                    var devamEdenDurusVarm� = �retim.T3_UretimDurus.Where(p => p.Zaman == 0).FirstOrDefault();

                    if (devamEdenDurusVarm� != null)
                        durum = 5;

                    if (durum == 0)
                        if (�retim.T3_UretimPlanliDurus.Where(p => p.Baslangic <= zaman && p.Bitis >= zaman).Any())
                            durum = 6;

                    if (durum == 0)
                        if (�retim.Baslangic == �retim.Bitis)
                        {
                            if (�retim.BitisHedef > zaman)
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

                    var sn_Personel�al��maBiten = �retim.T3_UretimPersonel.Where(p => p.Baslangic != p.Bitis).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
                    var sn_Personel�al��maDevamEden = �retim.T3_UretimPersonel.Where(p => p.Baslangic == p.Bitis).Sum(p => (DateTime.Now - p.Baslangic).TotalSeconds);

                    string aktifPersoneller = String.Join(", ", �retim.T3_UretimPersonel.Where(p => p.Baslangic == p.Bitis).Select(p => (p.Personel.Ad + " " + p.Personel.Soyad)));
                    string aktifPersonellerPanel = "";
                    if (aktifPersoneller.Length > 0)
                    {
                        aktifPersonellerPanel = "<div class='p-0 ps-3 m-0 text-black'>  " +
                                                    "<svg width='24' height='24' fill='currentColor' class='bi bi-person-lines-fill' viewBox='0 0 16 16'>  <path d='M6 8a3 3 0 1 0 0-6 3 3 0 0 0 0 6zm-5 6s-1 0-1-1 1-4 6-4 6 3 6 4-1 1-1 1H1zM11 3.5a.5.5 0 0 1 .5-.5h4a.5.5 0 0 1 0 1h-4a.5.5 0 0 1-.5-.5zm.5 2.5a.5.5 0 0 0 0 1h4a.5.5 0 0 0 0-1h-4zm2 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1h-2zm0 3a.5.5 0 0 0 0 1h2a.5.5 0 0 0 0-1h-2z'/></svg>" +
                                                    "<span class = 'ms-2'> " + aktifPersoneller + "</span></div>";
                    }

                    string aktifDuruslar = String.Join(", ", �retim.T3_UretimDurus.Where(p => p.Baslangic == p.Bitis).Select(p => p.Kod));
                    string aktifDuruslarPanel = "";
                    if (aktifDuruslar.Length > 0)
                    {
                        aktifDuruslarPanel = "<div class='p-0 ps-3 m-0 text-black'>  " +
                                                    "<svg xmlns='http://www.w3.org/2000/svg' width='24' height='24' fill='currentColor' class='bi bi-exclamation-triangle-fill' viewBox='0 0 16 16'>  <path d='M8.982 1.566a1.13 1.13 0 0 0-1.96 0L.165 13.233c-.457.778.091 1.767.98 1.767h13.713c.889 0 1.438-.99.98-1.767L8.982 1.566zM8 5c.535 0 .954.462.9.995l-.35 3.507a.552.552 0 0 1-1.1 0L7.1 5.995A.905.905 0 0 1 8 5zm.002 6a1 1 0 1 1 0 2 1 1 0 0 1 0-2z'/></svg>" +
                                                    "<span class = 'ms-2'> " + aktifDuruslar + "</span></div>";
                    }



                    var sn_DurusBiten = �retim.T3_UretimDurus.Where(p => p.Baslangic != p.Bitis).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
                    var sn_DurusDevamEden = �retim.T3_UretimDurus.Where(p => p.Baslangic == p.Bitis).Sum(p => (DateTime.Now - p.Baslangic).TotalSeconds);

                    var sn_Planl�DurusBiten = �retim.T3_UretimPlanliDurus.Where(p => p.Baslangic != p.Bitis && p.Bitis < zaman).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
                    var sn_Planl�DurusDevamEden = �retim.T3_UretimPlanliDurus.Where(p => p.Baslangic != p.Bitis && p.Bitis > zaman).Sum(p => (zaman - p.Baslangic).TotalSeconds);


                    TimeSpan sn_planl�durus = TimeSpan.FromSeconds(sn_Planl�DurusBiten + sn_Planl�DurusDevamEden);
                    TimeSpan sn_durus = TimeSpan.FromSeconds(sn_DurusBiten + sn_DurusDevamEden);
                    TimeSpan sn_�al��ma = TimeSpan.FromSeconds(sn_Personel�al��maBiten + sn_Personel�al��maDevamEden);

                    string stil_Calisma = "text-bg-light";
                    string stil_Durus = "text-bg-light";
                    string stil_Mola = "text-bg-light";

                    if (sn_Planl�DurusDevamEden > 0) stil_Mola = "text-bg-info";
                    if (sn_DurusDevamEden > 0) stil_Durus = "text-bg-danger";
                    if (sn_Personel�al��maDevamEden > 0) stil_Calisma = "text-bg-primary";

                    icerik += "<div class='card  m-1 p-0'> " +
                                        "   <div class='card-header " + stil_Baslik + "'> " + �retim.Kod + "</div> " +
                                        "   <div class='card-body p-0'> " +
                                                aktifPersonellerPanel +
                                                aktifDuruslarPanel +
                                        "       <div class='row p-0 m-0'>  " +
                                        "           <div class='card text-bg-dark m-1 p-0 col'> " +
                                        "               <div class='card-header p-1'>Zaman</div> " +
                                        "               <div class='card-body p-0 ps-2 text-center'> " +
                                        "                   <i class='bi bi-align-start'></i> " +
                                        "                   " + �retim.Baslangic.ToString("HH:mm") + "   <br /> " +
                                        "                   <i class='bi bi-align-end'></i> " +
                                        "                   " + �retim.BitisHedef.ToString("HH:mm") +
                                        "               </div> " +
                                        "           </div>  " +
                                        "           <div class=' card " + stil_Calisma + " m-1 p-0 col-3'> " +
                                        "               <div class='card-header p-1'>�al��ma</div> " +
                                        "               <div class='card-body p-0'> " +
                                        "                   <p class='text-center fs-1 m-0'> " + sn_�al��ma.ToString(@"hh\:mm\:ss", null) + " </p> " +
                                        "               </div> " +
                                        "           </div>  " +
                                        "           <div class=' card " + stil_Durus + "  m-1 p-0 col-3'> " +
                                        "               <div class='card-header p-1'>Duru�</div> " +
                                        "               <div class='card-body p-0'> " +
                                        "                   <p class='text-center fs-1 m-0'>" + sn_durus.ToString(@"hh\:mm\:ss", null) + "</p> " +
                                        "               </div> " +
                                        "           </div>  " +
                                        "           <div class=' card " + stil_Mola + "  m-1 p-0 col-3'> " +
                                        "               <div class='card-header p-1'> Mola </div> " +
                                        "               <div class='card-body p-0'> " +
                                        "                   <p class='text-center fs-1 m-0'>" + sn_planl�durus.ToString(@"hh\:mm\:ss", null) + "</p> " +
                                        "               </div> " +
                                        "           </div> " +
                                        "       </div> " +
                                        "   </div> " +
                                        "</div> ";

                    aktif�retimModel += icerik;

                    list_Uretim.Add(new()
                    {
                        Kod = �retim.Kod,
                        Barkod = �retim.Barkod,
                        Baslangic = �retim.Baslangic,
                        Bitis = �retim.Bitis,
                        DkHedef = �retim.SureHedef,
                        DkGer�ek = �retim.SureGercek,
                        DkHedefG = �retim.SureHedefG,
                        Product = �retim.Urun.Tip.Product,
                        Rated_Volt = �retim.Urun.Tip.Rated_Volt,
                        Panel_Width = �retim.Urun.Tip.Panel_Width,
                        Panel_Type = �retim.Urun.Tip.Panel_Type,
                        Panel_Curr = �retim.Urun.Tip.Panel_Curr,
                        Shortc_Curr = �retim.Urun.Tip.Shortc_Curr,
                        Ct = �retim.Urun.Tip.Ct,
                        Vt = �retim.Urun.Tip.Vt,
                        Vt_With = �retim.Urun.Tip.Vt_With,
                        Vt_Rem = �retim.Urun.Tip.Vt_Rem,
                        Vt_Fix = �retim.Urun.Tip.Vt_Fix,
                        Sa = �retim.Urun.Tip.Sa,
                        Cap_Volt_Ind = �retim.Urun.Tip.Cap_Volt_Ind,
                        Es_Present = �retim.Urun.Tip.Es_Present,
                        Es_Type_Subs = �retim.Urun.Tip.Es_Type_Subs,
                        Ct_Sec_Con = �retim.Urun.Tip.Ct_Sec_Con,
                        Vt_Sec_Con = �retim.Urun.Tip.Vt_Sec_Con,
                        ProjectId = �retim.Urun.ProjectId,
                        ProjectName = �retim.Urun.ProjectName,
                        Switchgear = �retim.Urun.Switchgear,
                        Panel_No = �retim.Urun.Panel_No,
                        Shortc_Time = �retim.Urun.Shortc_Time,
                        Discon = �retim.Urun.Discon,
                        Durum = durum,
                        DurumKod = aktifPersoneller + aktifDuruslar,
                    });
                }
                 
                aktuel = �al��ma.Aktuel;
                delta = �al��ma.Delta;

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

            var istasyonDuru�Listesi = �al��ma.Istasyon.T3_IstasyonDurus.Where(p => p.Baslangic == p.Bitis).ToList();

            if (istasyonDuru�Listesi.Count > 0)
            {
                aktif�retimModel = "<div class='card m-1 p-0'> " +
                                        "   <div class='card-body p-0'> " +
                                        "       <div class='row p-0 m-0'>  ";

                foreach (var durus in istasyonDuru�Listesi)
                {
                    TimeSpan sn_durus = (zaman - durus.Baslangic);

                    string icerik = "<div class='card  text-bg-danger m-1 p-0 col'> " +
                             "   <div class='card-header p-1'>" + durus.Kod + "</div> " +
                             "   <div class='card-body p-0'> " +
                             "          <p class='text-center fs-1 m-0'>" + sn_durus.ToString(@"hh\:mm\:ss", null) + "</p> " +
                             "   </div> " +
                             "</div>  ";

                    aktif�retimModel += icerik;
                }
                aktif�retimModel += "       </div> " +
                                    "   </div> " +
                                    "</div> ";
            }


            bool online = List_Online_Cihazlar.Where(p => p.Id == �al��ma.IstasyonId).Any();

            SayfaModel model = new()
            {
                Istasyon = �al��ma.Istasyon.Kod,
                Durum = "",
                Online = online,
                AktifUretimModel = aktif�retimModel,
                DurusUretimModel = durus�retimModel,
                Calisma = new CalismaModel
                {
                    Kod = �al��ma.Kod,
                    Bas =
                          �al��ma.Baslangic.ToShortTimeString(),
                    Bit =
                          �al��ma.Bitis.ToShortTimeString(),
                    Hedef = �al��ma.Hedef,
                    Aktuel = aktuel,
                    Delta = delta,
                },
                ListUretim = list_Uretim,
            };

            return model;
        }
        public DashboardModel Dashboard(T3_Calisma �al��ma)
        {
            DateTime Zaman = DateTime.Now;
            var dashboard = new DashboardModel();

            var list_TimeLine = new List<string[]>();

            var �al��ma_ba�lang�� = �al��ma.Baslangic;
            var �al��ma_biti� = �al��ma.Bitis;

            foreach (var mola in �al��ma.T3_CalismaPlanliDurus.OrderBy(p => p.Baslangic))
            {
                var �ncesindeki_mola = �al��ma.T3_CalismaPlanliDurus.Where(p => p.Id != mola.Id && p.Baslangic < mola.Baslangic)
                                                                    .OrderByDescending(p => p.Baslangic)
                                                                    .FirstOrDefault();
                if (�ncesindeki_mola != null)
                    �al��ma_ba�lang�� = �ncesindeki_mola.Bitis;

                list_TimeLine.Add(new string[] { "Planl� �al��ma", �al��ma.Kod, "", "darkgreen", �al��ma_ba�lang��.ToString("yyyy-MM-dd HH:mm:ss"), mola.Baslangic.ToString("yyyy-MM-dd HH:mm:ss") });
                list_TimeLine.Add(new string[] { "Planl� �al��ma", mola.Kod, "", "orange", mola.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), mola.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            var sonmola = �al��ma.T3_CalismaPlanliDurus.OrderByDescending(p => p.Baslangic)
                                                       .FirstOrDefault();
            if (sonmola != null)
                list_TimeLine.Add(new string[] { "Planl� �al��ma", �al��ma.Kod, "", "darkgreen", sonmola.Bitis.ToString("yyyy-MM-dd HH:mm:ss"), �al��ma.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });
            else
                list_TimeLine.Add(new string[] { "Planl� �al��ma", �al��ma.Kod, "", "darkgreen", �al��ma.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), �al��ma.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });

            �al��ma_ba�lang�� = �al��ma.Baslangic;
            �al��ma_biti� = �al��ma.Bitis;

            List<UretimOlay> list_Olay = new List<UretimOlay>();
            List<UretimOlay> list_Istasyon_Bos = new List<UretimOlay>();

            foreach (var bo�ta in �al��ma.T3_CalismaIstasyonBosZaman)
            {
                var bo�_biti� = bo�ta.Bitis;
                if (bo�ta.Baslangic == bo�ta.Bitis)
                    bo�_biti� = Zaman;

                list_Istasyon_Bos.Add(new UretimOlay
                {
                    Id = bo�ta.Id,
                    Baslangic = bo�ta.Baslangic,
                    Bitis = bo�_biti�,
                    Kod = "Bo�",
                    Tip = 10
                });
                list_TimeLine.Add(new string[] { "�stasyon Bo� Zaman", "Bo�", "", "darkgray", bo�ta.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), bo�_biti�.ToString("yyyy-MM-dd HH:mm:ss") });
            }

            foreach (var �retim in �al��ma.Istasyon.T3_Uretim.OrderBy(p => p.Bitis))
            {

                var �retim_ba�lang�� = �retim.Baslangic;
                var �retim_biti� = �retim.Bitis;

                List<UretimOlay> list_Olay_Uretim = new List<UretimOlay>();
                List<UretimOlay> list_Olay_Personel = new List<UretimOlay>();
                foreach (var personel_�al��ma in �retim.T3_UretimPersonel.Where(p => p.Baslangic >= �al��ma_ba�lang��).OrderBy(p => p.Baslangic))
                {
                    var personel_�al��ma_Biti� = personel_�al��ma.Bitis;
                    if (personel_�al��ma.Baslangic == personel_�al��ma.Bitis)
                        personel_�al��ma_Biti� = Zaman;

                    list_Olay_Personel.Add(new UretimOlay
                    {
                        Id = personel_�al��ma.Id,
                        UretimId = �retim.Id,
                        Tip = 1,
                        TipKod = �retim.Kod,
                        Renk = "blue",
                        Kod = personel_�al��ma.Personel.Ad + " " + personel_�al��ma.Personel.Soyad,
                        Baslangic = personel_�al��ma.Baslangic,
                        Bitis = personel_�al��ma_Biti�,
                    });
                }

                if (list_Olay_Personel.Count == 0)
                    list_Olay_Uretim.Add(new()
                    {
                        Id = �retim.Id,
                        UretimId = �retim.Id,
                        Kod = �retim.Kod,
                        Baslangic = �retim.Baslangic,
                        Bitis = �retim.Bitis,
                        Renk = "blue",
                        Tip = 1,
                        TipKod = �retim.Kod,
                    });
                else
                    list_Olay_Uretim.AddRange(list_Olay_Personel.Merge());

                List<UretimOlay> list_Olay_Durus = new();
                foreach (var durus in �retim.T3_UretimDurus.Where(p => p.Baslangic >= �al��ma_ba�lang��).OrderBy(p => p.Baslangic))
                {
                    var durus_Biti� = durus.Bitis;
                    if (durus.Baslangic == durus.Bitis)
                        durus_Biti� = Zaman;

                    list_Olay_Durus.Add(new UretimOlay
                    {
                        Id = durus.Id,
                        UretimId = �retim.Id,
                        Tip = 2,
                        TipKod = �retim.Kod,
                        Renk = "red",
                        Kod = durus.Kod,
                        Baslangic = durus.Baslangic,
                        Bitis = durus_Biti�,
                    });
                }
                list_Olay_Uretim.AddRange(list_Olay_Durus.Merge());

                List<UretimOlay> list_Olay_Planl�Durus = new();
                foreach (var planlidurus in �retim.T3_UretimPlanliDurus.OrderBy(p => p.Baslangic))
                {
                    var durus_Biti� = planlidurus.Bitis;
                    if (planlidurus.Baslangic == planlidurus.Bitis)
                        durus_Biti� = Zaman;

                    list_Olay_Planl�Durus.Add(new UretimOlay
                    {
                        Id = planlidurus.Id,
                        UretimId = �retim.Id,
                        Tip = 3,
                        TipKod = �retim.Kod,
                        Renk = "orange",
                        Kod = planlidurus.Kod,
                        Baslangic = planlidurus.Baslangic,
                        Bitis = durus_Biti�
                    });
                }

                list_Olay_Uretim.AddRange(list_Olay_Planl�Durus.Merge());

                foreach (var item in list_Olay_Uretim)
                {
                    list_TimeLine.Add(new string[] { item.TipKod, item.Kod, item.Tooltip, item.Renk, item.Baslangic.ToString("yyyy-MM-dd HH:mm:ss"), item.Bitis.ToString("yyyy-MM-dd HH:mm:ss") });
                }

                list_Olay.AddRange(list_Olay_Uretim);
            }

            dashboard.TimeLine = list_TimeLine;
            dashboard.UretimSayisi = �al��ma.Istasyon.T3_Uretim.Count;

            if (�al��ma_biti� > Zaman)
                �al��ma_biti� = Zaman;

            foreach (var personel_�al��ma in list_Olay.Where(p => p.Tip == 1))
            {
                var personel_�al��ma_biti� = personel_�al��ma.Bitis;

                if (personel_�al��ma.Baslangic == personel_�al��ma.Bitis)
                    personel_�al��ma_biti� = Zaman;

                int saniye = 0;
                saniye = (int)(personel_�al��ma_biti� - personel_�al��ma.Baslangic).TotalSeconds;
                var varm� = dashboard.Pie_Personel_Calisma.Where(p => p[0].ToString() == personel_�al��ma.Kod).FirstOrDefault();
                if (varm� == null)
                {
                    dashboard.Pie_Personel_Calisma.Add(new object[]
                    {
                        personel_�al��ma.Kod,
                        saniye,
                        pasta_Tooltip(personel_�al��ma.Kod, "a", saniye, "primary")
                    });
                }
                else
                {
                    varm�[1] = ((int)varm�[1]) + saniye;
                    varm�[2] = pasta_Tooltip(personel_�al��ma.Kod, "a", (int)varm�[1], "primary");
                }
            }

            foreach (var durus in list_Olay.Where(p => p.Tip == 2))
            {
                int saniye = 0;
                var varm� = dashboard.Pie_Durus.Where(p => p[0].ToString() == durus.Kod).FirstOrDefault();
                if (varm� == null)
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
                    varm�[1] = ((int)varm�[1]) + saniye;
                    varm�[2] = pasta_Tooltip(durus.Kod, "a", saniye, "danger");
                }
            }

            int toplam_personel_calisma = (int)list_Olay.Where(p => p.Tip == 1).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
            int toplam_durus = (int)list_Olay.Where(p => p.Tip == 2).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
            int toplam_planli_durus = (int)list_Olay.Where(p => p.Tip == 3).Sum(p => (p.Bitis - p.Baslangic).TotalSeconds);
            int ge�en_zaman = (int)(�al��ma_biti� - �al��ma_ba�lang��).TotalSeconds * dashboard.UretimSayisi;
            int istasyon_bos = list_Istasyon_Bos.Where(p => p.Tip == 10).Sum(p => (int)(p.Bitis - p.Baslangic).TotalSeconds);

            dashboard.Pie = new()
            { 
                //new object[] { "Toplam Zaman", ge�en_zaman },  
                new object[] { "Bo� Zaman", istasyon_bos, pasta_Tooltip("Bo� Zaman", "a", istasyon_bos, "secondary")  },
                new object[] { "�al��ma", toplam_personel_calisma, pasta_Tooltip("�al��ma", "a", toplam_personel_calisma, "primary") },
                new object[] { "Duru�", toplam_durus , pasta_Tooltip("Duru�", "a", toplam_durus, "danger")},
                new object[] { "Planl� Duru�", toplam_planli_durus , pasta_Tooltip("Planl� Duru�", "a", toplam_planli_durus, "warning")},
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
        public string pasta_Tooltip(string ba�l�k, string a��klama, int saniye, string tema)
        {
            string d�nen = "";

            TimeSpan time = TimeSpan.FromSeconds(saniye);
            string zaman = time.ToString(@"hh\:mm\:ss");

            d�nen = "<div class='card text-bg-" + tema + "' style='width: 8rem;'> " +
                        "<div class='card-header'>" + ba�l�k + "</div>" +
                        "<div class='card-footer text-bg-light'>" + zaman + "</div>" +
                    "</div>";
            return d�nen;
        }
    }
}