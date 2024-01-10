using _05_AdminUI.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();


var connectionString = builder.Configuration.GetConnectionString("T3IdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'T3IdentityContextConnection' not found.");

builder.Services.AddDbContext<T3IdentityContext>(options =>
    options.UseSqlite(connectionString));

builder.Services.AddDefaultIdentity<T3IdentityUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
    .AddEntityFrameworkStores<T3IdentityContext>();

builder.Services.Configure<IdentityOptions>(options =>
{

    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 3;
    options.Password.RequiredUniqueChars = 0;

    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.AddAuthorization(x => x.AddPolicy("AdminPolicy",
                                       policy => policy.RequireClaim("Yetki", "T3")));

builder.Services.AddAuthorization(x => x.AddPolicy("UrunTipPolicy", 
                                       policy => policy.RequireClaim("Sayfa", "UrunTip")));

builder.Services.AddAuthorization(x => x.AddPolicy("DurusTipPolicy",
                                       policy => policy.RequireClaim("Sayfa", "DurusTip")));

builder.Services.AddAuthorization(x => x.AddPolicy("UrunPolicy",
                                       policy => policy.RequireClaim("Sayfa", "Urun")));

builder.Services.AddAuthorization(x => x.AddPolicy("RaporPolicy",
                                       policy => policy.RequireClaim("Sayfa", "Rapor")));

builder.Services.AddAuthorization(x => x.AddPolicy("DashboardDetayPolicy",
                                       policy => policy.RequireClaim("Sayfa", "DashboardDetay")));

builder.Services.AddAuthorization(x => x.AddPolicy("KullanýcýPolicy",
                                       policy => policy.RequireClaim("Sayfa", "Kullanýcý")));

builder.Services.AddAuthorization(x => x.AddPolicy("VardiyaPolicy",
                                       policy => policy.RequireClaim("Sayfa", "Vardiya")));

builder.Services.AddAuthorization(x => x.AddPolicy("IstasyonPolicy",
                                       policy => policy.RequireClaim("Sayfa", "Istasyon")));

builder.Services.AddAuthorization(x => x.AddPolicy("PersonelPolicy",
                                       policy => policy.RequireClaim("Sayfa", "Personel")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.MapRazorPages();

InitializeDatabase(app);

app.Run();


void InitializeDatabase(IApplicationBuilder app)
{
    if (app.ApplicationServices != null)
#pragma warning disable CS8602 // Dereference of a possibly null reference.
        using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
        {
            var context = scope.ServiceProvider.GetService<T3IdentityContext>();
            context.Database.Migrate();

            var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<T3IdentityUser>>();

            var t3user = userMgr.FindByNameAsync("t3").Result;
            
            if (t3user == null)
            {
                t3user = new T3IdentityUser
                {
                    UserName = "T3",
                    Email = "support@t3otomasyon.com.tr",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false, 
                };

                var result = userMgr.CreateAsync(t3user, "T3Otomasyon2006").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(t3user, new Claim[]{
                            new Claim("Yetki", "T3"),
                            new Claim("Sayfa", "UrunTip"),
                            new Claim("Sayfa", "DurusTip"),
                            new Claim("Sayfa", "Urun"),
                            new Claim("Sayfa", "Rapor"),
                            new Claim("Sayfa", "DashboardDetay"),
                            new Claim("Sayfa", "Kullanýcý"),
                            new Claim("Sayfa", "Vardiya"),
                            new Claim("Sayfa", "Istasyon"),
                            new Claim("Sayfa", "Personel"),
                        }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            else
            {

            }

            var urunuser = userMgr.FindByNameAsync("urun").Result;

            if (urunuser == null)
            {
                urunuser = new T3IdentityUser
                {
                    UserName = "urun",
                    Email = "support@t3otomasyon.com.tr",
                    EmailConfirmed = true,
                    TwoFactorEnabled = false,
                };

                var result = userMgr.CreateAsync(urunuser, "urun").Result;
                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }

                result = userMgr.AddClaimsAsync(urunuser, new Claim[]{
                            new Claim("Yetki", "T3")  
                        }).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(result.Errors.First().Description);
                }
            }
            else
            {

            }

        }
#pragma warning restore CS8602 // Dereference of a possibly null reference.
}