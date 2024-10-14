using GiftsAPI.Data;
using GiftsAPI.Data.Models;
using GiftsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GiftsAPI.Controllers;

[Route("api/person")]
public class PersonController(GiftsContext context) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PersonDto>>> GetPeopleAsync()
    {
        var people = await context.People
            .OrderBy(p => p.Name)
            .ThenBy(p => p.Id)
            .Select(p => new PersonDto { Id = p.Id, Name = p.Name })
            .ToListAsync();

        return Ok(people);
    }

    [HttpGet("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonDto>> GetPersonAsync(long id)
    {
        var person = await context.People
            .Where(p => p.Id == id)
            .Select(p => new PersonDto { Id = p.Id, Name = p.Name })
            .FirstOrDefaultAsync();

        if (person == null)
            return NotFound();

        return Ok(person);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult> CreatePersonAsync([FromBody] PersonDto personDto)
    {
        var person = new Person { Name = personDto.Name };

        context.People.Add(person);

        await context.SaveChangesAsync();

        personDto.Id = person.Id;

        return CreatedAtRoute(new { id = person.Id }, personDto);
    }

    [HttpPut("{id:long}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePersonAsync(long id, [FromBody] PersonDto personDto)
    {
        var person = await context.People.FindAsync(id);

        if (person == null)
            return NotFound();

        person.Name = personDto.Name;

        await context.SaveChangesAsync();

        personDto.Id = person.Id;

        return Ok(personDto);
    }

    [HttpDelete("{id:long}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeletePersonAsync(long id)
    {
        var person = await context.People.FindAsync(id);

        if (person != null)
        {
            foreach (var gift in person.Gifts)
                context.Gifts.Remove(gift);

            context.People.Remove(person);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}