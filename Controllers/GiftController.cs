using GiftsAPI.Data;
using GiftsAPI.Data.Models;
using GiftsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftsAPI.Controllers;

[Route("api/gift")]
public class GiftsController(GiftsContext context) : ControllerBase
{

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<GiftDto>>> GetGiftsAsync()
    {
        var gifts = await context.Gifts
            .Include(g => g.Person)
            .OrderBy(g => g.Person.Name)
            .ThenBy(g => g.Name)
            .ThenBy(g => g.Price)
            .Select(g => new GiftDto
            {
                Id = g.Id,
                Person = new PersonDto { Id = g.Person.Id, Name = g.Person.Name },
                Name = g.Name,
                Status = g.Status,
                Price = g.Price,
                URL = g.URL,
                Notes = g.Notes
            })
            .ToListAsync();

        return Ok(gifts);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<GiftDto>> GetGiftAsync(long id)
    {
        var gift = await context.Gifts
            .Include(g => g.Person)
            .Where(g => g.Id == id)
            .Select(g => new GiftDto
            {
                Id = g.Id,
                Person = new PersonDto { Id = g.Person.Id, Name = g.Person.Name },
                Name = g.Name,
                Status = g.Status,
                Price = g.Price,
                URL = g.URL,
                Notes = g.Notes
            })
            .FirstOrDefaultAsync();

        if (gift == null)
            return NotFound();

        return Ok(gift);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateGiftAsync([FromBody] GiftDto giftDto)
    {
        var person = await context.People.FindAsync(giftDto.Person.Id);

        if (person == null)
            return BadRequest("Person not found");

        var gift = new Gift
        {
            Name = giftDto.Name,
            Status = giftDto.Status,
            Price = giftDto.Price,
            URL = giftDto.URL,
            Notes = giftDto.Notes,
            Person = person
        };

        context.Gifts.Add(gift);

        await context.SaveChangesAsync();

        giftDto.Id = gift.Id;

        return CreatedAtRoute(new { id = gift.Id }, giftDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateGiftAsync(long id, [FromBody] GiftDto giftDto)
    {
        var gift = await context.Gifts.FindAsync(id);

        if (gift == null)
            return NotFound();

        gift.Name = giftDto.Name;
        gift.Status = giftDto.Status;
        gift.Price = giftDto.Price;
        gift.URL = giftDto.URL;
        gift.Notes = giftDto.Notes;

        await context.SaveChangesAsync();

        giftDto.Id = gift.Id;

        return Ok(giftDto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeleteGiftAsync(long id)
    {
        var gift = await context.Gifts.FindAsync(id);

        if (gift != null)
        {
            context.Gifts.Remove(gift);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}