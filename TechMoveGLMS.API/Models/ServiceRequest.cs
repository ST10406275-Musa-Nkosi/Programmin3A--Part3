using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.API.Models
{
    public class ServiceRequest
    {
        [Key]
        public int ServiceRequestId { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Cost { get; set; } 

        public string Status { get; set; } = "Pending";

        public int ContractId { get; set; }
        public virtual Contract? Contract { get; set; }
    }
}