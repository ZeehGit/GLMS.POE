using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace GLMS.Web.Models
{
    public class Client
    {
        public int ClientId { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [StringLength(200)]
        public string ContactDetails { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Region { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
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