using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Zaabee.SequentialGuid;
using Zaabee.Serializer.Abstractions;
using Zaaby.Common;
using Zaaby.DDD.Abstractions.Domain;
using Zaaby.DDD.Abstractions.Infrastructure.MessageBus;

namespace Zaaby.DDD
{
    public class ZaabyDddContext : DbContext
    {
        private readonly ITextSerializer _serializer;
        public DbSet<UnpublishedMessage> UnpublishedMessages { get; set; }
        public DbSet<PublishedMessage> PublishedMessages { get; set; }

        public ZaabyDddContext(DbContextOptions options, ITextSerializer serializer) : base(options)
        {
            _serializer = serializer;
        }

        public override int SaveChanges()
        {
            PersistenceDomainEvents();
            return base.SaveChanges();
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            PersistenceDomainEvents();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess,
            CancellationToken cancellationToken = new())
        {
            await PersistenceDomainEventsAsync();
            return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new())
        {
            await PersistenceDomainEventsAsync();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void PersistenceDomainEvents()
        {
            var messages = TakeAwayDomainEvents()
                .Select(domainEvent => new UnpublishedMessage
                {
                    Id = SequentialGuidHelper.GenerateComb(),
                    EventType = domainEvent.GetType().ToString(),
                    Content = _serializer.SerializeToString(domainEvent),
                    PersistenceUtcTime = DateTime.UtcNow
                });
            UnpublishedMessages.AddRange(messages);
        }

        private async Task PersistenceDomainEventsAsync()
        {
            var messages = TakeAwayDomainEvents()
                .Select(domainEvent => new UnpublishedMessage
                {
                    Id = SequentialGuidHelper.GenerateComb(),
                    EventType = domainEvent.GetType().ToString(),
                    Content = _serializer.SerializeToString(domainEvent),
                    PersistenceUtcTime = DateTime.UtcNow
                });
            await UnpublishedMessages.AddRangeAsync(messages);
        }

        private List<IDomainEvent> TakeAwayDomainEvents()
        {
            var domainEntities = ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.DomainEvents is not null && x.Entity.DomainEvents.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.DomainEvents)
                .ToList();

            domainEntities.ForEach(entity => entity.Entity.ClearDomainEvents());
            return domainEvents;
        }
    }
}