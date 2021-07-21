using Domain.Common;
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

			base.OnModelCreating(modelBuilder);
		}

		public new virtual DbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
		{
			return base.Set<TEntity>();
		}
	}
}