using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Entity.Entities;
using Entity.Helper;
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
using server.Repository;
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
            services.AddScoped<IStripePaymentRepository, StripePaymentRepository>();

            services.AddScoped<IExternalPaymentGW, ExternalPaymentGW>();
            services.AddScoped<IPaymentGWRepository, PaymentGWRepo>();

            var price = "price_1IvESmH4DR7BOnAWhAVdBjya";
            if (price == "price_12345" || price == "" || price == null) {
              Console.WriteLine("You must set a Price ID in .env. Please see the README.");
              Environment.Exit(0);
            }

            StripeConfiguration.AppInfo = new AppInfo
            {
                Name = "stripe-samples/accept-a-payment/prebuilt-checkout-page",
                Url = "https://github.com/stripe-samples",
                Version = "0.0.1",
            };

            services.Configure<Entity.Configuration.StripeOptions>(options =>
            {
                options.PublishableKey = "pk_test_51Iv3WDH4DR7BOnAWOzy6nvLnX1UiDGmY5EADgtZyDSZiQrtfVZoyG204SLoi9hg7hVTupzad055ssSHNeEOGgLcV002zk9HVRw";
                options.SecretKey = "sk_test_51Iv3WDH4DR7BOnAWtWXeiWcg3Ztdl3xnPqLAkvQY9rg2orkMVwxEgPYSh2KKfI2WH5UORaVDbwlcJnZdPAwxkHDS00c06HuETL";
               // options.WebhookSecret = "whsec_U75LXStIJUFKzREIaF4pBJPrwY0ltUeV";
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            //app.UseHttpsRedirection();
            app.UseFileServer();
            app.UseRouting();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());

            // custom jwt auth middleware
            //app.UseMiddleware<JwtMiddleware>();
            //app.UseAuthorization();
            app.UseAuthentication();

            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
