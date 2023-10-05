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

                [HttpGet("{id}")]
        public async Task<ActionResult<Freelancer>> GetFreelancer(int id)
        {
            var Freelancer = await _dbContext.Freelancers.FirstAsync(id);
            if(Freelancer == null)
            {
                return NotFound();
            }
            return OK(Freelancers);
        }
        [HttpPost]
        public async Task<ActionResult> AddFreelancer(Freelancer freelancer)
        {

            if(freelancer == null)
            {
                return BadRequest();
            }
            _dbContext.Freelancers.Add(freelancer);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFreelancer), new {id= freelancer.FreelancerID}, freelancer);
        }
        [HttpPost]
        public async Task<ActionResult> AddFreelancer(Freelancer freelancer)
        {

            if(freelancer == null)
            {
                return BadRequest();
            }
            _dbContext.Freelancers.Add(freelancer);
            await _dbContext.SaveChangesAsync();
            return CreatedAtAction(nameof(GetFreelancer), new {id= freelancer.FreelancerID}, freelancer);
        }
    }
}