using Domain.Common;
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
            builder.Property(s => s.Name)
                   .IsRequired()
                   .HasMaxLength((int)MaxLengthSize.Name);
        }
    }
}