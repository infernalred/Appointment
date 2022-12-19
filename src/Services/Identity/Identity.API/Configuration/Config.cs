﻿using Duende.IdentityServer.Models;

namespace Identity.API.Configuration;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("api1", "My API")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },

                AllowedScopes = {"api1"}
            }
        };
}