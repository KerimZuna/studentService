using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjekatPraksa.Models
{
    public class StudentEntity
    {
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        public string? Mobile { get; set; }
        public string? Email { get; set; }

        public string? ImageUrl { get; set; }

        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}
