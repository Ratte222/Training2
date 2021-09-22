//#define FillTable

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
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Training2.AutoMapper;
using Training2.BackgroundService;
using Training2.Middleware;
using MyLoggerLibrary;
using MyLoggerLibrary.Services;
using MyLoggerLibrary.LoggerConfigExtensions;
using MyLoggerLibrary.Formatting;
using MyLoggerLibraryMsSQL;
namespace Training2
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
               .AddJsonFile("google_secret.json").AddConfiguration(configuration);
            // создаем конфигурацию
            Configuration = builder.Build();
            //Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddRazorPages();

            string connection = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<DAL.EF.AppDBContext>(options => options.UseSqlServer(connection), ServiceLifetime.Transient);
            var appSettingsSection = Configuration.GetSection("AppSettings");
            var appSettings = appSettingsSection.Get<AppSettings>();
            var mongoSettingsSection = Configuration.GetSection("MongoDBSettings");
            var mongoSettings = mongoSettingsSection.Get<MongoDBSettings>();
            var googleSettingsSection = Configuration.GetSection("web");
            var googleSettings = googleSettingsSection.Get<GoogleSecret>();
            #region JWT_Setting
            //--------- JWT settingd ---------------------

            services.AddIdentity<Client, IdentityRole>()
               .AddEntityFrameworkStores<DAL.EF.AppDBContext>()
               .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings.
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
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

                options.SignIn.RequireConfirmedEmail = true;
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
            //services.AddRouting();
            services.AddAutoMapper(typeof(AutoMapperProfile));
            #region mongodb
            //https://docs.microsoft.com/ru-ru/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-3.1&tabs=visual-studio
            //https://alexalvess.medium.com/getting-started-with-net-core-api-mongodb-and-transactions-c7a021684d01
            //https://metanit.com/nosql/mongodb/1.2.php
            services.AddSingleton<IMongoClient>(options =>
            {
                return new MongoClient(mongoSettings.ConnectionString);
            });
            //services.AddScoped(c =>
            //    c.GetService<IMongoClient>().StartSession());
            #endregion
            services.AddSingleton<AppSettings>(appSettings);
            services.AddSingleton<MongoDBSettings>(mongoSettings);
            services.AddSingleton<GoogleSecret>(googleSettings);
            services.AddSingleton<SmtpConfig>(Configuration.GetSection("SmtpConfig").Get<SmtpConfig>());
            services.AddScoped<IClientService, ClientService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IProductPhotoService, ProductPhotoService>();
            services.AddScoped<IAnnouncementService, AnnouncementService>();
            services.AddScoped<IMongoRepoArticle, ArticleService>();

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

            #region redis cackhing
            //https://www.youtube.com/watch?v=UrQWii_kfIE
            //https://docs.microsoft.com/ru-ru/aspnet/core/performance/caching/distributed?view=aspnetcore-3.1#distributed-redis-cache
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetConnectionString("Redis");
                options.InstanceName = "Training2_";
            });
            #endregion

            var myLoggerConfig = new LoggerConfiguration()
                .WriteTo.Console()
            .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"logs", "mylog.json"), new JsonFormatter(), RollingInterval.Day)
            .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                @"logs", "mylog.bin"), new BinaryFormatter(), RollingInterval.Day)
            .WriteTo.File(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    @"logs", "mylog.xml"), new XmlFormatter(), RollingInterval.Day)
            .WriteTo.MsSQLServer(connection, "Logs");
            MyLoggerLibrary.Interfaces.ILogger logger = myLoggerConfig.CreateLoggger();
            logger.LogInfo("My first log");
            
            //myLoggerConfig.
            //services.AddHostedService<UpdateRedisService>(provider =>
            //{
            //    return new UpdateRedisService(provider.GetService<IAnnouncementService>(),
            //        provider.GetService<IDistributedCache>());
            //});
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env,
            AppDBContext applicationContext, UserManager<Client> userManager, RoleManager<IdentityRole> roleManager,
            ILogger<Startup> logger)
        {
            applicationContext.Database.Migrate();
            DbInitializer.Initialize(applicationContext, userManager, roleManager);
#if FillTable
            string erroeMessage = DbInitializer.FillMoreRandomData(applicationContext, userManager, 10000);
            logger.LogError(erroeMessage);
            DbInitializer.FillProductPhoto(applicationContext);
#endif
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
            app.UseMiddleware<ExeptionMeddleware>();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapRazorPages();
            });
        }
    }
}
