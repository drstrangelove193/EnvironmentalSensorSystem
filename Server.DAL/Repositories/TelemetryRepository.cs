using Microsoft.EntityFrameworkCore;
using Server.DAL.Entities;

namespace Server.DAL.Repositories;


public class TelemetryRepository : ITelemetryRepository
{
    private readonly AppDbContext _context;

    public TelemetryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddTelemetryDataAsync(List<TelemetryEntity> telemetryEntities)
    {
        await _context.Telemetries.AddRangeAsync(telemetryEntities);
        await _context.SaveChangesAsync();
    }

    public IQueryable<TelemetryEntity> GetMaxIlluminanceByDeviceForLast30Days(string deviceId)
    {
        var timeNow = DateTime.UtcNow.Date;
        var thirtyDaysAgo = timeNow.AddDays(-30);
        var id = Guid.Parse(deviceId);

        return _context.Telemetries
            .AsNoTracking()
            .Where(t => t.DeviceId == id && (t.Timestamp >= thirtyDaysAgo && t.Timestamp < timeNow))
            .GroupBy(t => t.Timestamp.Date)
            .Select(g => new TelemetryEntity
            {
                Timestamp = g.Key,
                Illuminance = g.Max(x => x.Illuminance)
            })
            .OrderBy(t => t.Timestamp);
    }
}