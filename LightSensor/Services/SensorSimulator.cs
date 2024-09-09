using LightSensor.Interfaces;
using LightSensor.Models;

namespace LightSensor.Services;

class SensorSimulator : ISimulator
{
    private readonly IIlluminanceGenerator _generator;
    private readonly IDataSender _sender;

    public SensorSimulator(IIlluminanceGenerator generator, IDataSender sender)
    {
        _generator = generator;
        _sender = sender;
    }

    public async Task RunSimulation()
    {
        DateTime startTime = DateTime.UtcNow;

        while (true)
        {
            List<IlluminanceData> hourlyData = _generator.GenerateIlluminanceForHour(startTime);
            await _sender.SendDataAsync(hourlyData);

            startTime = startTime.AddHours(1);
            await Task.Delay(TimeSpan.FromMinutes(15));
        }
    }
}