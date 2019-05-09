namespace Zaaby.Abstractions
{
    public interface IZaabyMessagePublisher
    {
        void Publish<TMessage>(TMessage message);
    }
}