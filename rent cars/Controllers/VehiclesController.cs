using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using veggga.Controllers.Resources;
using veggga.Models;
using AutoMapper;
using veggga.Persistence;
using Microsoft.EntityFrameworkCore;
using veggga.Core;

namespace veggga.Controllers
{
  
    [ApiController]
    public class VehiclesController : ControllerBase
    {
        ////////////////////////////////////////
        private readonly IMapper mapper;
        private readonly VegaDbContext context;
        private readonly IVehicleRepositry vehicleRepositry;

        public VehiclesController(IMapper mapper, VegaDbContext _context, IVehicleRepositry vehicleRepositry)
        {
            this.mapper = mapper;
            context = _context;
            this.vehicleRepositry = vehicleRepositry;
        }
        ////////////////////////////////////////


        [HttpPost("api/vehicles")]
        public async Task<IActionResult> CreateVehicles([FromBody] SaveVehicleResource vehicleResource)
        {

            if (!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            //chek if model id is exist
            var model = await context.Models.FindAsync(vehicleResource.ModelId);
            if (model == null) {
                ModelState.AddModelError("ModelId", "invalid modelId.");
                return BadRequest(ModelState);
            }


            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            vehicleRepositry.AddVehicle(vehicle);


            //return response
            vehicle = await vehicleRepositry.GetVehicle(vehicle.Id, true);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }



        [HttpPut("api/vehicles/{id}")]
        public async Task<IActionResult> UpdateVehicles(int id, [FromBody] SaveVehicleResource vehicleResource)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }



            var vehicle = await vehicleRepositry.GetVehicle(id, true);
            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);

            vehicle.LastUpdate = DateTime.Now;

            await context.SaveChangesAsync();

            //return response
            vehicle = await vehicleRepositry.GetVehicle(vehicle.Id, true);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }

        [HttpDelete("api/vehicles/{id}")]
        public async Task<IActionResult> UpdateVehicles(int id)
        {

            var vehicle = await vehicleRepositry.GetVehicle(id, false);

            if (vehicle == null)
                return NotFound();

            vehicleRepositry.DeleteVehicle(vehicle);

            return Ok();
        }

        [HttpGet("api/vehicles/{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            //return response
            var vehicle = await vehicleRepositry.GetVehicle(id, true);
            if (vehicle == null)
                return BadRequest();

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(result);
        }
        [HttpGet("api/vehicles")]
        public async Task<IEnumerable<VehicleResource>> GetVehicles() 
        {
            var vehicles =await vehicleRepositry.GetVehicles();
            return mapper.Map<IEnumerable<Vehicle>, IEnumerable<VehicleResource>>(vehicles);
        }

}
}
