using LightSensor.Interfaces;
using LightSensor.Models;
using System.Text;
using System.Text.Json;

namespace LightSensor.Services;

public class DataSender : IDataSender
{
    private readonly HttpClient httpClient = new HttpClient();
    private readonly IAuthenticationService _authService;
    private readonly string _deviceId;
    private readonly string BaseUri = "https://localhost:7237";
    private readonly string apiVersion = "1";
    private const int MaxRetries = 3;

    public DataSender(IDeviceIdProvider deviceIdProvider, IAuthenticationService authService)
    {
        _deviceId = deviceIdProvider.GetDeviceId();
        _authService = authService;
    }

    public async Task SendDataAsync(List<IlluminanceData> data)
    {
        string token = await _authService.GetAccessTokenAsync();
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        string json = JsonSerializer.Serialize(data);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        string url = $"{BaseUri}/{apiVersion}/devices/{_deviceId}/telemetry";
        HttpResponseMessage response = null;

        for (int attempt = 0; attempt < MaxRetries; attempt++)
        {
            try
            {
                response = await httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                return;
            }
            catch (HttpRequestException e)
            {
                string errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Attempt {attempt + 1} failed: {e.Message}");
                Console.WriteLine($"Server responded with: {errorContent}");

                if (attempt == MaxRetries - 1)
                    throw;
                await Task.Delay(1000);
            }
        }
    }
}
