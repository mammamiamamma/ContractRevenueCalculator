using System.ComponentModel.DataAnnotations;

namespace APBDProject.Models;

public class Discount
{
    [Key]
    public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [Required]
    [MaxLength(20)]
    public string OfferType { get; set; } // "Subscription" or "Upfront"
    [Range(0, 100)]
    public double Value { get; set; }
    [DataType(DataType.Date)]
    public DateTime StartDate { get; set; }
    [DataType(DataType.Date)]
    public DateTime EndDate { get; set; }
}