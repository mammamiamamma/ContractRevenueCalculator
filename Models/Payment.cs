using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace APBDProject.Models;

public class Payment
{
    [Key]
    public int Id { get; set; }

    public int ContractId { get; set; }
    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; }

    public decimal Amount { get; set; }
    public DateTime PaymentDate { get; set; }
}