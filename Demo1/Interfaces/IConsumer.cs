namespace Interfaces
{
    public interface IConsumer
    {
    }

    public interface IConsumer<in TMessage> : IConsumer
    {
        void Consume(TMessage message);
    }
}