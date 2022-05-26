using Microsoft.EntityFrameworkCore;
using veggga.Core;
using veggga.Models;

namespace veggga.Persistence
{
    public class VehicleRepositry : IVehicleRepositry
    {
        private readonly VegaDbContext context;

        public VehicleRepositry(VegaDbContext _context)
        {
            context = _context;
        }
        public async Task<Vehicle> GetVehicle(int id,bool includeRelated)
        {
            if (includeRelated == false)
            {
                return await context.Vehicles.SingleOrDefaultAsync(v => v.Id == id);
            }
            else 
            { 
                return await context.Vehicles
                     .Include(v => v.Features)
                       .ThenInclude(vf => vf.Feature)
                     .Include(v => v.Model)
                       .ThenInclude(vm => vm.Make)
                     .SingleOrDefaultAsync(v => v.Id == id);
            }


        }
        public void AddVehicle(Vehicle vehicle)
        {
            context.Vehicles.Add(vehicle);
             context.SaveChanges();


        }

        public void DeleteVehicle(Vehicle vehicle)
        {
            context.Vehicles.Remove(vehicle);
             context.SaveChanges();

        }

        public async Task<IEnumerable<Vehicle>> GetVehicles()
        {
            return await context.Vehicles
                     .Include(v => v.Features)
                       .ThenInclude(vf => vf.Feature)
                     .Include(v => v.Model)
                       .ThenInclude(vm => vm.Make).ToListAsync();
        }
    }
}
