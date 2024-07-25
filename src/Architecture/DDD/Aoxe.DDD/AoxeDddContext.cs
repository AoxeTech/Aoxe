namespace Aoxe.DDD;

public class AoxeDddContext(DbContextOptions options, ITextSerializer serializer)
    : DbContext(options)
{
    public DbSet<AoxeUnpublishedMessage> UnpublishedMessages { get; set; } = default!;
    public DbSet<AoxePublishedMessage> PublishedMessages { get; set; } = default!;

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

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await PersistenceDomainEventsAsync();
        return await base.SaveChangesAsync(cancellationToken);
    }

    public override async Task<int> SaveChangesAsync(
        bool acceptAllChangesOnSuccess,
        CancellationToken cancellationToken = default
    )
    {
        await PersistenceDomainEventsAsync();
        return await base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
    }

    private void PersistenceDomainEvents()
    {
        var messages = TakeAwayDomainEvents()
            .Select(
                domainEvent =>
                    new AoxeUnpublishedMessage(
                        SequentialGuidHelper.GenerateComb(),
                        domainEvent.GetType().ToString(),
                        serializer.ToText(domainEvent),
                        DateTime.UtcNow
                    )
            );
        UnpublishedMessages.AddRange(messages);
    }

    private async Task PersistenceDomainEventsAsync()
    {
        var messages = TakeAwayDomainEvents()
            .Select(
                domainEvent =>
                    new AoxeUnpublishedMessage(
                        SequentialGuidHelper.GenerateComb(),
                        domainEvent.GetType().ToString(),
                        serializer.ToText(domainEvent),
                        DateTime.UtcNow
                    )
            );
        await UnpublishedMessages.AddRangeAsync(messages);
    }

    private List<IDomainEvent> TakeAwayDomainEvents()
    {
        var entityEntries = ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Any())
            .ToList();

        var domainEvents = entityEntries.SelectMany(x => x.Entity.DomainEvents).ToList();

        entityEntries.ForEach(entity => entity.Entity.ClearDomainEvents());
        return domainEvents;
    }
}
