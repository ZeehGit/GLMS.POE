using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Web.Models
{
    public enum ContractStatus
    {
        Draft,
        Active,
        Expired,
        OnHold
    }

    public class Contract
    {
        public int ContractId { get; set; }

        [Required]
        public int ClientId { get; set; }

        [ForeignKey("ClientId")]
        public Client? Client { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        public ContractStatus Status { get; set; } = ContractStatus.Draft;

        [Required]
        [StringLength(50)]
        public string ServiceLevel { get; set; } = string.Empty;

        // PDF file path stored on server
        public string? SignedAgreementPath { get; set; }

        // Navigation property
        public ICollection<ServiceRequest> ServiceRequests { get; set; }
            = new List<ServiceRequest>();
    }
}

/*
* Title: Entity Framework Core
* Author: Microsoft
* Date: 12 November 2024
* Version: 1
* Availability: https://learn.microsoft.com/en-us/ef/core/
*/

/*
* Title: Creating and Configuring a Model
* Author: Microsoft
* Date: 28 March 2023
* Version: 1
* Availability: https://learn.microsoft.com/en-us/ef/core/modeling/
*/