using C_DiscApp.Models;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace C_DiscApp.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IDiscService _discService;
        private readonly UserManager<User> _userManager;

        public InventoryController(IDiscService discService, UserManager<User> userManager)
        {
            _discService = discService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var discs = await _discService.GetAllDiscsAsync(userId);
            return View(discs);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Disc disc)
        {
            if (ModelState.IsValid)
            {
                disc.UserId = _userManager.GetUserId(User);
                await _discService.AddDiscAsync(disc);
                return RedirectToAction(nameof(Index));
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
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userId = _userManager.GetUserId(User);
            await _discService.DeleteDiscAsync(id, userId);
            return RedirectToAction(nameof(Index));
        }
    }
}