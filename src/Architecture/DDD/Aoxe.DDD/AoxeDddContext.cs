namespace Aoxe.DDD;

public class AoxeDddContext : DbContext
{
    private readonly ITextSerializer _serializer;
    public DbSet<UnpublishedMessage> UnpublishedMessages { get; set; } = default!;
    public DbSet<PublishedMessage> PublishedMessages { get; set; } = default!;

    public AoxeDddContext(DbContextOptions options, ITextSerializer serializer)
        : base(options)
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
                    new UnpublishedMessage
                    {
                        Id = SequentialGuidHelper.GenerateComb(),
                        EventType = domainEvent.GetType().ToString(),
                        Content = _serializer.ToText(domainEvent),
                        PersistenceUtcTime = DateTime.UtcNow
                    }
            );
        UnpublishedMessages.AddRange(messages);
    }

    private async Task PersistenceDomainEventsAsync()
    {
        var messages = TakeAwayDomainEvents()
            .Select(
                domainEvent =>
                    new UnpublishedMessage
                    {
                        Id = SequentialGuidHelper.GenerateComb(),
                        EventType = domainEvent.GetType().ToString(),
                        Content = _serializer.ToText(domainEvent),
                        PersistenceUtcTime = DateTime.UtcNow
                    }
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
