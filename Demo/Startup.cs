using Demo.Interfaces;
using Demo.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using RoomBooking.WebApi;
using RoomBooking.WebApi.Common;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;


namespace Demo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        readonly string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            IConfigurationRoot objConfig = new ConfigurationBuilder()
                   .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                   .AddJsonFile("appsettings.json")
                   .Build();

            services.AddTransient(typeof(IEmployeeService), typeof(EmployeeServices));
            services.AddTransient(typeof(IDocuments), typeof(Documents));
            services.AddSwaggerGen();
            services.AddControllers();
            services.AddCors(options =>
            {
                if (AppSettings.IsDevlopent == 1)
                {

                    options.AddPolicy(MyAllowSpecificOrigins,
                  builder =>
                  {
                      builder.AllowCredentials();
                      builder.WithExposedHeaders("Content-Disposition");
                      builder.WithOrigins("*", "http://localhost:4200", "http://172.16.1.227:1011");
                      //builder.WithOrigins(AppSettings.ApiOrigins); //Pending = site access,issuedby,authorised domain include in this
                      //builder.WithHeaders("user_Id", "LOGIN_ID", "COMPANY_ID", "TOKEN_NO", "Content-Type");
                      builder.WithHeaders("Content-Type");
                      builder.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS");
                  });
                }
                else
                {

                    options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        
                        builder.AllowCredentials();
                        builder.WithOrigins("*", "https://localhost:44351", "http://localhost:4200", "http://172.16.1.227:1011");                                                          
                        builder.WithExposedHeaders("Content-Disposition");                                                                            
                        builder.WithHeaders("Content-Type");
                        builder.WithMethods("GET", "POST", "PUT", "DELETE", "OPTIONS"); 
                    });
                }
            });





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
            app.UseStaticFiles();
            app.UseCors("AllowAnyCorsPolicy");
            app.UseAuthentication();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("api", "{controller=values}/{action}");
            });
            app.UseCors(x => x
             .AllowAnyMethod()
             .AllowAnyHeader()
             .SetIsOriginAllowed(origin => true) // allow any origin
             );
            // app.UseCors(MyAllowSpecificOrigins);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI V1");
                c.DefaultModelsExpandDepth(-1);
                c.DocExpansion(DocExpansion.None);
            });

          
           // app.UseHttpsRedirection();


        }
    }
}
