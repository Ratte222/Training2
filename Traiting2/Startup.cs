using BLL.Helpers;
using BLL.Interfaces;
using BLL.Services;
using DAL.EF;
using DAL.Model;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Traiting2.AutoMapper;

namespace Traiting2
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
            services.AddControllers();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DAL.EF.AppDBContext>(options => options.UseSqlServer(connection), ServiceLifetime.Transient);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            services.AddSingleton<AppSettings>(appSettings);
            services.AddSingleton<SmtpConfig>(Configuration.GetSection("SmtpConfig").Get<SmtpConfig>());
            #region JWT_Setting
            //--------- JWT settingd ---------------------

            services.AddIdentity<Client, IdentityRole>()
               .AddEntityFrameworkStores<DAL.EF.AppDBContext>()
               .AddDefaultTokenProviders(); ;

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 8;
                options.Password.RequiredUniqueChars = 0;

                // Lockout settings.
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                // User settings.
                options.User.AllowedUserNameCharacters =
                "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

                //options.SignIn.RequireConfirmedEmail = true;
            });
            var key = System.Text.Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
          .AddJwtBearer(options =>
          {
              options.RequireHttpsMetadata = true;//if false - do not use SSl
              options.SaveToken = true;
              options.Events = new JwtBearerEvents()
              {
                  OnAuthenticationFailed = (ctx) =>
                  {
                      if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                      {
                          ctx.Response.StatusCode = 401;
                      }

                      return Task.CompletedTask;
                  },
                  OnForbidden = (ctx) =>
                  {
                      if (ctx.Request.Path.StartsWithSegments("/api") && ctx.Response.StatusCode == 200)
                      {
                          ctx.Response.StatusCode = 403;
                      }

                      return Task.CompletedTask;
                  }
              };
              options.TokenValidationParameters = new TokenValidationParameters
              {
                  // specifies whether the publisher will be validated when validating the token 
                  ValidateIssuer = true,
                  // a string representing the publisher
                  ValidIssuer = appSettings.Issuer,

                  // whether the consumer of the token will be validated 
                  ValidateAudience = true,
                  // token consumer setting 
                  ValidAudience = appSettings.Audience,
                  // whether the lifetime will be validated 
                  ValidateLifetime = true,

                  // security key installation 
                  //IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                  IssuerSigningKey = new SymmetricSecurityKey(key),
                  // security key validation 
                  ValidateIssuerSigningKey = true,
              };
          });
            //--------- JWT settingd ---------------------
            #endregion

            services.AddAutoMapper(typeof(AutoMapperProfile));
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IEmailService, EmailService>();

            #region Swagger
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Training2 API", Version = "v1", });
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Scheme = "bearer",
                    Description = "Please insert JWT token into field"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        new string[] { }
                    }
                });
            });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AppDBContext applicationContext, UserManager<Client> userManager, RoleManager<IdentityRole> roleManager)
        {
            applicationContext.Database.Migrate();
            DbInitializer.Initialize(applicationContext, userManager, roleManager);
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
            });
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
