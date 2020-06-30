using System;
using AutoMapper;
using CargoPlanner.Algos;
using CargoPlanner.API.Db;
using CargoPlanner.API.Mapping;
using CargoPlanner.API.Utility;
using CargoPlanner.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Item = CargoPlanner.API.Dtos.Result.Item;

namespace CargoPlanner.API
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

            services.AddScoped<IGenericRepository<Instance, Guid>, InstanceRepository>();
            services.AddScoped<IGenericRepository<Result, Guid>, ResultRepository>();
            services.AddScoped<IGenericRepository<User, Guid>, UserRepository>();
            services.AddScoped<IAlgo, BestFitAlgo>();

            // AutoMapper
            var mapper = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CargoPlannerMapperProfile());
            }).CreateMapper();
            services.AddSingleton(mapper);

            // CosmosDb
            services.AddDbContext<CargoPlannerContext>(
                o => o.UseCosmos(Configuration["CosmosDb:AccountEndpoint"],
                                 Configuration["CosmosDb:PrimaryKey"],
                                 Configuration["CosmosDb:DatabaseName"]));

            // Swagger
            services.AddSwaggerGen(o =>
            {
                o.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "CargoPlanner API",
                    Version = "v1"
                });
                o.CustomSchemaIds(x => x.FullName);
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseCors(o => o
                             .AllowAnyHeader()
                             .AllowAnyMethod()
                             .AllowAnyOrigin());

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint("/swagger/v1/swagger.json", "CargoPlanner API v1");
            });
        }
    }
}