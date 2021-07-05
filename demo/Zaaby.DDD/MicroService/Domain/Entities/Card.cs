using System;
using Zaaby.DDD.Abstractions.Domain;

namespace Domain.Entities
{
    public class Card : Entity
    {
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }

        public Card(Guid id, string name)
        {
            if (id == Guid.Empty) throw new ArgumentNullException(nameof(id));
            (Id, Name) = (id, name);
        }
    }
}