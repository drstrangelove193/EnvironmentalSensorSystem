using Server.BLL.DTOs;

namespace Server.BLL.Interfaces;

public interface ITelemetryService
{
    Task AddTelemetryDataAsync(string deviceId, List<IlluminanceDataDto> data);
    Task<List<MaxIlluminanceByDateDto>> GetLastMonthsTelemetryDataAsync(string deviceId);
}
