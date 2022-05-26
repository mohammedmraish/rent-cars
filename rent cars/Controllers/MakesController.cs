using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using veggga.Controllers.Resources;
using veggga.Core.Models;
using veggga.Persistence;

namespace veggga.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MakesController : ControllerBase
    {


        //----------------------------------------
        private readonly VegaDbContext context;
        private readonly IMapper mapper;

        public MakesController(VegaDbContext _context,IMapper mapper)
        {
            context = _context;
            this.mapper = mapper;
        }
        //t----------------------------------------


        [HttpGet("/api/makes")]
        public async Task<IEnumerable<MakeResource>>GetMakes() 
        {
            var makes= await context.Makes.Include(m => m.Models).ToListAsync();

            return mapper.Map<List<Make>, List<MakeResource>>(makes);
        }


        [HttpGet("/api/features")]
        public async Task<IEnumerable<KeyValuePairResource>> GetFeatures()
        {
           var features= await context.Features.ToListAsync();

            return mapper.Map<List<Feature>, List<KeyValuePairResource>>(features);
        
        }
    }
}
