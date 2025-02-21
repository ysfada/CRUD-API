﻿using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
	public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
	{
		/// Tablo oluşturabilmek için bu şekilde Configurationları hazırlamamız gerekiyor. 
		public void Configure(EntityTypeBuilder<Customer> builder)
		{
			builder.Property(c => c.FirstName)
				.IsRequired()
				.HasMaxLength((int) MaxLengthSize.Name);
			builder.Property(c => c.LastName)
				.IsRequired()
				.HasMaxLength((int) MaxLengthSize.Name);
			builder.Property(c => c.IsActive)
				.HasDefaultValue(0)
				.ValueGeneratedOnAdd();
			builder.Property(c => c.CreatedTime)
				.HasDefaultValueSql("GETUTCDATE()")
				.ValueGeneratedOnAdd()
				.IsRequired();

			// TODO: auto-update UpdatedTime column (use trigger)
		}
	}
}