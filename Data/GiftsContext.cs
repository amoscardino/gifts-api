using GiftsAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace GiftsAPI.Data;

public class GiftsContext(DbContextOptions<GiftsContext> options) 
    : DbContext(options)
{
    public DbSet<Gift> Gifts { get; set; } = null!;
    public DbSet<Person> People { get; set; } = null!;
}