using System.ComponentModel.DataAnnotations;

namespace APBDProject.Models
{
    public class Client
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ClientType { get; set; } // "Individual" or "Company"

        [MaxLength(50)]
        public string? FirstName { get; set; }

        [MaxLength(50)]
        public string? LastName { get; set; }

        [Required]
        [MaxLength(100)]
        public string Address { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [MaxLength(11)]
        public string? PESEL { get; set; } // Optional for individuals

        [MaxLength(100)]
        public string? CompanyName { get; set; } // Optional for companies

        [MaxLength(10)]
        public string? KRS { get; set; } // Optional for companies
        [Required]
        public bool IsDeleted { get; set; } // Soft delete flag for individuals

        // Navigation property
        public ICollection<Contract> Contracts { get; set; }
    }
}