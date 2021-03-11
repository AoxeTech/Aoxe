using System.Threading.Tasks;

namespace Zaaby.MessageHub.RabbitMQ
{
    public interface IZaabyMessagePublisher
    {
        void Publish<TMessage>(TMessage message);
        Task PublishAsync<TMessage>(TMessage message);
    }
}