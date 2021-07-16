using Application.Customers.Services;
using Application.Orders.Services;
using Application.Products.Services;
using Infrastructure.Persistence;
using Infrastructure.Persistence.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Common
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection InjectApplicationServices(this IServiceCollection services)
		{
			// Repository register ediyoruz
			services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));

			// Servislerimizi register etmemiz gerekiyor. Aksi taktirde çalışmaz.
			// NOT : Normalde bu şekilde kullanmıyoruz otomatik olarak register edecek yapımız mevcut.
			services.AddScoped<ICustomerService, CustomerService>();
			services.AddScoped<IProductService, ProductService>();
			services.AddScoped<IOrderService, OrderService>();
			return services;
		}


		public static IServiceCollection AddMyContext(this IServiceCollection services,
			IConfiguration configuration)
		{
			// Contextimizi register edip connectionstring bilgilerimizi nereden çekeceğini ve migrationları nereye oluşturacağını belirtiyoruz.
			services.AddScoped<DbContext>(provider => provider.GetService<CrudDbContext>())
				.AddDbContextPool<CrudDbContext>(options =>
					options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
						b => b.MigrationsAssembly("Infrastructure")));

			return services;
		}
	}
}