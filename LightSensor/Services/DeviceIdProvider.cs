using LightSensor.Interfaces;

namespace LightSensor.Services;

public class DeviceIdProvider : IDeviceIdProvider
{
    private readonly string _deviceId;

    public DeviceIdProvider()
    {
        _deviceId = "12345678-1234-1234-1234-1234567890ab";
    }

    public string GetDeviceId()
    {
        return _deviceId;
    }
}
