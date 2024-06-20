using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Repository.EntityConfigurations;
using Aoxe.Serializer.Abstractions;
using Aoxe.DDD;

namespace Repository;

public sealed class CustomDbContext : AoxeDddContext
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