﻿using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Core.Builder;
using Umbraco.Web.BackOffice.Filters;
using Umbraco.Web.BackOffice.Security;
using Umbraco.Web.Common.Builder;

namespace Umbraco.Extensions
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AddAllBackOfficeComponents(this IUmbracoBuilder builder)
        {
            return builder
                .AddConfiguration()
                .AddUmbracoCore()
                .AddWebComponents()
                .AddRuntimeMinifier()
                .AddBackOffice()
                .AddBackOfficeIdentity()
                .AddMiniProfiler()
                .AddMvcAndRazor()
                .AddWebServer()
                .AddPreviewSupport()
                .AddHostedServices()
                .AddHttpClients();
        }

        public static IUmbracoBuilder AddBackOffice(this IUmbracoBuilder builder)
        {
            builder.Services.AddAntiforgery();
            builder.Services.AddSingleton<IFilterProvider, OverrideAuthorizationFilterProvider>();

            builder.Services
                .AddAuthentication() // This just creates a builder, nothing more
                // Add our custom schemes which are cookie handlers
                .AddCookie(Core.Constants.Security.BackOfficeAuthenticationType)
                .AddCookie(Core.Constants.Security.BackOfficeExternalAuthenticationType, o =>
                {
                    o.Cookie.Name = Core.Constants.Security.BackOfficeExternalAuthenticationType;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
                });

            // TODO: Need to add more cookie options, see https://github.com/dotnet/aspnetcore/blob/3.0/src/Identity/Core/src/IdentityServiceCollectionExtensions.cs#L45

            builder.Services.ConfigureOptions<ConfigureBackOfficeCookieOptions>();
            return builder;
        }

        public static IUmbracoBuilder AddBackOfficeIdentity(this IUmbracoBuilder builder)
        {
            builder.Services.AddUmbracoBackOfficeIdentity();

            return builder;
        }

        public static IUmbracoBuilder AddPreviewSupport(this IUmbracoBuilder builder)
        {
            builder.Services.AddSignalR();

            return builder;
        }
    }
}