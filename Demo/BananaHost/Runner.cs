using BananaServices;
using IBananaServices;
using Zaabee.RabbitMQ.Abstractions;

namespace BananaHost
{
    public class Runner
    {
        private readonly IZaabeeRabbitMqClient _zaabeeRabbitMqClient;
        private readonly IBananaService _bananaService;

        //public Runner(IZaabeeRabbitMqClient zaabeeRabbitMqClient, IBananaService bananaService)
        //{
        //    _zaabeeRabbitMqClient = zaabeeRabbitMqClient;
        //    _bananaService = bananaService;
        //    Subscribe();
        //}

        public void Subscribe()
        {
            //_zaabeeRabbitMqClient.SubscribeEvent<BananaMessage>(_bananaService.Consume);
        }
    }
}