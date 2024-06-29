using APBDProject.DTOs;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APBDProject.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    }

    [HttpPost("current")]
    [Authorize(Policy = "RequireUserOrAdminRole")]
    public async Task<IActionResult> CalculateCurrentRevenue([FromBody] RevenueDto revenueCalculationDto)
    {
        return await _revenueService.CalculateCurrentRevenueAsync(revenueCalculationDto);
    }

    [HttpPost("predicted")]
    [Authorize(Policy = "RequireUserOrAdminRole")]
    public async Task<IActionResult> CalculatePredictedRevenue([FromBody] RevenueDto revenueCalculationDto)
    {
        return await _revenueService.CalculatePredictedRevenueAsync(revenueCalculationDto);
    }
}