using LightSensor.Interfaces;
using Microsoft.IdentityModel.Clients.ActiveDirectory;

namespace LightSensor.Services;

public class AuthenticationService : IAuthenticationService
{
    private static string clientId = "20971897-24f1-4b4c-98dc-4dc0bb3ad7b4";
    private static string authority = "https://login.microsoftonline.com/57b53338-2c7a-4d8d-a1a3-7367d0c93abc/";
    private static string clientSecret = "2Kd8Q~vkImqUEJIOSGcCgagn1zRUAcpmK4dzobjd";
    private static string resource = "api://483941bd-515d-42a4-bdce-6f9639388088";

    public async Task<string> GetAccessTokenAsync()
    {
        AuthenticationContext context = new AuthenticationContext(authority);
        ClientCredential clientCredential = new ClientCredential(clientId, clientSecret);
        var result = await context.AcquireTokenAsync(resource, clientCredential);
        return result.AccessToken;
    }
}