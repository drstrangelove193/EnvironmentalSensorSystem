using LightSensor.Factories;
using LightSensor.Interfaces;

namespace LightSensor;

class Program
{
    public static async Task Main()
    {
        ISimulator simulator = SimulatorFactory.CreateSimulator("illuminance");
        await simulator.RunSimulation();
    }
}
