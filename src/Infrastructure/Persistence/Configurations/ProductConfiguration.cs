using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	public class ProductConfiguration : IEntityTypeConfiguration<Product>
	{
		public void Configure(EntityTypeBuilder<Product> builder)
		{
			builder.Property(p => p.ProductName)
				.IsRequired()
				.HasMaxLength((int) MaxLengthSize.Name);
			builder.Property(p => p.Price)
				.HasColumnType("money")
				.IsRequired();
			builder.Property(p => p.IsActive)
				.HasDefaultValue(0)
				.ValueGeneratedOnAdd();
			builder.Property(p => p.CreatedTime)
				.HasDefaultValueSql("GETUTCDATE()")
				.ValueGeneratedOnAdd()
				.IsRequired();

			// TODO: auto-update UpdatedTime column (use trigger)
		}
	}
}