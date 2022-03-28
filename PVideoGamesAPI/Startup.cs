using CarsAPI.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using PVideoGamesAPI.Data;
using PVideoGamesAPI.GamesMapper;
using PVideoGamesAPI.Repository;
using PVideoGamesAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PVideoGamesAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        public object ValidateIssuerSinging { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<ApplicationDbContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            
            services.AddScoped<IRepositoryCategory, RepositoryCategory>();
            services.AddScoped<IRepositoryVideoGame, RepositoryVideoGame>();
            services.AddScoped<IRepositoryRequirements, RepositoryRequirements>();
            services.AddScoped<IRepositoryUser, RepositoryUser>();

            //Dependecia del TOKEN

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,

                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.
                    GetBytes(Configuration.GetSection("AppSettings:Token").Value)),

                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            }
            );


            services.AddAutoMapper(typeof(GamesMappers));

            services.AddControllers();

            services.AddCors();

            //Documentancion
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("APICategory", new OpenApiInfo
                { 
                    Title = "API Category",
                    Version = "v1",
                    Description = "backend Videos Games",
                    Contact = new OpenApiContact()
                    {
                        Email = "Franklynbrea100@gmail.com",
                        Name = "Franklyn Brea",
                        Url = new Uri("https://google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT Lincese",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }                    
                });
                c.SwaggerDoc("APIVideoGame", new OpenApiInfo
                {
                    Title = "API Video Games",
                    Version = "v1",
                    Description = "backend Videos Games",
                    Contact = new OpenApiContact()
                    {
                        Email = "Franklynbrea100@gmail.com",
                        Name = "Franklyn Brea",
                        Url = new Uri("https://google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT Lincese",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });
                c.SwaggerDoc("APIRequeriments", new OpenApiInfo
                {
                    Title = "API Requeriments",
                    Version = "v1",
                    Description = "backend Videos Games",
                    Contact = new OpenApiContact()
                    {
                        Email = "Franklynbrea100@gmail.com",
                        Name = "Franklyn Brea",
                        Url = new Uri("https://google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT Lincese",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });
                c.SwaggerDoc("APIUsers", new OpenApiInfo
                {
                    Title = "API Users",
                    Version = "v1",
                    Description = "backend Videos Games",
                    Contact = new OpenApiContact()
                    {
                        Email = "Franklynbrea100@gmail.com",
                        Name = "Franklyn Brea",
                        Url = new Uri("https://google.com")
                    },
                    License = new OpenApiLicense()
                    {
                        Name = "MIT Lincese",
                        Url = new Uri("https://en.wikipedia.org/wiki/MIT_License")
                    }
                });

                var Comment = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var ruta = Path.Combine(AppContext.BaseDirectory, Comment);
                c.IncludeXmlComments(ruta);
                c.AddSecurityDefinition("Bearer",
                     new OpenApiSecurityScheme
                     {
                         Description = "Autenticacion JWT (Bearer)",
                         Type = SecuritySchemeType.Http,
                         Scheme = "Bearer"
                     });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement{
                     {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        }, new List<string>()
                    }
                });
                
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/APIVideoGame/swagger.json", "API Video Games v1");
                    c.SwaggerEndpoint("/swagger/APICategory/swagger.json", "API Category v1");
                    c.SwaggerEndpoint("/swagger/APIRequeriments/swagger.json", "API Requerimientos v1");
                    c.SwaggerEndpoint("/swagger/APIUsers/swagger.json", "API Users v1");

                });


            }
            else
            {
                app.UseExceptionHandler(builder => {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var error = context.Features.Get<IExceptionHandlerFeature>();

                        if (error != null)
                        {
                            context.Response.AddAplicationError(error.Error.Message);
                            await context.Response.WriteAsync(error.Error.Message);
                        }

                    });
                });

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/APIVideoGame/swagger.json", "API Video Games v1");
                    c.SwaggerEndpoint("/swagger/APICategory/swagger.json", "API Category v1");
                    c.SwaggerEndpoint("/swagger/APIRequeriments/swagger.json", "API Requerimientos v1");
                    c.SwaggerEndpoint("/swagger/APIUsers/swagger.json", "API Users v1");

                    c.RoutePrefix = "";
                });

            }



            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

        }
    }
}
