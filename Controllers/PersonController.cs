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
            .Select(p => new PersonDto { Id = p.Id, Name = p.Name })
            .ToListAsync();

        return Ok(people);
    }

    [HttpGet("{id}")]
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
    public async Task<ActionResult> CreatePersonAsync(PersonDto personDto)
    {
        var person = new Person { Name = personDto.Name };

        context.People.Add(person);

        await context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersonAsync), new { id = person.Id }, personDto);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdatePersonAsync(long id, PersonDto personDto)
    {
        var person = await context.People.FindAsync(id);

        if (person == null)
            return NotFound();

        person.Name = personDto.Name;

        await context.SaveChangesAsync();

        return Ok(personDto);
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> DeletePersonAsync(long id)
    {
        var person = await context.People.FindAsync(id);

        if (person != null)
        {
            context.People.Remove(person);
            await context.SaveChangesAsync();
        }

        return NoContent();
    }
}