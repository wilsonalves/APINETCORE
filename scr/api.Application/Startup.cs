using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Api.CrossCuting.DependecyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Api.Domain.Security;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace application
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

            ConfigureService.ConfigureDependenciesServices(services);
            ConfigureRepository.ConfigureDependenciesRepository(services);
            services.AddControllers();

            var singningConfigurations = new SigningConfigurations();
            services.AddSingleton(singningConfigurations);

            var tokenConfiguration = new TokenConfiguration();
            new ConfigureFromConfigurationOptions<TokenConfiguration>(Configuration.GetSection("TokenConfigurations"))
            .Configure(tokenConfiguration);
            services.AddSingleton(tokenConfiguration);

            services.AddAuthentication(authoptions =>
            {
                authoptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authoptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(beareoptions =>
            {
                var paramsValidation = beareoptions.TokenValidationParameters;
                paramsValidation.IssuerSigningKey = singningConfigurations.Key;
                paramsValidation.ValidAudience = tokenConfiguration.Audience;
                paramsValidation.ValidIssuer = tokenConfiguration.Issuer;
                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;

            });

            services.AddAuthorization(auth =>
                      {
                          auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
                              .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
                              .RequireAuthenticatedUser().Build());
                      });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "API DOT NET CORE",
                    Description = "Arquitetura DDD",
                    TermsOfService = new Uri("http://www.teste.com.br"),
                    Contact = new OpenApiContact
                    {
                        Name = " Wilson Alves de Almeida",
                        Email = "wilson3943@gmail.com",
                        Url = new Uri("http://www.gooogle.com.br")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "termos de licença",
                        Url = new Uri("http://www.gooogle.com.br")
                    }
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Entre com o Token JWT",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference {
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
            }

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                x.SwaggerEndpoint("/swagger/v1/swagger.json", "API NET CORE");
                x.RoutePrefix = string.Empty;
            });
            app.UseAuthentication();

            //git
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
