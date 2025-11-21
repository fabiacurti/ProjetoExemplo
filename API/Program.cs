using System;
using System.Data;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using Data.Interfaces;
using Data.Repositories;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using API.Filters;
using Data.Configuration;
using Business.Interfaces;
using Business.Services;
using Business.Mappings;
using Data.Util;
using Business.Hubs;
using API.Service;
using NLog.Web;

namespace API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Configurar NLog
            builder.Host.UseNLog();
            builder.Host.UseContentRoot(AppContext.BaseDirectory);

            // Add services to the container
            var configuration = builder.Configuration;
            var services = builder.Services;

            services.AddControllers();
            
            var origins = configuration.GetSection("Cors")["Origins"].Split(';');
            services.AddCors(options =>
                options.AddDefaultPolicy(builder => builder.WithOrigins(origins).AllowAnyHeader().AllowAnyMethod()));
            
            services.AddMvc(options =>
            {
                options.Filters.Add<ApplicationFilter>();
            });
            
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
                    Description = "Template padrão para APIs .NET 8",
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

            // Configurar conexões
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            services.AddTransient(provider => new Func<IDbConnection>(() => new SqlConnection(connectionString)));

            // Configurar dependências de dados
            services.AddTransient<IExemploRepository, ExemploRepository>();
            services.AddTransient<IExemploSubItemRepository, ExemploSubItemRepository>();
            services.AddTransient<IExportarExcel, ExportarExcel>();
            services.AddTransient<IUploadViagemRepository, UploadViagemRepository>();

            // Configurar AutoMapper
            services.AddTransient(provider => new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ExemploMapper());
                mc.AddProfile(new ExemploSubItemMapper());
            }).CreateMapper());

            // Configurar serviços da API
            services.AddTransient<IExemploService, ExemploService>();
            services.AddTransient<IExemploSubItemService, ExemploSubItemService>();
            services.AddTransient<IUploadViagemService, UploadViagemService>();

            // Configurar opções
            services.Configure<SmtpEmailConfig>(configuration.GetSection("SmtpEmail"));

            // Adicionar serviço hospedado
            services.AddHostedService<ImportService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline
            app.UseCors();

            if (app.Environment.IsDevelopment())
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
                c.SwaggerEndpoint("/rota/swagger/v1/swagger.json", "API");
                c.DocExpansion(DocExpansion.None);
            });

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
            app.MapHub<ExemploHub>("/exemploHub", o => 
            { 
                o.ApplicationMaxBufferSize = 10000000; 
                o.TransportMaxBufferSize = 10000000; 
            });

            app.Run();
        }
    }
}
