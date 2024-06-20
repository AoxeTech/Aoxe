using Domain.AggregateRoots;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Repository.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("User");

            builder.HasKey(user => user.Id);

            builder.Ignore(user => user.DomainEvents);

            builder.Property(user => user.Name)
                .HasColumnType("varchar(255)")
                .IsRequired();

            builder.Property(user => user.Age)
                .HasColumnType("integer")
                .IsRequired();

            builder.Property(user => user.Gender)
                .HasColumnType("integer")
                .IsRequired();

            //值类型没有Id，ORM将其转换为主表的多个字段
            builder.OwnsOne(user => user.Address, addressBuilder =>
            {
                addressBuilder.Property(address => address.Country)
                    .HasColumnType("varchar(25)");
                addressBuilder.Property(address => address.State)
                    .HasColumnType("varchar(25)");
                addressBuilder.Property(address => address.City)
                    .HasColumnType("varchar(25)");
                addressBuilder.Property(address => address.Street)
                    .HasColumnType("varchar(25)");
            });

            //集合/数组使用jsonb字段类型
            builder.Property(user => user.Tags)
                .HasColumnType("jsonb")
                .IsRequired();

            //聚合内的子实体ORM会自动在子表中加上对应的外键
            builder.OwnsMany(user => user.Cards, cardBuilder =>
            {
                cardBuilder.ToTable("Card");
                cardBuilder.HasKey(card => card.Id);
                //子实体Id不需要自动生成https://github.com/npgsql/efcore.pg/issues/971
                cardBuilder.Property(card => card.Id).ValueGeneratedNever();
                cardBuilder.Property(card => card.Name)
                    .HasColumnType("varchar(25)")
                    .IsRequired();
            });
        }
    }
}