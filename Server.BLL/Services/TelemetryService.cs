using Microsoft.EntityFrameworkCore;
using Server.BLL.DTOs;
using Server.BLL.Exceptions;
using Server.BLL.Interfaces;
using Server.DAL.Entities;
using Server.DAL.Repositories;

namespace Server.BLL.Services;

public class TelemetryService : ITelemetryService
{
    private readonly ITelemetryRepository _repository;

    public TelemetryService(ITelemetryRepository repository)
    {
        _repository = repository;
    }

    public async Task AddTelemetryDataAsync(string deviceId, List<IlluminanceDataDto> data)
    {
        var telemetryEntities = data.Select(dto => new TelemetryEntity
        {
            DeviceId = Guid.Parse(deviceId),
            Illuminance = dto.Illum,
            Timestamp = DateTimeOffset.FromUnixTimeSeconds(dto.Time).UtcDateTime
        }).ToList();

        try
        {
            await _repository.AddTelemetryDataAsync(telemetryEntities);
        }
        catch (Exception ex)
        {
            throw new DbOperationException("Failed to add telemetry data to the database.", ex);
        }
    }

    public async Task<List<MaxIlluminanceByDateDto>> GetLastMonthsTelemetryDataAsync(string deviceId)
    {
        IQueryable<TelemetryEntity> query = _repository.GetMaxIlluminanceByDeviceForLast30Days(deviceId);

        if (!await query.AnyAsync())
        {
            throw new NotFoundException($"No telemetry data found for device with provided ID");
        }

        IQueryable<MaxIlluminanceByDateDto> dtoQuery = query.Select(t => new MaxIlluminanceByDateDto
        {
            Date = t.Timestamp.ToString("yyyy-MM-dd"),
            MaxIlluminance = t.Illuminance
        });

        return await dtoQuery.ToListAsync();
    }
}
