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
    }
}