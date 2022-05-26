using System.ComponentModel.DataAnnotations;

namespace veggga.Core.Models
{
    public class Feature
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Name { get; set; }
    }
}
