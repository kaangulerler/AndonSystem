using _01_DbModel.Db;
using _02_Api.Hubs;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

IConfiguration configuration = new ConfigurationBuilder()
                            .AddJsonFile("appsettings.json")
                            .Build();
  
builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp",
                          builder =>
                          {
                              builder.AllowAnyHeader()
                                       .AllowAnyMethod()
                                       .SetIsOriginAllowed((x) => true)
                                       .AllowCredentials();
                               
                          });
});

builder.Services.AddDbContext<XModel>(options =>
                                      options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSignalR();



builder.Services.AddControllers().AddJsonOptions(x =>
                    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseCors("corsapp");
 
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapHub<SoketHub>("/soketHub");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<XModel>();

    dbContext.Database.Migrate();

    var lokasyonVarmý = dbContext.T3_Lokasyon.ToList().Any();

    var id = new Guid();
     
    if (!lokasyonVarmý)
    {

        id = Guid.Parse("00000000-0000-0000-0000-000000000001");

        dbContext.T3_Lokasyon.Add(new T3_Lokasyon
        {
            Id = id,
            Kod = "ZS-1", 
            T3_Istasyon = new List<T3_Istasyon> {
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000102"),
                    Kod = "ZS1-SA1",
                    IpAdres = "",
                    Barkod = "F100102",
                    LokasyonId = id,
                    Sira = 1,
                    SiraNo = 102,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000101"),
                    Kod = "ZS1-FA1",
                    IpAdres = "",
                    Barkod = "F100101",
                    LokasyonId = id,
                    Sira = 2,
                    SiraNo = 101,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000103"),
                    Kod = "ZS1-FA2",
                    IpAdres = "",
                    Barkod = "F100103",
                    LokasyonId = id,
                    Sira = 3,
                    SiraNo = 103,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000104"),
                    Kod = "ZS1-FA3",
                    IpAdres = "",
                    Barkod = "F100104",
                    LokasyonId = id,
                    Sira = 4,
                    SiraNo = 104,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000105"),
                    Kod = "ZS1-FA4",
                    IpAdres = "",
                    Barkod = "F100105",
                    LokasyonId = id,
                    Sira = 5,
                    SiraNo = 105,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000106"),
                    Kod = "ZS2-FA5",
                    IpAdres = "",
                    Barkod = "F100106",
                    LokasyonId = id,
                    Sira = 6,
                    SiraNo = 106,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000107"),
                    Kod = "ZS1-FA6",
                    IpAdres = "",
                    Barkod = "F100107",
                    LokasyonId = id,
                    Sira = 7,
                    SiraNo = 107,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000108"),
                    Kod = "ZS1-FA7",
                    IpAdres = "",
                    LokasyonId = id,
                    Barkod = "F100108",
                    Sira = 8,
                    SiraNo = 108,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000109"),
                    Kod = "ZS1-FA8",
                    IpAdres = "",
                    Barkod = "F100109",
                    LokasyonId = id,
                    Sira = 9,
                    SiraNo = 109,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                }
            }

        });

        id = Guid.Parse("00000000-0000-0000-0000-000000000002");

        dbContext.T3_Lokasyon.Add(new T3_Lokasyon
        {
            Id = id,
            Kod = "ZS-2", 
            T3_Istasyon = new List<T3_Istasyon> {
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000121"),
                    Kod = "ZS2-FA1",
                    IpAdres = "",
                    Barkod = "F100121",
                    LokasyonId = id,
                    Sira = 21,
                    SiraNo = 121,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000122"),
                    Kod = "ZS2-FA2-R5",
                    IpAdres = "",
                    Barkod = "F100122",
                    LokasyonId = id,
                    Sira = 22,
                    SiraNo = 122,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000123"),
                    Kod = "ZS2-FA2-R6",
                    IpAdres = "",
                    Barkod = "F100123",
                    LokasyonId = id,
                    Sira = 23,
                    SiraNo = 123,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000124"),
                    Kod = "ZS2-FA3",
                    IpAdres = "",
                    Barkod = "F100124",
                    LokasyonId = id,
                    Sira = 24,
                    SiraNo = 124,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000125"),
                    Kod = "ZS2-FA4",
                    IpAdres = "",
                    Barkod = "F100125",
                    LokasyonId = id,
                    Sira = 25,
                    SiraNo = 125,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000126"),
                    Kod = "ZS2-FA5",
                    IpAdres = "",
                    Barkod = "F100126",
                    LokasyonId = id,
                    Sira = 26,
                    SiraNo = 126,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000127"),
                    Kod = "ZS2-FA6",
                    IpAdres = "",
                    Barkod = "F100127",
                    LokasyonId = id,
                    Sira = 27,
                    SiraNo = 127,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000128"),
                    Kod = "ZS2-FA7",
                    IpAdres = "",
                    Barkod = "F100128",
                    LokasyonId = id,
                    Sira = 28,
                    SiraNo = 128,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000129"),
                    Kod = "ZS2-FA8",
                    IpAdres = "",
                    Barkod = "F100129",
                    LokasyonId = id,
                    Sira = 29,
                    SiraNo = 129,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000130"),
                    Kod = "ZS2-FA9",
                    IpAdres = "",
                    Barkod = "F100130",
                    LokasyonId = id,
                    Sira = 30,
                    SiraNo = 130,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                }
            }

        });

        id = Guid.Parse("00000000-0000-0000-0000-000000000003");

        dbContext.T3_Lokasyon.Add(new T3_Lokasyon
        {
            Id = id,
            Kod = "MWS", 
            T3_Istasyon = new List<T3_Istasyon> {
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000141"),
                    Kod = "MWS-eP-1030",
                    IpAdres = "",
                    Barkod = "F100141",
                    LokasyonId = id,
                    Sira = 41,
                    SiraNo = 141,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000142"),
                    Kod = "MWS-eP-520",
                    IpAdres = "",
                    Barkod = "F100142",
                    LokasyonId = id,
                    Sira = 42,
                    SiraNo = 142,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000143"),
                    Kod = "MWS-TRUMPF",
                    IpAdres = "",
                    Barkod = "F100143",
                    LokasyonId = id,
                    Sira = 43,
                    SiraNo = 143,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000144"),
                    Kod = "MWS-Cu-PUNCH",
                    IpAdres = "",
                    Barkod = "F100144",
                    LokasyonId = id,
                    Sira = 46,
                    SiraNo = 144,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000145"),
                    Kod = "MWS-Cu-FREZE",
                    IpAdres = "",
                    Barkod = "F100145",
                    LokasyonId = id,
                    Sira = 47,
                    SiraNo = 145,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000146"),
                    Kod = "MWS-Cu-BÜKME",
                    IpAdres = "",
                    Barkod = "F100146",
                    LokasyonId = id,
                    Sira = 48,
                    SiraNo = 146,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000147"),
                    Kod = "MWS-PRIMA-SGE",
                    IpAdres = "",
                    Barkod = "F100147",
                    LokasyonId = id,
                    Sira = 44,
                    SiraNo = 147,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                },
                new T3_Istasyon
                {
                    Id = Guid.Parse("00000000-0000-0000-0000-000000000148"),
                    Kod = "MWS-PRIMA-CE",
                    IpAdres = "",
                    Barkod = "F100148",
                    LokasyonId = id,
                    Sira = 45,
                    SiraNo = 148,
                    LokasyonTree = "",
                    Reset = 0,
                    SnSorgu = 0,
                    Zaman = DateTime.Now,
                }
            }
        });

        dbContext.SaveChanges();
    }

    var vardiyaVarmý = dbContext.T3_Vardiya.ToList().Any();
    if (!vardiyaVarmý)
    {
        var vardId = Guid.NewGuid();
        var baþlangýç = TimeSpan.Parse("08:00");
        var bitiþ = TimeSpan.Parse("18:00");

        dbContext.T3_Vardiya.Add(new T3_Vardiya
        {
            Id = vardId,
            Kod = "A Vardiyasý",
            Baslangic = baþlangýç,
            Bitis = bitiþ,
            Zaman = (int)((bitiþ - baþlangýç).TotalMinutes),
            T3_VardiyaMola = new List<T3_VardiyaMola>
            {
                new T3_VardiyaMola
                {
                    Id = Guid.NewGuid(),
                    Kod = "Çay Sabah",
                    Baslangic = TimeSpan.Parse("10:00"),
                    Bitis = TimeSpan.Parse("10:15"),
                    VardiyaId = vardId,
                    Zaman = 15,
                },
                new T3_VardiyaMola
                {
                    Id = Guid.NewGuid(),
                    Kod = "Yemek",
                    Baslangic = TimeSpan.Parse("12:00"),
                    Bitis = TimeSpan.Parse("13:00"),
                    VardiyaId = vardId,
                    Zaman = 60,
                },
                new T3_VardiyaMola
                {
                    Id = Guid.NewGuid(),
                    Kod = "Çay Akþam",
                    Baslangic = TimeSpan.Parse("15:00"),
                    Bitis = TimeSpan.Parse("15:15"),
                    VardiyaId = vardId,
                    Zaman = 15,
                }
            }
        });

        await dbContext.SaveChangesAsync();
    }

    id = Guid.Parse("00000001-0000-0000-0000-000000000000");

    var personelVarmý = dbContext.T3_Personel.Where(p=>p.Id == id).ToList().Any();

    if (!personelVarmý)
    {
        var barkod = "F0" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "12345",
            Ad = "T3",
            Soyad = "Otomasyon",
            Barkod = barkod
        });

        //ZS1 Personelleri Default

        id = Guid.Parse("00001001-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1001",
            Ad = "Necati",
            Soyad = "Zengin",
            Barkod = barkod
        });

        id = Guid.Parse("00001002-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1002",
            Ad = "Ayhan",
            Soyad = "YAMAN",
            Barkod = barkod
        });

        id = Guid.Parse("00001003-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1003",
            Ad = "Yekta Arda",
            Soyad = "MÝNAZ",
            Barkod = barkod
        });

        id = Guid.Parse("00001004-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1004",
            Ad = "Özcan",
            Soyad = "COÞKUN",
            Barkod = barkod
        });

        id = Guid.Parse("00001005-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1005",
            Ad = "Murat",
            Soyad = "AKIN",
            Barkod = barkod
        });

        id = Guid.Parse("00001006-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1006",
            Ad = "Onur",
            Soyad = "ÇAKIR",
            Barkod = barkod
        });

        id = Guid.Parse("00001007-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1007",
            Ad = "Ramazan",
            Soyad = "ÇATAK",
            Barkod = barkod
        });

        id = Guid.Parse("00001008-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1008",
            Ad = "Ufuk",
            Soyad = "ODABAÞI",
            Barkod = barkod
        });

        id = Guid.Parse("00001009-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1009",
            Ad = "Fatih",
            Soyad = "TAÇ",
            Barkod = barkod
        });

        id = Guid.Parse("00001010-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1010",
            Ad = "Fuat",
            Soyad = "TÜRKOÐLU",
            Barkod = barkod
        });

        id = Guid.Parse("00001011-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1011",
            Ad = "Tanju",
            Soyad = "BAÞER",
            Barkod = barkod
        });

        id = Guid.Parse("00001012-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1012",
            Ad = "Cihan",
            Soyad = "YILDIZ",
            Barkod = barkod
        });

        id = Guid.Parse("00001013-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1013",
            Ad = "Ayhan",
            Soyad = "BULUNTEKÝN",
            Barkod = barkod
        });

        id = Guid.Parse("00001014-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1014",
            Ad = "Hasan",
            Soyad = "KAÞIK",
            Barkod = barkod
        });

        id = Guid.Parse("00001015-0000-0000-0000-000000000000");
        barkod = "F0" + id.ToString().ToUpper().Split("-")[0];
        dbContext.T3_Personel.Add(new T3_Personel()
        {
            Id = id,
            Kod = "1015",
            Ad = "Þükrü",
            Soyad = "KURT",
            Barkod = barkod
        });
         
        await dbContext.SaveChangesAsync();
    }



    var duruþTipVarmý = dbContext.T3_DurusTip.ToList().Any();
    if (!duruþTipVarmý)
    {
        id = Guid.Parse("00000101-0000-0000-0000-000000000000");
        var barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Plansýz Duruþ",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000102-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Mola-1",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000103-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Mola-2 (Namaz)",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000104-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Dizayn Teyit Bekleme",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000105-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Malzeme Bekleme (MWS)", 
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000106-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Malzeme Bekleme (Depo)",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000107-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Malzeme Bekleme (SA)",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000108-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Üretim Hazýrlýk",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000109-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Pano Bekleme",
            DurusTipTree = "",
            Barkod = barkod,
        });


        id = Guid.Parse("00000110-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Hat Arýza",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000111-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Eðitim",
            DurusTipTree = "",
            Barkod = barkod,
        });


        id = Guid.Parse("00000112-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Eksik Tamamlama",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000113-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Proje Dönüþü",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000114-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Robot Kabini Yükleme",
            DurusTipTree = "",
            Barkod = barkod,
        });

        id = Guid.Parse("00000115-0000-0000-0000-000000000000");
        barkod = "F3" + id.ToString().ToUpper().Split("-")[0];

        dbContext.T3_DurusTip.Add(new T3_DurusTip()
        {
            Id = id,
            Kod = "Malzeme Çekme",
            DurusTipTree = "",
            Barkod = barkod,
        });

         
        await dbContext.SaveChangesAsync();
    }
     
    var urunVarmý = dbContext.T3_Urun.Where(p => p.Kod == "MWS").ToList().Any();
    if (!urunVarmý)
    {
        dbContext.T3_Urun.Add(new T3_Urun
        {
            Id = new Guid(),
            Kod = "MWS",
            Barkod = "MWS",
            Tip = new T3_UrunTip
            {
                Id = new Guid(),
                Cap_Volt_Ind = "",
                Ct = 0,
                Ct_Sec_Con = 0,
                Es_Present = "",
                Es_Type_Subs = "",
                Panel_Curr = 0,
                Panel_Type = "",
                Panel_Width = 0,
                Product = "",
                Rated_Volt = 0,
                Sa = 0,
                Shortc_Curr = 0,
                Vt = 0,
                Vt_Fix = 0,
                Vt_Rem = 0,
                Vt_Sec_Con = 0,
                Vt_With = 0,
            },
            Discon = 0,
            Panel_No = "",
            Bom = "",
            ProjectId = 0,
            ProjectName = "",
            Shortc_Time = 0,
            Switchgear = "",
            TipId = new Guid(),
        });

        await dbContext.SaveChangesAsync();

    }
}


app.Run();
