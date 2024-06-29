using System.ComponentModel.DataAnnotations;

namespace APBDProject.Models;

public class SoftwareSystem
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [MaxLength(50)]
    public string InfoAboutCurrVersion { get; set; }

    [MaxLength(50)]
    public string Category { get; set; }
    
    public decimal Price { get; set; }
    
    public bool IsSubscriptionAvailable { get; set; }
    public bool IsUpfrontCostAvailable { get; set; }
    
    public ICollection<Contract> Contracts { get; set; }
}