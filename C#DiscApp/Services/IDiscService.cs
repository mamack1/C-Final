using System.Collections.Generic;
using System.Threading.Tasks;
using C_DiscApp.Models;

namespace C_DiscApp.Services
{
    public interface IDiscService
    {
        Task<IEnumerable<Disc>> GetAllDiscsAsync(string userId);
        Task<Disc> GetDiscByIdAsync(int id, string userId);
        Task AddDiscAsync(Disc disc);
        Task UpdateDiscAsync(Disc disc);
        Task DeleteDiscAsync(int id, string userId);
    }
}