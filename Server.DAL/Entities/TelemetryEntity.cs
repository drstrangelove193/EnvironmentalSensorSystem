namespace Server.DAL.Entities;

public class TelemetryEntity
{
    public int Id { get; set; }
    public Guid DeviceId { get; set; }
    public double Illuminance { get; set; }
    public DateTime Timestamp { get; set; }
}
