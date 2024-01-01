// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;

namespace FreeCourse.IdentityServer
{
    public static class Config
    {
        public static IEnumerable<ApiResource> ApiResources => new ApiResource[]                    // Audience icine yazdiririq -> ApiResource ile 
        { // Bunu StartUp faylinda qeydiyyatdan kecirt AddInMemoryApiResource(Config.ApiResource)
            new ApiResource("resource_catalog"){Scopes={"catalog_fullpermission"}},                 // catalog_fullpermission --> Audience icine yazir
            new ApiResource("resource_photo_stock"){Scopes={"photo_stock_fullpermission"}},          // photo_stock_fullpermission  --> Audience icine yazir
            new ApiResource("resource_basket"){Scopes={"basket_fullpermission"}},          // photo_stock_fullpermission  --> Audience icine yazir
            new ApiResource("resource_discount"){Scopes={"discount_fullpermission"}},          // photo_stock_fullpermission  --> Audience icine yazir
            new ApiResource(IdentityServerConstants.LocalApi.ScopeName)                             // Identity Server ozune icaze verir 
        };

        public static IEnumerable<IdentityResource> IdentityResources =>   // Icazeler --> Userin hansi melumatlarina icaze veririk WebApiler baxa bilsin onu yaziriq
                   new IdentityResource[]
                   {
                       new IdentityResources.Email(),
                       new IdentityResources.OpenId(),
                       new IdentityResources.Profile(),
                       new IdentityResource(){Name = "roles" ,DisplayName ="Roles" ,
                           Description="User roles",UserClaims=new []{"role"} }
                   };

        public static IEnumerable<ApiScope> ApiScopes =>                                /// Icazeleri yaziriq 
            new ApiScope[]
            {
                new ApiScope("catalog_fullpermission","Catalog Api for full permission"),                  // catalog_fullpermission -a icaze verir 
                new ApiScope("photo_stock_fullpermission","Photo Stock for Api full permission"),          // photo_stock_fullpermission - a icaze verir
                new ApiScope("basket_fullpermission","Basket for Api full permission"),          // photo_stock_fullpermission - a icaze verir
                new ApiScope("discount_fullpermission","Discount for Api full permission"),          // photo_stock_fullpermission - a icaze verir
                new ApiScope(IdentityServerConstants.LocalApi.ScopeName)                                   // IdentityServer ozune icaze verir 
            };

        public static IEnumerable<Client> Clients =>
            new Client[]
            {
                new Client  // Code Passwordsuz giren istifadeciler ucun 
                {
                   ClientName = "ASP NET CORE MVC",
                   ClientId = "WebMvcClient",   // Ozumuz ID teyin edirik
                   ClientSecrets = {new Secret ("secret".Sha256())},    // Ozumuz sifre teyin edirik
                   AllowedGrantTypes = GrantTypes.ClientCredentials,    // Grant Type -> Yeni tipi de ClientCredentials olmalidir bu tipli token ucun
                   AllowedScopes={ "catalog_fullpermission", "photo_stock_fullpermission", IdentityServerConstants.LocalApi.ScopeName }  // Client kime icaze verecek 
                },
                 new Client
                {
                   ClientName = "ASP NET CORE MVC",
                   ClientId = "WebMvcClientForUser",
                   AllowOfflineAccess = true,
                   ClientSecrets = {new Secret ("secret".Sha256())},
                   AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                   AllowedScopes={"basket_fullpermission","discount_fullpermission",IdentityServerConstants.StandardScopes.Email,IdentityServerConstants.StandardScopes.OpenId
                   ,IdentityServerConstants.StandardScopes.Profile , IdentityServerConstants.StandardScopes.OfflineAccess, IdentityServerConstants.LocalApi.ScopeName ,"roles"},
                    AccessTokenLifetime= 3600, // second
                    RefreshTokenExpiration = TokenExpiration.Absolute,  //Deqiq tarix vermek ucun yaziriq
                     AbsoluteRefreshTokenLifetime = (int)(DateTime.Now.AddDays(60)-DateTime.Now).TotalSeconds,
                     RefreshTokenUsage =TokenUsage.ReUse // Yeniden istifade olunan Refresh Token edirik
                 }

            };
    }
}