using System.ComponentModel.DataAnnotations;
using GiftsAPI.Enums;

namespace GiftsAPI.Models;

public class GiftDto
{
    public long Id { get; set; }
    
    public PersonDto Person { get; set; } = null!;

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public GiftStatus Status { get; set; }

    public decimal? Price { get; set; }

    [MaxLength(2000)]
    public string? URL { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }
}
