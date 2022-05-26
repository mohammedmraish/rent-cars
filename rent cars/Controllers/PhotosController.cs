using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using veggga.Controllers.Resources;
using veggga.Core;
using veggga.Core.Models;
using veggga.Persistence;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace veggga.Controllers
{
    [Route("api/vehicles/{vehiclesId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IHostingEnvironment host;
        private readonly IVehicleRepositry repositry;
        private readonly VegaDbContext context;
        private readonly IMapper mapper;

        public PhotosController(IHostingEnvironment host, IVehicleRepositry repositry,VegaDbContext _context,IMapper mapper)
        {
            this.host = host;
            this.repositry = repositry;
            context = _context;
            this.mapper = mapper;
        }

         
        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetPhoto(int vehiclesId)
        {
            var photos= await context.Photos
                .Where(p => p.VehicleId == vehiclesId)
                .ToListAsync();
            return  mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        }





        [HttpPost]
        public async Task<IActionResult> Upload(int vehiclesId,IFormFile file) 
        {

            var vehicle =await  repositry.GetVehicle(vehiclesId, false);
            if(vehicle==null)
                return NotFound();


            if (file == null) return BadRequest("Null file");
            if (file.Length == 0) return BadRequest("Empty file");
            if (file.Length >10 * 1024 * 1024) return BadRequest("max file 10 maga byte");



                       var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
                Directory.CreateDirectory(uploadsFolderPath);

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(uploadsFolderPath);
            var filePath=Path.Combine(uploadsFolderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            context.SaveChanges();


            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
