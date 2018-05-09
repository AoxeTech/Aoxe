using Zaaby.Core.Domain;

namespace FinanceDomain.AggregateRoots
{
    public class Account : IAggregateRoot<string>
    {
        public string Id { get; protected set; }
    }
}