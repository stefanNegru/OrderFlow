using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderFlow.Domain.Inventory;
using OrderFlow.Domain.Products;

namespace OrderFlow.Infrastructure.Persistence.Configurations;

public sealed class InventoryItemConfiguration : IEntityTypeConfiguration<InventoryItem>
{
    public void Configure(EntityTypeBuilder<InventoryItem> builder)
    {
        builder.ToTable("inventory_items");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Quantity)
            .IsRequired();

        builder.HasIndex(x => x.ProductId)
            .IsUnique();

        builder.HasOne<Product>()
            .WithOne()
            .HasForeignKey<InventoryItem>(
                x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
