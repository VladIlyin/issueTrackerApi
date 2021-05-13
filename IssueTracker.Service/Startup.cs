using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;

using IssueTracker.EntityFramework.Models;
using IssueTracker.Persistance.Queries;
using IssueTracker.EntityFramework.Queries;

namespace TaskManagerApi
{
    public class Startup
    {
        public static string ConnectionString;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            ConnectionString = Configuration["ConnectionString"];
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "TaskManagerApi", Version = "v1" });
            });

            var connectionString = Configuration["ConnectionString"];
            services.AddDbContext<TaskManagerContext>(options => {
                options.UseNpgsql(connectionString);
            });

            services.AddScoped<IUserGetAllQuery, UserGetAllQuery>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseExceptionHandler("/error")
                 .WithConventions(config =>
                 {
                     config.ContentType = "application/json";
                     config.MessageFormatter(s => JsonConvert.SerializeObject(new
                     {
                         Message = "An error occurred whilst processing your request"
                     }));

                     //config.ForException<BadRequestException>()
                     //               .ReturnStatusCode(400)
                     //               .UsingMessageFormatter((ex, context) => JsonConvert.SerializeObject(new
                     //               {
                     //                   ex.Message
                     //               }));

                     //config.ForException<NotFoundException>()
                     //               .ReturnStatusCode(404)
                     //               .UsingMessageFormatter((ex, context) => JsonConvert.SerializeObject(new
                     //               {
                     //                   ex.Message
                     //               }));

                 });

            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManagerApi v1"));

            app.UseRouting();

            //app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private void AddIdentity(IServiceCollection services)
        //{
        //    services.AddDbContext<IdentityDbContext>(config =>
        //        config.UseInMemoryDatabase("DevIdentity"));

        //    services.AddIdentity<IdentityUser, IdentityRole>(
        //        options =>
        //        {
        //            options.Password.RequireDigit = false;
        //            options.Password.RequiredLength = 3;
        //            options.Password.RequireLowercase = false;
        //            options.Password.RequireUppercase = false;
        //        })
        //        .AddEntityFrameworkStores<IdentityDbContext>();
        //}
    }
}
