using System.ComponentModel.DataAnnotations;

namespace ElectronicEquipamentStore_API.Models.Dto
{
    public class ElectronicEquipamentDto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is Required.")]
        [MaxLength(30, ErrorMessage = "The maximum of characters is 30.")]
        [MinLength(2, ErrorMessage =  "The minimum of characters is 2.")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? EquipamentType { get; set; }

        [Required(ErrorMessage = "Price is Required")]
        public decimal Price { get; set; }
    }
}
