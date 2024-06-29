using APBDProject.DTOs;
using APBDProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBDProject.Service.Interfaces;

public interface IContractService
{
    Task<IActionResult> CreateContractAsync(ContractDto contractDto);
    Task<IActionResult> IssuePaymentAsync(PaymentDto paymentDto);
    Task<IActionResult> RemoveContractAsync(int id);
}