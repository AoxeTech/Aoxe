using System.Threading.Tasks;

namespace Zaaby.Abstractions
{
    public interface IZaabyMessagePublisher
    {
        void Publish<TMessage>(TMessage message);
        Task PublishAsync<TMessage>(TMessage message);
    }
}