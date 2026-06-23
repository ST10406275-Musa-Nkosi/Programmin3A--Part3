using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechMoveGLMS.API.Data;
using TechMoveGLMS.API.Models;
using TechMoveGLMS.API.Data;   
using TechMoveGLMS.API.Models;

namespace TechMoveGLMS.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceRequestsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServiceRequestsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/ServiceRequestsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ServiceRequest>>> GetServiceRequests()
        {
            return await _context.ServiceRequests.ToListAsync();
        }

        // POST: api/ServiceRequestsApi
        [HttpPost]
        public async Task<ActionResult<ServiceRequest>> PostServiceRequest(ServiceRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            _context.ServiceRequests.Add(request);
            await _context.SaveChangesAsync();
            return Ok(request);
        }
    }
}