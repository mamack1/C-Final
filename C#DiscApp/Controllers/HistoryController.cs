using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Data;
using C_DiscApp.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace C_DiscApp.Controllers
{
    public class HistoryController : Controller
    {
        private readonly DiscContext _context;
        private readonly UserManager<User> _userManager;

        public HistoryController(DiscContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User); // Get the current user ID
            var isAdmin = User.IsInRole("Admin"); // Check if the user is an admin

            var gameHistories = isAdmin
                ? _context.GameHistories.OrderByDescending(g => g.DatePlayed).ToList() // Admin sees all games
                : _context.GameHistories.Where(g => g.UserId == userId).OrderByDescending(g => g.DatePlayed).ToList(); // User sees their own games

            
            
            var viewModel = new GameHistoryViewModel
            {
                GameHistories = gameHistories,
                IsAdmin = isAdmin
            };

            return View(viewModel);
        }
    }
}
