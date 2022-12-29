using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace Identity.API.Configuration;

public static class Config
{
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new("api1", "My API")
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new()
            {
                ClientId = "client_id",
                ClientSecrets = { new Secret("client_secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"AppointmentsAPI"}
            },
            new()
            {
                ClientId = "client_id_mvc1",
                ClientSecrets = { new Secret("client_secret_mvc".Sha256()) },
                AllowedGrantTypes = GrantTypes.Code,
                AllowedScopes =
                {
                    "AppointmentsAPI",
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                RedirectUris = {"http://localhost:3001/signin-oidc"}
            }
        };

    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new("AppointmentsAPI")
        };

    public static IEnumerable<IdentityResource> IdentityResources =>
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile()
        };
}