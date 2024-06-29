using APBDProject.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace APBDProject.Service.Interfaces;

public interface IRevenueService
{
    Task<IActionResult> CalculateCurrentRevenueAsync(RevenueDto revenueCalculationDto);
    Task<IActionResult> CalculatePredictedRevenueAsync(RevenueDto revenueCalculationDto);
}