using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreelancerNamespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class FreelancerController:ControllerBase
    {
        private readonly FreelancerProjectDbContext _dbContext;
        public FreelancerController(FreelancerProjectDbContext dbContext ){
            _dbContext = dbContext;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Freelancer>>> GetAllFreelancers()
        {
            var Freelancers = await _dbContext.Freelancers.ToListAsync();
            if(Freelancers == null || !Freelancers.Any())
            {
                return NotFound();
            }
            return OK(Freelancers);
        }
    }
}