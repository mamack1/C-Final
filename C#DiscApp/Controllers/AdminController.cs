using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Services;
using Microsoft.AspNetCore.Identity;
using C_DiscApp.Models;
using System.Threading.Tasks;
using System.Linq;

namespace C_DiscApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IDiscService _discService;

        public AdminController(UserManager<User> userManager, IDiscService discService)
        {
            _userManager = userManager;
            _discService = discService;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        public async Task<IActionResult> UserDetails(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditUser(User user)
        {
            var existingUser = await _userManager.FindByIdAsync(user.Id);
            if (existingUser == null)
            {
                return NotFound();
            }

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.UserName = user.Email;

            var result = await _userManager.UpdateAsync(existingUser);
            if (result.Succeeded)
            {
                return RedirectToAction("Users");
            }

            return View(user);
        }

        public async Task<IActionResult> DeleteUser(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteUserConfirmed(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }

            ModelState.AddModelError("", "Failed to delete the user.");
            return View("DeleteUser", user); // Return to DeleteUser view in case of failure
        }

        public async Task<IActionResult> UserInventory(string id)
        {
            var discs = await _discService.GetAllDiscsAsync(id);
            ViewBag.UserId = id;
            return View(discs);
        }

        public IActionResult CreateDisc(string userId)
        {
            var disc = new Disc { UserId = userId };
            return View(disc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDisc(Disc disc)
        {
            if (ModelState.IsValid)
            {
                disc.UserId = ViewBag.UserId;  // Ensure userId is set from ViewBag
                await _discService.AddDiscAsync(disc);
                return RedirectToAction("UserInventory", new { id = disc.UserId });
            }
            return View(disc);
        }

        public async Task<IActionResult> EditDisc(int id, string userId)
        {
            var disc = await _discService.GetDiscByIdAsync(id, userId);
            if (disc == null)
            {
                return NotFound();
            }
            ViewBag.UserId = userId;
            return View(disc);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDisc(Disc disc)
        {
            if (ModelState.IsValid)
            {
                await _discService.UpdateDiscAsync(disc);
                return RedirectToAction("UserInventory", new { id = disc.UserId });
            }
            return View(disc);
        }

        public async Task<IActionResult> DeleteDisc(int id, string userId)
        {
            var disc = await _discService.GetDiscByIdAsync(id, userId);
            if (disc == null)
            {
                return NotFound();
            }
            ViewBag.UserId = userId;
            return View(disc);
        }

        [HttpPost, ActionName("DeleteDisc")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDiscConfirmed(int id, string userId)
        {
            await _discService.DeleteDiscAsync(id, userId);
            return RedirectToAction("UserInventory", new { id = userId });
        }
    }
}