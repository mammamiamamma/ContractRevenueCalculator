using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDProject.Models;

public class Contract
{
    [Key]
    public int Id { get; set; }
    public int ClientId { get; set; }
    [ForeignKey(nameof(ClientId))]
    public Client Client { get; set; }
    public int SoftwareId { get; set; }
    [ForeignKey(nameof(SoftwareId))]
    public SoftwareSystem SoftwareSystem { get; set; }
    [Required]
    [MaxLength(50)]
    public string SoftwareVersion { get; set; }
    public int SupportYears { get; set; } 
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public decimal Price { get; set; }
    public decimal PaidAmount { get; set; }
    public bool IsSigned { get; set; }
    public bool IsPaid { get; set; }
    
    public ICollection<Payment> Payments { get; set; }
}