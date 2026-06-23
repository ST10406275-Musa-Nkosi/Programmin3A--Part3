using Microsoft.AspNetCore.Authorization; // Added for JWT protection security
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechMoveGLMS.API.Data;
using TechMoveGLMS.API.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Enforces that all endpoints within this controller require a valid JWT bearer token
    public class ContractsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Constructor handles Dependency Injection for the DB context
        public ContractsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1. GET: api/ContractsApi (With optional filtering by status)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Contract>>> GetContracts([FromQuery] string? status = null)
        {
            var query = _context.Contracts.AsQueryable();

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(c => c.Status == status);
            }

            var contracts = await query.ToListAsync();
            return Ok(contracts); // Returns 200 OK with the JSON payload
        }

        // 2. POST: api/ContractsApi (To create a new contract)
        [HttpPost]
        public async Task<ActionResult<Contract>> PostContract([FromBody] Contract contract)
        {
            if (contract == null)
            {
                return BadRequest("Contract data is invalid."); // Returns 400 BadRequest
            }

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            // Returns 201 Created status with the route location
            return CreatedAtAction(nameof(GetContracts), new { id = contract.ClientId }, contract);
        }

        // 3. PATCH: api/ContractsApi/{id}/status (To approve/decline)
        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateContractStatus(int id, [FromBody] string newStatus)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null)
            {
                return NotFound($"Contract with ID {id} not found."); // Returns 404 NotFound
            }

            contract.Status = newStatus;
            _context.Entry(contract).State = EntityState.Modified;
            _context.SaveChangesAsync();

            return NoContent(); // Returns 204 NoContent (standard for successful PATCH updates)
        }
    }
}