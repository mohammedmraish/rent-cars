using veggga.Models;

namespace veggga.Core
{
    public interface IVehicleRepositry
    {
        Task<Vehicle> GetVehicle(int id,bool includeRelated);
        void AddVehicle(Vehicle vehicle);
        void DeleteVehicle(Vehicle vehicle);
        Task<IEnumerable<Vehicle>> GetVehicles();
    }
}