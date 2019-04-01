using MarsRover.PictureLibrary.Infra;
using MarsRover.PictureLibrary.Interfaces;
using MarsRover.PictureLibrary.Options;
using MarsRover.PictureLibrary.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System;
using System.IO;

namespace MarsRover.PictureLibrary
{
    public class Startup
    {
        public Startup(IConfiguration configuration,IHostingEnvironment env)
        {
            Configuration = configuration;
            _env = env;
        }

        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment _env;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var physicalProvider = _env.ContentRootFileProvider;
            var datesFilePath = Configuration["ValidDatesFilePath"];
            services.Configure<DateFileStoreOptions>(o =>
            {
                o.FileProvider = physicalProvider;
                o.FilePath = datesFilePath;
            });

            services.AddSingleton<IValidDatesProvider, ValidDatesProvider>();

            var storagePath = Configuration["ImageStoragePath"];
            services.Configure<ImageStoreOptions>(o =>
            {
                o.FileProvider = physicalProvider;
                o.ImageStoragePath = storagePath;
            });            

            services.AddHttpClient<IPictureStore, PictureStore>();
            services.AddHttpClient<INASAClient, NASAClient>(c => 
            {
                c.BaseAddress = new Uri(Configuration["NASAAPIURL"]);                 
            });           
            services.AddScoped<IPictureRepository, PictureRepository>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // In production, the React files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "client-app/build";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                app.UseHttpsRedirection();
            }

           
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            var storagePath = Configuration["ImageStoragePath"];
            Directory.CreateDirectory(storagePath);
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                Path.Combine(Directory.GetCurrentDirectory(), storagePath)),
                RequestPath = storagePath.Replace("\\", "/").Insert(0, "/")
            });

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "client-app";

                if (env.IsDevelopment())
                {
                    //spa.UseReactDevelopmentServer(npmScript: "start");
                    spa.UseProxyToSpaDevelopmentServer("http://localhost:3000");
                }
            });
        }
    }
}
