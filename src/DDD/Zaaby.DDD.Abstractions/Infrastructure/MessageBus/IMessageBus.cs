using System.Threading.Tasks;

namespace Zaaby.DDD.Abstractions.Infrastructure.MessageBus
{
    public interface IMessageBus
    {
        void Publish<T>(string topic, T message);
        Task PublishAsync<T>(string topic, T message);
    }
}