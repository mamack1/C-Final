using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Models;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace C_DiscApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DiscsController : ControllerBase
    {
        private readonly IDiscService _discService;
        private readonly UserManager<User> _userManager;

        public DiscsController(IDiscService discService, UserManager<User> userManager)
        {
            _discService = discService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Disc>>> GetDiscs()
        {
            var userId = _userManager.GetUserId(User);
            var discs = await _discService.GetAllDiscsAsync(userId);
            return Ok(discs);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Disc>> GetDisc(int id)
        {
            var userId = _userManager.GetUserId(User);
            var disc = await _discService.GetDiscByIdAsync(id, userId);

            if (disc == null)
            {
                return NotFound();
            }

            return disc;
        }

        [HttpPost]
        public async Task<ActionResult<Disc>> PostDisc(Disc disc)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            disc.UserId = _userManager.GetUserId(User);
            await _discService.AddDiscAsync(disc);

            return CreatedAtAction(nameof(GetDisc), new { id = disc.DiscID }, disc);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutDisc(int id, Disc disc)
        {
            if (id != disc.DiscID)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            disc.UserId = _userManager.GetUserId(User);
            await _discService.UpdateDiscAsync(disc);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDisc(int id)
        {
            var userId = _userManager.GetUserId(User);
            var disc = await _discService.GetDiscByIdAsync(id, userId);

            if (disc == null)
            {
                return NotFound();
            }

            await _discService.DeleteDiscAsync(id, userId);

            return NoContent();
        }
    }
}