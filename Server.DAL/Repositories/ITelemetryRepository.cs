using Server.DAL.Entities;

namespace Server.DAL.Repositories;

public interface ITelemetryRepository
{
    Task AddTelemetryDataAsync(List<TelemetryEntity> telemetryEntities);

    IQueryable<TelemetryEntity> GetMaxIlluminanceByDeviceForLast30Days(string deviceId);
}
