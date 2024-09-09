using System.ComponentModel.DataAnnotations;

namespace Server.BLL.DTOs;

public class IlluminanceDataDto
{
    [Required]
    public double Illum { get; set; }

    [Required]
    public long Time { get; set; }
}
