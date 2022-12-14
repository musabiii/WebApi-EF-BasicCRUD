using System.ComponentModel.DataAnnotations;

namespace BasicCRUD.Models
{
    public class Student
    {
        public int Id { get; set; }
        [Required]
        public string? StudentName { get; set; }
        public string? Address { get; set; }
        public string? Standard { get; set; }
    }
}
