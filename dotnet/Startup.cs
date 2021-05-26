using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
using Repositories;
using Repositories.IRepositories;
using server.Helpers;
using server.Service;
using Stripe;

namespace server
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
            services.AddDbContext<stripeContext>(options =>
                           options.UseSqlServer(Configuration["ConnectionStrings:DefaultConnection"]),
                           ServiceLifetime.Scoped);

            services.InjectJwtAuthService(Configuration);
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IStripePaymentsRepository, StripePaymentsRepository>();
            services.AddScoped<IAuthorizeCapturePaymentGateway, AuthorizeCapturePaymentGateway>();
             
            StripeConfiguration.AppInfo = new AppInfo
            {
                Name = "stripe-samples/accept-a-payment/prebuilt-checkout-page",
                Url = "https://github.com/stripe-samples",
                Version = "0.0.1",
            };

            StripeConfiguration.ApiKey = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";

            services.Configure<Entity.Configuration.StripeOptions>(options =>
            {
                options.PublishableKey = "pk_test_51Iv3WDH4DR7BOnAWOzy6nvLnX1UiDGmY5EADgtZyDSZiQrtfVZoyG204SLoi9hg7hVTupzad055ssSHNeEOGgLcV002zk9HVRw";
                options.SecretKey = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";
                options.Price = "price_1IvESmH4DR7BOnAWhAVdBjya";
                options.PaymentMethodTypes = "card".Split(",").ToList();
                options.Domain = "http://localhost:4242";
            });

            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(),
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }
            app.UseFileServer();
            app.UseRouting();
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
       
        }
    }
}
