using C_DiscApp.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Net.Sockets;

namespace C_DiscApp.Data
{
    public class DiscContext : DbContext
    {
        public DiscContext(DbContextOptions<DiscContext> options)
        : base(options)
        {
        }

        public DbSet<Disc> Discs { get; set; }

    }
}
