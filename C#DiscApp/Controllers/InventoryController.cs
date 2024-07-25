using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using C_DiscApp.Data;
using C_DiscApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

public class InventoryController : Controller
{
    private readonly DiscContext _context;
    private readonly UserManager<User> _userManager;

    public InventoryController(DiscContext context, UserManager<User> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var userId = _userManager.GetUserId(User);
        var discs = await _context.Discs.Where(d => d.UserId == userId).ToListAsync();
        return View(discs);
    }

    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var disc = await _context.Discs
            .Where(d => d.DiscID == id && d.UserId == userId)
            .FirstOrDefaultAsync();

        if (disc == null)
        {
            return NotFound();
        }

        return View(disc);
    }

    public IActionResult Add()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Add([Bind("DiscID,Name,Type,Weight,Brand,Color,ImageUrl,Speed,Glide,Turn,Fade,Description")] Disc disc)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.GetUserAsync(User);
            disc.UserId = user.Id; // Set the UserId for the current user
            disc.User = user; // Set the User for the current user

            _context.Add(disc);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(disc);
    }

    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var disc = await _context.Discs
            .Where(d => d.DiscID == id && d.UserId == userId)
            .FirstOrDefaultAsync();

        if (disc == null)
        {
            return NotFound();
        }

        return View(disc);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, [Bind("DiscID,Name,Type,Weight,Brand,Color,ImageUrl,Speed,Glide,Turn,Fade,Description")] Disc disc)
    {
        if (id != disc.DiscID)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var existingDisc = await _context.Discs
            .Where(d => d.DiscID == id && d.UserId == userId)
            .FirstOrDefaultAsync();

        if (existingDisc == null)
        {
            return Forbid();
        }

        if (ModelState.IsValid)
        {
            try
            {
                // Update existing disc properties
                existingDisc.Name = disc.Name;
                existingDisc.Type = disc.Type;
                existingDisc.Weight = disc.Weight;
                existingDisc.Brand = disc.Brand;
                existingDisc.Color = disc.Color;
                existingDisc.ImageUrl = disc.ImageUrl;
                existingDisc.Speed = disc.Speed;
                existingDisc.Glide = disc.Glide;
                existingDisc.Turn = disc.Turn;
                existingDisc.Fade = disc.Fade;
                existingDisc.Description = disc.Description;

                _context.Update(existingDisc);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiscExists(disc.DiscID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(disc);
    }

    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var userId = _userManager.GetUserId(User);
        var disc = await _context.Discs
            .Where(d => d.DiscID == id && d.UserId == userId)
            .FirstOrDefaultAsync();

        if (disc == null)
        {
            return NotFound();
        }

        return View(disc);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var disc = await _context.Discs.FindAsync(id);
        if (disc == null || disc.UserId != _userManager.GetUserId(User))
        {
            return Forbid();
        }

        _context.Discs.Remove(disc);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool DiscExists(int id)
    {
        return _context.Discs.Any(e => e.DiscID == id);
    }
}
