using System;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Amazon.S3;
using System.Linq;
using MyBeers.Common.CommonInterfaces;
using MyBeers.Common.Dispatchers;
using System.Reflection;
using MyBeers.Common.MongoSettings;
using System.Collections.Generic;
using MyBeers.Api.Queries;
using MyBeers.Utilities;
using MyBeers.BeerLib.Seed.Commands;
using MyBeers.RatingLib.QueryHandlers;
using MyBeers.UserLib.CommandHandlers;
using MyBeers.ListLib.CommandHandlers;
using MyBeers.UserLib.Api.Queries;
using MyBeers.Migration.Api.Commands;
using MyBeers.Migration.CommandHandlers;

namespace MyBeers.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public List<Assembly> Assemblies = new List<Assembly>
        {
            typeof(CreateListCommandHandler).GetTypeInfo().Assembly,
            typeof(CreateUserCommandHandler).GetTypeInfo().Assembly,
            typeof(RatingsQueryHandler).GetTypeInfo().Assembly,
            typeof(SeedBeerCommand).GetTypeInfo().Assembly,
            typeof(AuthenticateUserQuery).GetTypeInfo().Assembly,
            typeof(MigrateDataCommandHandler).GetTypeInfo().Assembly
        };

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options => {
                options.AddPolicy("AllowAllOrigins", builder => builder.AllowAnyOrigin());
                options.AddPolicy("AllowAllMethods", builder => builder.AllowAnyMethod());
                options.AddPolicy("AllowAllHeaders", builder => builder.AllowAnyHeader());
            });

            services.AddScoped(typeof(IMongoRepository<>), typeof(Repository<>));

            services.AddControllers().AddNewtonsoftJson();

            services.Configure<Utils.DBSettings>(
                Configuration.GetSection(nameof(Utils.DBSettings)));
            services.AddSingleton<Utils.IDBSettings>(sp =>
                sp.GetRequiredService<IOptions<Utils.DBSettings>>().Value);


            services.Configure<Common.MongoSettings.DBSettings>(Configuration.GetSection(nameof(Common.MongoSettings.DBSettings)));
            services.AddSingleton<Common.MongoSettings.IDBSettings>(sp =>
                sp.GetRequiredService<IOptions<Common.MongoSettings.DBSettings>>().Value);

            services.AddRouting(opt => opt.LowercaseUrls = true);

            services.AddHttpContextAccessor();

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new OpenApiInfo {Title = "MyBeers API", Version = "v1" });
            });

            //JWT secrets get
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            var key = Encoding.ASCII.GetBytes(appSettings.Secret);

            //Jwt Setup
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = context =>
                    {
                        var queryDispatcher = context.HttpContext.RequestServices.GetRequiredService<IQueryDispatcher>();
                        var userId = context.Principal.Identity.Name;
                        var user = queryDispatcher.Dispatch<UserQuery, UserQuery.User>(new UserQuery { Id = userId });
                        if (user == null)
                        {
                            // return unauthorized if user no longer exists
                            context.Fail("Unauthorized");
                        }
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            
            var commandHandlers = Assemblies.SelectMany(x => x.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)));

            foreach (var handler in commandHandlers)
            {
                services.AddScoped(handler.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(ICommandHandler<>)), handler);
            }

            var queryHandlers = Assemblies.SelectMany(x => x.GetTypes())
                .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)));

            foreach (var handler in queryHandlers)
            {
                services.AddScoped(handler.GetInterfaces()
                    .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IQueryHandler<,>)), handler);
            }


            services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            services.AddScoped<IQueryDispatcher, QueryDispatcher>();

            services.AddMvc();
            services.AddDefaultAWSOptions(Configuration.GetAWSOptions("AWSOptions"));
            services.AddAWSService<IAmazonS3>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
                app.UseDeveloperExceptionPage();
            //}

            app.UseCors(sp => sp
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseSwagger();

            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "MyBeers API");
            });


            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        //private static void AddCommandQueryHandlers(this IServiceCollection services, Type handlerInterface)
        //{
        //    var handlers = typeof(Startup).Assembly.GetTypes()
        //        .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface)
        //    );

        //    foreach (var handler in handlers)
        //    {
        //        services.AddScoped(handler.GetInterfaces().First(i => i.IsGenericType && i.GetGenericTypeDefinition() == handlerInterface), handler);
        //    }
        //}
    }
}
