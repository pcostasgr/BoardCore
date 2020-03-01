using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BoardCore.Helpers;
using BoardCore.Services;
using BoardCore.Repositories;
using BoardCore.Repositories.Interfaces;


namespace BoardCore
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

            services.AddCors(options => options.AddPolicy("MyPolicy", build =>
                {                
                        build
                        .SetIsOriginAllowed((host)=>true)
                        .WithOrigins("http://localhost:3000/")
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                        
                }
            ));


            services.AddControllers();

            var appSettingsSection=Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            var AppSettings=appSettingsSection.Get<AppSettings>();
            var key=Encoding.ASCII.GetBytes(AppSettings.Secret);
            services.AddAuthentication( x => 
            {
                x.DefaultAuthenticateScheme=JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme=JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata=false;
                x.SaveToken=true;
                x.TokenValidationParameters=new TokenValidationParameters
                {
                    ValidateIssuerSigningKey=true,
                    IssuerSigningKey=new SymmetricSecurityKey(key),
                    ValidateIssuer=false,
                    ValidateAudience=false 
                };
            });

          

            services.AddScoped<IUserService,UserService>();
            services.AddTransient<IUsersRepository,UsersRepository>();
            services.AddTransient<IListsRepository, ListsRepository>();
            services.AddTransient<ICardsRepository, CardsRepository>();
            services.AddTransient<ICheckListsRepository, CheckListsRepository>();
            services.AddTransient<ICheckListItemsRepository, CheckListItemsRepository>();

            services.AddApiVersioning(o => {
            o.ReportApiVersions = true;
            o.AssumeDefaultVersionWhenUnspecified = true;
            o.DefaultApiVersion = new ApiVersion(1, 0);
            o.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            /*app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );*/

           app.UseCors("MyPolicy");

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
