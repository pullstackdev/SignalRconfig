using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UdemySignalR.API.Hubs;
using UdemySignalR.API.Models;

namespace UdemySignalR.API
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

            services.AddDbContext<AppDbContext>(options => //dbcontext sayfasýndaki yeni ctor yöntemi bunu gerektirdi
            {
                options.UseSqlServer(Configuration["ConStr"]);
            });

            //cors ekle
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder => //CorsPolicy ne istersen o ismi ver
                {
                    builder.WithOrigins("https://localhost:44325").AllowAnyHeader().AllowAnyMethod().AllowCredentials(); //bu url'e izin ver (web/mvc url)
                });
            });

            services.AddControllers();
            services.AddSignalR(); //kullanacaðý için eklenmeli
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

            app.UseCors("CorsPolicy"); //auth üztüne bu eklenmeli

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                // http://localhost:4400/MyHub //clientlar bu url ile hub'a baðlanmýþ olacak
                endpoints.MapHub<MyHub>("/MyHub"); //middleware eklendi
            });
        }
    }
}
