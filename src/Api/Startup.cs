using System.Linq;
using System.Net.Mime;
using System.Text.Json;
using Application;
using Application.Common;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Api
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			// TODO: In production use more sensible options
			services.AddCors(options =>
			{
				options.AddDefaultPolicy(
					builder => { builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod(); });
			});

			services.InjectApplicationServices();
			services.AddMyContext(Configuration);
			services.AddMediatR();
			services.AddAutoMapperProfiles();

			services.AddOptions();
			services.AddControllers(options => { options.SuppressAsyncSuffixInActionNames = false; });
			services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Api", Version = "v1"}); });
			services.AddMvc(options => { options.Filters.Add(new ValidationFilter()); }).AddFluentValidation(options =>
			{
				options.RegisterValidatorsFromAssemblyContaining<ApplicationEntrypoint>();
			});

			services.AddHealthChecks()
				.AddSqlServer(
					Configuration.GetConnectionString("DefaultConnection"),
					"SELECT 1;",
					"sql",
					HealthStatus.Degraded,
					new[] {"db", "sql", "sqlserver", "ready"});
		}

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
				app.UseSwagger();
				app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Api v1"));
			}

			app.UseRouting();

			app.UseCors();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();

				endpoints.MapHealthChecks("/health/ready", new HealthCheckOptions
				{
					Predicate = (check) => check.Tags.Contains("ready"),
					ResponseWriter = async (context, report) =>
					{
						var result = JsonSerializer.Serialize(
							new
							{
								status = report.Status.ToString(),
								checks = report.Entries.Select(entry => new
								{
									name = entry.Key,
									status = entry.Value.Status.ToString(),
									exception = entry.Value.Exception != null ? entry.Value.Exception.Message : "none",
									duration = entry.Value.Duration.ToString()
								})
							}
						);

						context.Response.ContentType = MediaTypeNames.Application.Json;
						await context.Response.WriteAsync(result);
					}
				});
				endpoints.MapHealthChecks("/health/live", new HealthCheckOptions
				{
					Predicate = (_) => false
				});
			});
		}
	}
}