using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PeopleNetCoreBackend.Data;
using PeopleNetCoreBackend.Models;

namespace PeopleNetCoreBackend.Controllers
{
    /// <summary>
    /// Controller for managing people data
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = "People")]
    public class PeopleController : ControllerBase
    {
        private readonly PeopleDbContext _context;

        public PeopleController(PeopleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all people from the database
        /// </summary>
        /// <returns>A list of all people</returns>
        /// <response code="200">Returns the list of people</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Person>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<Person>>> GetPeople()
        {
            var people = await _context.People.ToListAsync();
            return Ok(people);
        }

        /// <summary>
        /// Retrieves a specific person by CPF
        /// </summary>
        /// <param name="cpf">The CPF of the person to retrieve</param>
        /// <returns>The person with the specified CPF</returns>
        /// <response code="200">Returns the person</response>
        /// <response code="404">If the person is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpGet("{cpf}")]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> GetPerson(string cpf)
        {
            var person = await _context.People.FindAsync(cpf);

            if (person == null)
            {
                return NotFound($"Person with CPF '{cpf}' not found.");
            }

            return Ok(person);
        }

        /// <summary>
        /// Creates a new person
        /// </summary>
        /// <param name="person">The person data to create</param>
        /// <returns>The created person</returns>
        /// <response code="201">Returns the newly created person</response>
        /// <response code="400">If the person data is invalid or CPF already exists</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPost]
        [ProducesResponseType(typeof(Person), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> CreatePerson([FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Check if person with this CPF already exists
            var existingPerson = await _context.People.FindAsync(person.Cpf);
            if (existingPerson != null)
            {
                return BadRequest($"Person with CPF '{person.Cpf}' already exists.");
            }

            _context.People.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPerson), new { cpf = person.Cpf }, person);
        }

        /// <summary>
        /// Updates an existing person
        /// </summary>
        /// <param name="cpf">The CPF of the person to update</param>
        /// <param name="person">The updated person data</param>
        /// <returns>The updated person</returns>
        /// <response code="200">Returns the updated person</response>
        /// <response code="400">If the person data is invalid or CPF mismatch</response>
        /// <response code="404">If the person is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpPut("{cpf}")]
        [ProducesResponseType(typeof(Person), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<Person>> UpdatePerson(string cpf, [FromBody] Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (cpf != person.Cpf)
            {
                return BadRequest("CPF in URL does not match CPF in request body.");
            }

            var existingPerson = await _context.People.FindAsync(cpf);
            if (existingPerson == null)
            {
                return NotFound($"Person with CPF '{cpf}' not found.");
            }

            _context.Entry(existingPerson).CurrentValues.SetValues(person);
            await _context.SaveChangesAsync();

            return Ok(person);
        }

        /// <summary>
        /// Deletes a person by CPF
        /// </summary>
        /// <param name="cpf">The CPF of the person to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">Person successfully deleted</response>
        /// <response code="404">If the person is not found</response>
        /// <response code="500">If there was an internal server error</response>
        [HttpDelete("{cpf}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePerson(string cpf)
        {
            var person = await _context.People.FindAsync(cpf);

            if (person == null)
            {
                return NotFound($"Person with CPF '{cpf}' not found.");
            }

            _context.People.Remove(person);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
