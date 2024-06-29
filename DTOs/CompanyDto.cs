using System.ComponentModel.DataAnnotations;

namespace APBDProject.DTOs;

public class CompanyDto
{
    [Required(ErrorMessage = "CompanyName is required.")]
    [MinLength(1, ErrorMessage = "CompanyName cannot be an empty string.")]
    public required string CompanyName { get; set; }
    [Required(ErrorMessage = "Address is required.")]
    [MinLength(1, ErrorMessage = "Address cannot be an empty string.")]
    public required string Address { get; set; }
    [Required(ErrorMessage = "Email is required.")]
    [MinLength(1, ErrorMessage = "Email cannot be an empty string.")]
    [EmailAddress]
    public required string Email { get; set; }
    [Required(ErrorMessage = "PhoneNum is required.")]
    [MinLength(1, ErrorMessage = "PhoneNum cannot be an empty string.")]
    [Phone]
    public required string PhoneNum { get; set; }
    [Required(ErrorMessage = "KRS is required.")]
    [MinLength(1, ErrorMessage = "KRS cannot be an empty string.")]
    public required string KRS { get; set; }
}