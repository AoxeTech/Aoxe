using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Repository.EntityConfigurations;
using Zaabee.Serializer.Abstractions;
using Zaaby.DDD;

namespace Repository;

public sealed class CustomDbContext : ZaabyDddContext
{
    public DbSet<User> Users { get; set; }

    public CustomDbContext(DbContextOptions<CustomDbContext> options, ITextSerializer serializer) : base(options,
        serializer)
    {
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
    }
}