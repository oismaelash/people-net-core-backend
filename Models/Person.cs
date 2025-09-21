using System.ComponentModel.DataAnnotations;

namespace PeopleNetCoreBackend.Models
{
    public class Person
    {
        [Key]
        public string Cpf { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        public string Genre { get; set; } = string.Empty;
        
        [Required]
        public string Address { get; set; } = string.Empty;
        
        [Required]
        public int Age { get; set; }
        
        [Required]
        public string Neighborhood { get; set; } = string.Empty;
        
        [Required]
        public string State { get; set; } = string.Empty;
    }
}
