using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.Property(o => o.CustomerId)
				.IsRequired();
			builder.Property(o => o.IsActive)
				.HasDefaultValue(0)
				.ValueGeneratedOnAdd();
			builder.Property(o => o.CreatedTime)
				.HasDefaultValueSql("GETUTCDATE()")
				.ValueGeneratedOnAdd()
				.IsRequired();
			builder
				.HasOne<Customer>(o => o.Customer)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.CustomerId);
			builder
				.HasOne<Product>(o => o.Product);

			// TODO: auto-update UpdatedTime column (use trigger)
		}
	}
}