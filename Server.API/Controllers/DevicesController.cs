using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Server.API.ActionFilters;
using Server.BLL.DTOs;
using Server.BLL.Interfaces;

namespace TelemetryServer.Controllers;

[ApiController]
[ApiVersion("1.0")]
[Route("{version:apiVersion}/[controller]")]

public class DevicesController : ControllerBase
{
    private readonly ITelemetryService _telemetryService;

    public DevicesController(ITelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    [HttpPost("{deviceId}/telemetry")]
    [ServiceFilter(typeof(ValidationFilter))]
    [Authorize(Roles = "Server.ReadWrite")]
    public async Task<IActionResult> PostTelemetry(string deviceId, [FromBody] List<IlluminanceDataDto> data)
    {
        await _telemetryService.AddTelemetryDataAsync(deviceId, data);
        return Ok();
    }

    [HttpGet("{deviceId}/statistics")]
    [ServiceFilter(typeof(ValidationFilter))]
    [Authorize(Roles = "Server.ReadWrite")]
    public async Task<IActionResult> GetDeviceStatistics(string deviceId)
    {
        var maxIlluminanceByDates = await _telemetryService.GetLastMonthsTelemetryDataAsync(deviceId);
        return Ok(maxIlluminanceByDates);
    }
}