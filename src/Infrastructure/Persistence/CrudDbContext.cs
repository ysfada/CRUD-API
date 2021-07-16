﻿using Domain.Common;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
	public class CrudDbContext : DbContext
	{
		public CrudDbContext(DbContextOptions<CrudDbContext> options) : base(options)
		{
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(typeof(CrudDbContext).Assembly);

			modelBuilder.Entity<Order>()
				.HasOne<Customer>(o => o.Customer)
				.WithMany(c => c.Orders)
				.HasForeignKey(o => o.CustomerId);

			modelBuilder.Entity<Order>()
				.HasOne<Product>(o => o.Product);

			base.OnModelCreating(modelBuilder);
		}

		public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
		{
			return base.Set<TEntity>();
		}
	}
}