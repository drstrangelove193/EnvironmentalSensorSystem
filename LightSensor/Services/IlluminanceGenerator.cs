using LightSensor.Interfaces;
using LightSensor.Models;

namespace LightSensor.Services;

public class IlluminanceGenerator : IIlluminanceGenerator
{
    private const double MinIlluminance = 120.0;
    private const double MaxIlluminance = 160.0;
    private const int RiseStartHour = 6;
    private const int PeakHour = 14;
    private const int FallEndHour = 24;
    private Random random = new Random();
    private double currentIlluminance = 0;
    private bool hasBeenInitialized = false;

    public List<IlluminanceData> GenerateIlluminanceForHour(DateTime startTime)
    {
        List<IlluminanceData> illuminanceDataList = new List<IlluminanceData>();
        double peakIlluminance = random.NextDouble() * (MaxIlluminance - MinIlluminance) + MinIlluminance;
        double riseRate = peakIlluminance / ((PeakHour - RiseStartHour) * 4);
        double fallRate = peakIlluminance / ((FallEndHour - PeakHour) * 4);

        for (int i = 0; i < 4; i++)
        {
            DateTime currentTime = startTime.AddMinutes(15 * i);
            double illuminance = CalculateAndUpdateIlluminance(currentTime, riseRate, fallRate, peakIlluminance);

            illuminanceDataList.Add(new IlluminanceData
            {
                Illum = illuminance,
                Time = ((DateTimeOffset)currentTime).ToUnixTimeSeconds()
            });
        }

        return illuminanceDataList;
    }

    private double CalculateAndUpdateIlluminance(DateTime currentTime, double riseRate, double fallRate, double peakIlluminance)
    {
        int currentHour = currentTime.Hour;
        if (!hasBeenInitialized)
        {
            // Initial calculation based on the current hour
            if (currentHour >= RiseStartHour && currentHour < PeakHour)
            {
                int intervalsSinceStart = (currentHour - RiseStartHour) * 4 + currentTime.Minute / 15;
                currentIlluminance = intervalsSinceStart * riseRate;
            }
            else if (currentHour >= PeakHour && currentHour < FallEndHour)
            {
                int intervalsSincePeak = (currentHour - PeakHour) * 4 + currentTime.Minute / 15;
                currentIlluminance = peakIlluminance - intervalsSincePeak * fallRate;
            }
            else
            {
                currentIlluminance = 0;
            }

            hasBeenInitialized = true;
        }
        else
        {
            // Update based on previous value
            if (currentHour >= RiseStartHour && currentHour < PeakHour)
            {
                currentIlluminance += riseRate;
            }
            else if (currentHour >= PeakHour && currentHour < FallEndHour)
            {
                currentIlluminance -= fallRate;
            }
            else
            {
                currentIlluminance = 0;  // Reset at night
            }
        }

        // Ensure illuminance does not drop below zero or exceed peak
        currentIlluminance = Math.Max(0, Math.Min(peakIlluminance, currentIlluminance));
        return Math.Round(currentIlluminance * 2) / 2.0;
    }
}
