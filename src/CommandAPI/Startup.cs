using System;
using CommandAPI.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Npgsql;

namespace CommandAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddNewtonsoftJson(c =>
                {
                    c.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var builder = new NpgsqlConnectionStringBuilder
            {
                ConnectionString = Configuration.GetConnectionString("PostgreSqlConnection"),
                Password = Configuration["Password"],
                Username = Configuration["UserID"]
            };

            services.AddDbContext<CommandContext>(opt =>
            {
                opt.UseNpgsql(builder.ConnectionString);
            });
            services.AddScoped<ICommandApiRepository, SqlCommandApiRepo>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
