using System.ComponentModel.DataAnnotations;
using GiftsAPI.Enums;

namespace GiftsAPI.Data.Models;

public class Gift
{
    public long Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public GiftStatus Status { get; set; } = GiftStatus.Idea;

    public decimal? Price { get; set; }

    [MaxLength(2000)]
    public string? URL { get; set; }

    [MaxLength(4000)]
    public string? Notes { get; set; }

    public Person Person { get; set; } = null!;
}