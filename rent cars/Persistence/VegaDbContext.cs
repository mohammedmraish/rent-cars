using Microsoft.EntityFrameworkCore;
using veggga.Core.Models;
using veggga.Models;

namespace veggga.Persistence
{
    public class VegaDbContext : DbContext
    {


        public DbSet<Make> Makes { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<Vehicle> Vehicles{ get; set; }
        public DbSet<VehicleFeature> VehicleFeatures{ get; set; }
        public DbSet<Photo> Photos { get; set; }



        public VegaDbContext(DbContextOptions<VegaDbContext>options) :base(options) 
        { 

        }


        //------------------------------------------------------fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VehicleFeature>().HasKey(vh =>
            new { vh.VehicleId, vh.FeatureId });       
        }

    }
}
