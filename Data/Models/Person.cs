using System.ComponentModel.DataAnnotations;

namespace GiftsAPI.Data.Models;

public class Person
{
    public long Id { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    public ICollection<Gift> Gifts { get; } = [];
}