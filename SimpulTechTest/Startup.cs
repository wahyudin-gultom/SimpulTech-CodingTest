using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using SimpulCodingTest.BLL.Implementation;
using SimpulTechTest.BLL.Interfaces;
using SimpulTechTest.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpulTechTest
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
            services.AddDbContext<SimpulTechContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DatabaseConn")));
            
            services.AddControllers();
            services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);
            services.AddScoped<IOwner, BLLOwner>();
            services.AddScoped<IPet, BLLPet>();
            services.AddScoped<IAppointment, BLLAppointment>();

            // configure swagger
            var contact = new OpenApiContact()
            {
                Name = "Simpul Tech",
                Email = "wahyudin.gultom@gmail.com",
                Url = new Uri("https://www.linkedin.com/in/wahyudin.gultom/")
            };

            var license = new OpenApiLicense()
            {
                Name = "Simpul Technology",
                Url = new Uri("https://simpul.tech/")
            };

            var info = new OpenApiInfo()
            {
                Version = "v1",
                Title = "Coding Test Documentation",
                Description = "All List API is Listed Here",
                TermsOfService = new Uri("https://simpul.tech/"),
                Contact = contact,
                License = license
            };

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", info);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SimpulTechTest v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            loggerFactory.AddFile("Log/Log{Date}.log");
        }
    }
}
