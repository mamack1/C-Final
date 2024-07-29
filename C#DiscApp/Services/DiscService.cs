using C_DiscApp.Data;
using C_DiscApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace C_DiscApp.Services
{
    public class DiscService : IDiscService
    {
        private readonly DiscContext _context;

        public DiscService(DiscContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Disc>> GetAllDiscsAsync(string userId)
        {
            return await _context.Discs.Where(d => d.UserId == userId).ToListAsync();
        }

        public async Task<Disc> GetDiscByIdAsync(int id, string userId)
        {
            return await _context.Discs.FirstOrDefaultAsync(d => d.DiscID == id && d.UserId == userId);
        }

        public async Task AddDiscAsync(Disc disc)
        {
            await _context.Discs.AddAsync(disc);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateDiscAsync(Disc disc)
        {
            _context.Discs.Update(disc);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteDiscAsync(int id, string userId)
        {
            var disc = await _context.Discs.FirstOrDefaultAsync(d => d.DiscID == id && d.UserId == userId);
            if (disc != null)
            {
                _context.Discs.Remove(disc);
                await _context.SaveChangesAsync();
            }
        }
    }
}