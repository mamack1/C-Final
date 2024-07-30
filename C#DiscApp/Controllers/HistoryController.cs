using Microsoft.AspNetCore.Mvc;
using C_DiscApp.Data;
using C_DiscApp.Models;
using System.Linq;

namespace C_DiscApp.Controllers
{
    public class HistoryController : Controller
    {
        private readonly GameHistoryContext _context;

        public HistoryController(GameHistoryContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var gameHistories = _context.GameHistories.OrderByDescending(g => g.DatePlayed).ToList();
            return View(gameHistories);
        }
    }
}
