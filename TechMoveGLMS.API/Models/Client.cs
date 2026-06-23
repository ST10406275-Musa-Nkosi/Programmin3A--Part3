using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.API.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        [Required]
        public string? Name { get; set; }
        [Required]
        public string? ContactDetails { get; set; } // This covers Email/Phone
        [Required]
        public string? Region { get; set; } 

        public virtual ICollection<Contract>? Contracts { get; set; }
    }
}