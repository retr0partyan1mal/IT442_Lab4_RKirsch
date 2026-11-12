
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MMABooksEFClasses.MarisModels;

namespace MMABOOKSRESTAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatesController : ControllerBase
    {
        private readonly MMABooksContext _context;

        public StatesController(MMABooksContext context)
        {
            _context = context;
        }

        // GET: api/States
        [HttpGet]
        public async Task<ActionResult<IEnumerable<State>>> GetStates()
        {
            return await _context.State.ToListAsync();
        }

        // GET: api/States/OR
        [HttpGet("{id}")]
        public async Task<ActionResult<State>> GetState(string id)
        {
            var state = await _context.State.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }

            return state;
        }

        // POST: api/States
        [HttpPost]
        public async Task<ActionResult<State>> PostState(State state)
        {
            _context.State.Add(state);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetState),
                new { id = state.StateCode },
                state);
        }

        // PUT: api/States/OR
        [HttpPut("{id}")]
        public async Task<IActionResult> PutState(string id, State state)
        {
            if (id != state.StateCode)
            {
                return BadRequest();
            }

            _context.Entry(state).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.State.AnyAsync(e => e.StateCode == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/States/OR
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteState(string id)
        {
            var state = await _context.State.FindAsync(id);

            if (state == null)
            {
                return NotFound();
            }

            _context.State.Remove(state);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
