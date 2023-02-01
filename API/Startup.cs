using System;
using System.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Data.SqlClient;
using Data.Interfaces;
using Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Filters;
using System.Linq;
using Data.Configuration;
using Business.Interfaces;
using Business.Services;
using Business.Mappings;
using Data.Util;
using Business.Hubs;
using API.Service;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var origins = Configuration.GetSection("Cors")["Origins"].Split(';');
            services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));
            services.AddMvc(options =>
            {
                options.Filters.Add<ApplicationFilter>();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddSignalR(o =>
            {
                o.EnableDetailedErrors = true;
            });
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API",
                    Description = "Template padrão para APIs .NET Core 3.1",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "API",
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                    }
                });
                options.ResolveConflictingActions(x => x.First());
                options.OperationFilter<AddRequiredHeaderParameter>();
            });
            ConfigureDataDependencies(services);
            ConfigureAutoMapper(services);
            ConfigureConnections(services);
            ConfigureAPIServices(services);
            ConfigureOptions(services);

            services.AddHostedService<ImportService>();
        }

        private void ConfigureConnections(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            services.AddTransient(provider => new Func<IDbConnection>(() => new SqlConnection(connectionString)));
        }

        private void ConfigureDataDependencies(IServiceCollection services)
        {
            services.AddTransient<IExemploRepository, ExemploRepository>();
            services.AddTransient<IExemploSubItemRepository, ExemploSubItemRepository>();
            services.AddTransient<IExportarExcel, ExportarExcel>();
            services.AddTransient<IUploadViagemRepository, UploadViagemRepository>();
        }

        private void ConfigureAutoMapper(IServiceCollection services)
        {
            services.AddTransient(provider => new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ExemploMapper());
                mc.AddProfile(new ExemploSubItemMapper());
            }).CreateMapper());
        }

        private void ConfigureAPIServices(IServiceCollection services)
        {
            services.AddTransient<IExemploService, ExemploService>();
            services.AddTransient<IExemploSubItemService, ExemploSubItemService>();
            services.AddTransient<IUploadViagemService, UploadViagemService>();
        }

        private void ConfigureOptions(IServiceCollection services)
        {
            services.Configure<SmtpEmailConfig>(Configuration.GetSection("SmtpEmail"));
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/rota/swagger/{documentName}/swagger.json";
                });
            }
            else
            {
                app.UseHsts();
                app.UseSwagger(c =>
                {
                    c.RouteTemplate = "/swagger/{documentName}/swagger.json";
                });
            }
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/rota/swagger/v1/swagger.json",
                    "API");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ExemploHub>("/exemploHub", o => { o.ApplicationMaxBufferSize = 10000000; o.TransportMaxBufferSize = 10000000; });
            });
        }
    }
}
