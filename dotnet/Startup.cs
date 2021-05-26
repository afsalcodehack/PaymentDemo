using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;
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
            //var price = Environment.GetEnvironmentVariable("PRICE");
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

            //services.Configure<StripeOptions>(options =>
            //{
            //    options.PublishableKey = Environment.GetEnvironmentVariable("STRIPE_PUBLISHABLE_KEY");
            //    options.SecretKey = Environment.GetEnvironmentVariable("STRIPE_SECRET_KEY");
            //    options.WebhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET");
            //    options.Price = Environment.GetEnvironmentVariable("PRICE");
            //    options.PaymentMethodTypes = Environment.GetEnvironmentVariable("PAYMENT_METHOD_TYPES").Split(",").ToList();
            //    options.Domain = Environment.GetEnvironmentVariable("DOMAIN");
            //});

            services.Configure<StripeOptions>(options =>
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
