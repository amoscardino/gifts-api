using System.ComponentModel.DataAnnotations;

namespace GiftsAPI.Models;

public class PersonDto
{
    public long Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;
}
