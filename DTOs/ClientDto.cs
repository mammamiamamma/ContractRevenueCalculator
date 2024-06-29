using System.ComponentModel.DataAnnotations;

namespace APBDProject.DTOs;

public class ClientDto
{
    [Required(ErrorMessage = "FirstName is required.")]
    [MinLength(1, ErrorMessage = "FirstName cannot be an empty string.")]
    public required string FirstName { get; set; }
    [Required(ErrorMessage = "LastName is required.")]
    [MinLength(1, ErrorMessage = "LastName cannot be an empty string.")]
    public required string LastName { get; set; }
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
    [Required(ErrorMessage = "PESEL is required.")]
    [MinLength(1, ErrorMessage = "PESEL cannot be an empty string.")]
    public required string PESEL { get; set; }
}