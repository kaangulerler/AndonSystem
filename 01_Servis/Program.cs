using _01_DbModel.Db;
using _01_Servis;
using Microsoft.EntityFrameworkCore;

 
IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        var connectionstring = context.Configuration.GetConnectionString("DefaultConnection");
        services.AddHostedService<Worker>();
        services.AddDbContext<XModel>(options => options.UseSqlServer(connectionstring));
    })
    .ConfigureLogging((context, logging) => {
        var env = context.HostingEnvironment;
        var config = context.Configuration.GetSection("Logging");
        logging.AddConfiguration(config);
        logging.AddConsole();
        logging.AddFilter("Microsoft.EntityFrameworkCore", LogLevel.Warning);
    })
    .UseWindowsService() 
    .Build();

await host.RunAsync();