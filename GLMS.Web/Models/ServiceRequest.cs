using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLMS.Web.Models
{
    public enum ServiceRequestStatus 
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }

    public class ServiceRequest
    {
        public int ServiceRequestId { get; set; }

        [Required]
        public int ContractId { get; set; }

        [ForeignKey("ContractId")]
        public Contract? Contract { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        // Cost in USD entered by user
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostUSD { get; set; }

        // Auto-calculated ZAR cost
        [Column(TypeName = "decimal(18,2)")]
        public decimal CostZAR { get; set; }

        // Exchange rate used at time of creation
        [Column(TypeName = "decimal(18,4)")]
        public decimal ExchangeRateUsed { get; set; }

        [Required]
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;
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