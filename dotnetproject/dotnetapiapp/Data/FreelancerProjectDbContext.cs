using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace dotnetapiapp.Data
{
    public class FreelancerProjectDbContext : DbContext
    {
        public FreelancerProjectDbContext(DbContextOptions<FreelancerProjectDbContext> options)
        : base(options)
        {
        }
    public DbSet<Freelancer> Freelancers {get; set;}
        public DbSet<Project> Freelancers {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<FreelancerProjectDbContext>()
            .HasOne(p => p.Freelancer)
            .WithMany(f => f.Projects)
            .HasForeignKey( p => p.FreelancerID)
            .OnDelete(DeleteBehaviour.Restrict);
        }

}
}