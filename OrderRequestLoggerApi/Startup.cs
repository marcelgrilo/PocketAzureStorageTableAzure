using AzureStorageTableHelper;
using AzureStorageTableHelper.Interfaces;

using Logger.Domain.Entities;
using Logger.Domain.Services;
using Logger.Domain.Services.Interfaces;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace OrderRequestLoggerApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IAzureTableStorage<Order>>(factory =>
            {
                return new AzureTableStorage<Order>(
                    new AzureTableSettings(
                        storageAccount: Configuration["AzureStorageAccount"],
                        storageKey: Configuration["AzureStorageKey"],
                        tableName: Configuration["AzureStorageTableName"],
                        storageConnectionString: Configuration["AzureStorageConnectionString1"]
                    ));
            });
            services.AddScoped<IOrderService, OrderService>();

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc();
        }
    }
}
