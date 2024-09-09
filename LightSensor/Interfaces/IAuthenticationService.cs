namespace LightSensor.Interfaces;

public interface IAuthenticationService
{
    Task<string> GetAccessTokenAsync();
}
