using LightSensor.Models;

namespace LightSensor.Interfaces;

internal interface IDataSender
{
    Task SendDataAsync(List<IlluminanceData> data);
}

