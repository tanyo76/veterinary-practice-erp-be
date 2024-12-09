using System.Linq;
using Microsoft.EntityFrameworkCore;
using UsersRestApi.Entities;

namespace UsersRestApi.Repositories
{
    public class ProjectDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Clinic> Clinics { get; set; }
        public DbSet<EmployeeToClinic> EmployeeToClinic { get; set; }

        public ProjectDBContext()
        {
            this.Users = this.Set<User>();
            this.Clinics = this.Set<Clinic>();
            this.EmployeeToClinic = this.Set<EmployeeToClinic>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
             .UseSqlServer("Server=127.0.0.1,1433;Initial Catalog=vetmasters;User Id=sa;Password=Test@123;Persist Security Info=False;Trusted_Connection=False;Encrypt=False;TrustServerCertificate=True;")
             .UseLazyLoadingProxies();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }
        }

    }
}
