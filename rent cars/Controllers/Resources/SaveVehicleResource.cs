using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace veggga.Controllers.Resources
{
    public class SaveVehicleResource
    {
        public int Id { get; set; }
        public int ModelId { get; set; }
        public bool IsRegistered { get; set; }
  
        public ContactResource Contact { get; set; }
        public ICollection<int> Features { get; set; }

        public SaveVehicleResource()
        {
            Features = new Collection<int>();
        }

    }

    public class ContactResource 
    {
        [Required]
        [MaxLength(255)]
        public string ContactName { get; set; }

        [MaxLength(255)]
        public string ContactEmail { get; set; }
        [Required]
        [MaxLength(255)]
        public string ContactPhone { get; set; }

    }
}
