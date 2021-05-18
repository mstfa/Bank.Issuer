using System;
using System.Collections.Generic;
using Bank.Issuer.API.Mapper;
using Bank.Issuer.Core;
using Bank.Issuer.Data;
using Bank.Issuer.Library.Enums;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Services;
using Bank.Issuer.Services.Base;
using Bank.Issuer.Services.Calculator.Master;
using Bank.Issuer.Services.Calculator.Visa;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bank.Issuer.API
{
    public class Startup
    {
        public delegate ICalculator ServiceResolver(Origin origin);
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<BankIssuerDbContext>(options => options.UseInMemoryDatabase("BankIssuer"));

            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddSingleton(MapsterConfig.GetConfiguredMappingConfig());
            services.AddScoped<MapsterMapper.IMapper, MapsterMapper.Mapper>();
            services.AddScoped<IBankIssuerMapper, BankIssuerMapper>();
            services.AddTransient<Master>();
            services.AddTransient<Visa>();

            services.AddTransient<Func<Origin, ICalculator>>(serviceProvider => key =>
            {
                return key switch
                {
                    Origin.MASTER => serviceProvider.GetService<Master>(),
                    Origin.VISA => serviceProvider.GetService<Visa>(),
                    _ => throw new KeyNotFoundException()
                };
            });

            services.AddTransient<IAccountService, AccountService>();
            services.AddTransient<ICalculationService, CalculationService>();
            services.AddTransient<IPaymentService, PaymentService>();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
