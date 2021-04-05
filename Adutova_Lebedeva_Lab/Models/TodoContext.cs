using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Adutova_Lebedeva_Lab.Models
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
           : base(options)
        {
        }

        public DbSet<Mission> Missions { get; set; }
        public DbSet<Employer> Employers { get; set; }
        public DbSet<User> Users { get; set; }
    }
}

