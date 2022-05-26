using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace School.Models
{
    public partial class ApiContext : DbContext
    {
        public ApiContext() { }

        public ApiContext(DbContextOptions<ApiContext> options) : base(options)
        {
        }

        public DbSet<FileData> Files { get; set; }
        //public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FileData>(entity =>
            {
                entity.Property(x => x.CreatedOn).HasColumnType("datetime");
            });
            //modelBuilder.Entity<User>().Property(u => u.Email).IsRequired();
            modelBuilder.Entity<Role>().Property(u => u.Name).IsRequired();

            onModelCreatingPartial(modelBuilder);
        }
        partial void onModelCreatingPartial(ModelBuilder modelBuilder);

    }
}
