using PgElasticSync.Elastic;

namespace PgElasticSync.Services
{
    public class SubscribeBackgroundService : BackgroundService
    {
        // контейнер DI
        private readonly IServiceProvider _services;
        private readonly RedisService _redisService;

        public SubscribeBackgroundService(IServiceProvider services, RedisService redisService)
        {
            _services = services;
            _redisService = redisService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _redisService.Subscribe<IndexEmployeeEvent>("create", async x => 
            {
                // для каждого события необходимо создавать некоторый новый скоуп - 
                // хранилище всех зарегистрированных сервисов, чтобы мы не обращались к Disposed-сервисам.
                using var scope = _services.CreateScope();
                var elasticRepository = scope.ServiceProvider.GetRequiredService<ElasticRepository>();
                await elasticRepository.IndexDocument(x.Employee);
            });

            await _redisService.Subscribe<IndexEmployeeEvent>("update", async x => 
            {
                using var scope = _services.CreateScope();
                var elasticRepository = scope.ServiceProvider.GetRequiredService<ElasticRepository>();
                await elasticRepository.UpdateDocument(x.Employee);
            });
        }
    }
}