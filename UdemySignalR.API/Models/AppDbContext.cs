using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UdemySignalR.API.Models
{
    public class AppDbContext: DbContext
    {
        //overridedan farklı bir yöntem ve connstring appsettings.json'a yazıldı ve startup'a da eklemek zorunda kaldık. önceki daha iyi
        public AppDbContext(DbContextOptions<AppDbContext> options) :base(options)
        {

        }
        public DbSet<Team> Teams { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
