using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using C_DiscApp.Models;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace C_DiscApp.Controllers
{
    [Authorize]
    public class InventoryController : Controller
    {
        private readonly IDiscService _discService;
        private readonly ILogger<InventoryController> _logger;
        private readonly UserManager<User> _userManager;

        public InventoryController(IDiscService discService, ILogger<InventoryController> logger, UserManager<User> userManager)
        {
            _discService = discService;
            _logger = logger;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); // Get the current user ID
            var discs = await _discService.GetAllDiscsAsync(userId);
            return View(discs);
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Disc disc)
        {
            if (ModelState.IsValid)
            {
                disc.UserId = _userManager.GetUserId(User); // Set the user ID from the current user
                await _discService.AddDiscAsync(disc);
                return RedirectToAction(nameof(Index));
            }
            return View(disc);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userId = _userManager.GetUserId(User);
            var disc = await _discService.GetDiscByIdAsync(id, userId);
            if (disc == null)
            {
                return NotFound();
            }
            return View(disc);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var userId = _userManager.GetUserId(User);
            var disc = await _discService.GetDiscByIdAsync(id, userId);
            if (disc == null)
            {
                return NotFound();
            }
            return View(disc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Disc disc)
        {
            if (id != disc.DiscID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                disc.UserId = _userManager.GetUserId(User);
                await _discService.UpdateDiscAsync(disc);
                return RedirectToAction(nameof(Index));
            }
            return View(disc);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var userId = _userManager.GetUserId(User);
            var disc = await _discService.GetDiscByIdAsync(id, userId);
            if (disc == null)
            {
                return NotFound();
            }
            return View(disc);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _discService.DeleteDiscAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}