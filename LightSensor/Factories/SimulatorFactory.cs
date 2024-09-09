using LightSensor.Interfaces;
using LightSensor.Services;

namespace LightSensor.Factories;

public class SimulatorFactory
{
    private static IDeviceIdProvider _deviceIdProvider = new DeviceIdProvider();
    private static IAuthenticationService _authenticationService = new AuthenticationService(); 

    public static ISimulator CreateSimulator(string type)
    {
        switch (type)
        {
            case "illuminance":
                IIlluminanceGenerator generator = new IlluminanceGenerator();
                IDataSender sender = new DataSender(_deviceIdProvider, _authenticationService);
                return new SensorSimulator(generator, sender);
            default:
                throw new ArgumentException("Invalid simulator type", nameof(type));
        }
    }
}

