using _01_DbModel.Db;
using System.Collections.Generic;

namespace _01_ServerServis
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private IServiceProvider _serviceProvider;

        public Worker(ILogger<Worker> logger,
                      IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            bool hata = false;
            var eskiZaman = DateTime.Now;
            var ilk_Loop = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                var Zaman = DateTime.Now;
                try
                {
                    if (!ilk_Loop)
                    {
                        if (Zaman.Hour != eskiZaman.Hour)
                            hata = false;

                        var db = _serviceProvider.CreateScope().ServiceProvider.GetRequiredService<XModel>();

                        try
                        {
                            foreach (var ist in db.T3_Istasyon.ToList())
                            {
                                ist.Zaman = Zaman;
                                await db.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                    }
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                    hata = false;
                    ilk_Loop = false;
                }
                catch (Exception ex)
                {
                    if (!hata)
                        _logger.LogError(ex.ToString());
                    hata = true;
                }
            }
        }
    }
}