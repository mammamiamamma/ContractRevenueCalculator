using System.ComponentModel.DataAnnotations;

namespace APBDProject.DTOs;

public class ContractDto
{
    [Required(ErrorMessage = "ClientId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ClientId must be a positive number.")]
    public int ClientId { get; set; }
    
    [Required(ErrorMessage = "SoftwareId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "SoftwareId must be a positive number.")]
    public int SoftwareId { get; set; }
    
    [Required(ErrorMessage = "SoftwareVersion is required.")]
    [MinLength(1, ErrorMessage = "SoftwareVersion cannot be an empty string.")]
    public string SoftwareVersion { get; set; }
    
    [Required(ErrorMessage = "StartDate is required.")]
    public DateTime StartDate { get; set; }
    
    [Required(ErrorMessage = "EndDate is required.")]
    public DateTime EndDate { get; set; }
    
    public int SupportYears { get; set; }
}