using AutoMapper;
using FluentValidation;
using MediatR;
using Microservices.Common.Behaviours;
using Microservices.Common.Interfaces;
using Microservices.Common.Localization.DbLocalizer;
using Microservices.Common.Localization.JsonLocalizer;
using Microservices.Common.Middlewares;
using Microservices.Common.Options;
using Microservices.Common.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Microservices.Common
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCommon(this IServiceCollection services, Assembly assembly, SsoOAuth2Options? ssoOAuth2Options = null)
        {
            //AutoMapper
            services.AddSingleton(provider => new MapperConfiguration(cfg => cfg.AddMaps(Assembly.GetExecutingAssembly())).CreateMapper());
            services.AddAutoMapper(assembly);

            //FluentValidation
            services.AddValidatorsFromAssembly(assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            //MediatR
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));

            services.AddHttpContextAccessor();
            services.AddScoped<ICurrentUser, CurrentUser>();

            //Localization
            services.AddLocalization();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            Type? iApplicationDbContextType = assembly.GetTypes().SingleOrDefault(x => x.Name == "IApplicationDbContext");
            if (iApplicationDbContextType != default)
            {
                services.AddSingleton<IDatabaseLocalizer, DatabaseLocalizer>(sp =>
                    new DatabaseLocalizer((IApplicationDbContextBase)sp.GetRequiredService(iApplicationDbContextType)));
            }

            services.AddCors();

            if (ssoOAuth2Options != null)
            {
                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        //disable HTTPS validation only on dev/test
                        //options.RequireHttpsMetadata = false;
                        //options.BackchannelHttpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback = delegate { return true; } };

                        options.Authority = ssoOAuth2Options.Authority;

                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateAudience = false,
                        };
                    });
            }


            services.AddControllers();

            services.AddSwagger(assembly, ssoOAuth2Options);

            return services;
        }
        public static IApplicationBuilder UseCommon(this IApplicationBuilder builder, Assembly assembly)
        {
            //1. custom middlewares
            builder.UseCustomMiddleWares();

            //2. CORS extra configs
            builder.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            //3. swagger
            builder.UseSwagger(assembly);

            //4. common .NET
            builder.UseHttpsRedirection();
            builder.UseRouting();
            builder.UseAuthentication();
            builder.UseAuthorization();

            return builder;
        }

        #region Private Functions

        private static IServiceCollection AddSwagger(this IServiceCollection services, Assembly assembly, SsoOAuth2Options? ssoOAuth2Options = null)
        {
            services.AddSwaggerGen(c =>
            {
                c.CustomSchemaIds(type => type.FullName);
                c.EnableAnnotations();
                c.SwaggerDoc("v1", new OpenApiInfo { Title = assembly.GetName().Name, Version = "v1" });
                //note: see how to configure Postman to auto-generate the token here: https://stackoverflow.com/a/58197959/249336

                if (ssoOAuth2Options != null)
                {
                    c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            ClientCredentials = new OpenApiOAuthFlow
                            {
                                TokenUrl = new Uri($"{ssoOAuth2Options.Authority}/connect/token"),
                                Scopes = new Dictionary<string, string>
                            {
                                 { "*", "All APIs" },
                            }
                            },
                            //Password = new OpenApiOAuthFlow
                            //{
                            //    TokenUrl = new Uri($"{Startup.SsoOAuth2Options.Authority}/connect/token"),
                            //    Scopes = new Dictionary<string, string>
                            //    {
                            //         { "*", "All APIs" },
                            //    }
                            //}
                        },
                        Scheme = JwtBearerDefaults.AuthenticationScheme,
                        In = ParameterLocation.Header,
                    });
                }

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = JwtBearerDefaults.AuthenticationScheme
                                }
                            },
                            Array.Empty<string>()
                    }
                });

                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.DocInclusionPredicate((name, api) => true);

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{assembly.GetName().Name}.xml");
                c.IncludeXmlComments(filePath);
                c.DescribeAllParametersInCamelCase();//ensures all parameters are mapped from camelCase in client to PascalCase in server
            });

            return services;
        }

        private static IApplicationBuilder UseSwagger(this IApplicationBuilder builder, Assembly assembly)
        {
            builder.UseSwagger();
            builder.UseSwaggerUI(c =>
            {
                string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", $"{assembly.GetName().Name} v1");
            });

            return builder;
        }

        #endregion
    }
}
