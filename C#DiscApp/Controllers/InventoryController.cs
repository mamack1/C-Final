using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using C_DiscApp.Data;
using C_DiscApp.Models;

public class InventoryController : Controller
{
    private readonly DiscContext _context;

    public InventoryController(DiscContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var discs = _context.Discs.ToList();
        return View(discs);
    }

    public IActionResult Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var disc = _context.Discs.FirstOrDefault(d => d.DiscID == id);
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
    public IActionResult Add([Bind("DiscID,Name,Type,Weight,Brand,Color,ImageUrl,Speed,Glide,Turn,Fade,Description,UserID")] Disc disc)
    {
        if (ModelState.IsValid)
        {
            _context.Add(disc);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        return View(disc);
    }

    public IActionResult Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var disc = _context.Discs.Find(id);
        if (disc == null)
        {
            return NotFound();
        }
        return View(disc);
    }

    [HttpPost]
    public IActionResult Edit(int id, [Bind("DiscID,Name,Type,Weight,Brand,Color,ImageUrl,Speed,Glide,Turn,Fade,Description,UserID")] Disc disc)
    {
        if (id != disc.DiscID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(disc);
                _context.SaveChanges();
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

    public IActionResult Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var disc = _context.Discs.FirstOrDefault(d => d.DiscID == id);
        if (disc == null)
        {
            return NotFound();
        }

        return View(disc);
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeleteConfirmed(int id)
    {
        var disc = _context.Discs.Find(id);
        _context.Discs.Remove(disc);
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }

    private bool DiscExists(int id)
    {
        return _context.Discs.Any(e => e.DiscID == id);
    }
}
