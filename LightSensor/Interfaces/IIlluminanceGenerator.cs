using LightSensor.Models;

namespace LightSensor.Interfaces;

public interface IIlluminanceGenerator
{
    List<IlluminanceData> GenerateIlluminanceForHour(DateTime startTime);
}
