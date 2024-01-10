using _01_DbModel.Db;
using _01_ServerServis;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
 


IHost host = Host.CreateDefaultBuilder(args)
    .UseWindowsService()
    .ConfigureServices((context, services) =>
    {
        var connectionstring = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddHostedService<Worker>();
        services.AddDbContext<XModel>(options => options.UseSqlServer(connectionstring));

    })
    .Build();

await host.RunAsync();
