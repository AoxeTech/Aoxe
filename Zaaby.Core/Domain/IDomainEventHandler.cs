namespace Zaaby.Core.Domain
{
    public interface IDomainEventHandler<in TDomainEvent, TId> where TDomainEvent : IDomainEvent<TId>
    {
        void Handle(TDomainEvent domainEvent);
    }
}