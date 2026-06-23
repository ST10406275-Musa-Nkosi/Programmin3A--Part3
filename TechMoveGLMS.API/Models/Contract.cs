using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TechMoveGLMS.API.Models
{
    public class Contract
    {
        [Key]
        public int ContractId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }

        [Required] // Draft, Active, Expired
        public string Status { get; set; } = "Draft";

        [Required] // Gold, Silver, Bronze
        public string? ServiceLevel { get; set; }

        // This is for the "File Handling Mechanism" in your rubric
        public string? SignedAgreementPath { get; set; }

        public int ClientId { get; set; }
        public virtual Client? Client { get; set; }

        public virtual ICollection<ServiceRequest>? ServiceRequests { get; set; }
    }
}