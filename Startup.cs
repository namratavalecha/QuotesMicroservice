using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using PolicyAdmin.QuotesMS.API.Data;
using PolicyAdmin.QuotesMS.API.Repository;
using PolicyAdmin.QuotesMS.API.DataLayer;
using PolicyAdmin.QuotesMS.API.Services;
using PolicyAdmin.QuotesMS.API.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;

namespace PolicyAdmin.QuotesMS.API
{
    public class Startup
    {
        static readonly log4net.ILog _log4net = log4net.LogManager.GetLogger("RollingFile");
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            _log4net.Info("Initializing Program");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            //if (Configuration.GetValue<bool>("InMemoryDatabase"))
            //{
            //    services.AddDbContext<QuotesContext>(options => options.UseInMemoryDatabase("PolicyAdmin_Quotes"));
            //    _log4net.Info("Initialized In-Memory DB");
            //}
            //else
            //{
            //    try
            //    {
            //        services.AddDbContext<QuotesContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Database")));
            //        _log4net.Info("Initialized SQL DB");
            //    }
            //    catch (Exception e)
            //    {
            //        _log4net.Error("Error In Connection to Databse : " + e.Message);
            //    }

            //}
            services.AddHttpClient();
            services.AddTransient<IQuotesDBService,QuotesDBService>();
            services.AddTransient<IQuoteRepository,QuoteRepository>();
            services.AddDbContext<QuotesContext>(options =>
               options.UseSqlServer("Server=tcp:mftepas.database.windows.net,1433;Initial Catalog=PolicyAdministrationSystem;Persist Security Info=False;User ID=mftepas;Password=project#123;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"));

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //    .AddJwtBearer(opt =>
            //    {
            //        opt.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            ValidIssuer = Configuration["Jwt:Issuer"],
            //            ValidAudience = Configuration["Jwt:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Secret"]))
            //        };
            //    });
            services.AddCors(c => c.AddPolicy("POD_2_Policy", builder =>
            {
                builder.AllowAnyOrigin();
                builder.AllowAnyMethod();
                builder.AllowAnyHeader();
            }));

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1.0", new OpenApiInfo { Title = "Web API", Version = "1.0" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // var scope = app.ApplicationServices.CreateScope();
            // var context = scope.ServiceProvider.GetRequiredService<QuotesContext>();
            // DataGenerator.Initialize(context);
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors("Policy Administration System");
            app.UseSwagger();
            // app.UseMiddleware<JWTMiddleware>();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Web API (V 1.0)");

            });

            // loggerFactory.AddLog4Net();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
