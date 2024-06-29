using APBDProject.Context;
using APBDProject.DTOs;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBDProject.Service.Implementation;

public class RevenueService : IRevenueService
{
    private readonly RevenueRecognitionContext _context;
    private readonly HttpClient _httpClient;

    public RevenueService(RevenueRecognitionContext context, HttpClient httpClient)
    {
        _context = context;
        _httpClient = httpClient;
    }

    public async Task<IActionResult> CalculateCurrentRevenueAsync(RevenueDto revenueCalculationDto)
    {
        var contracts = _context.Contracts.AsQueryable();

        if (!string.IsNullOrEmpty(revenueCalculationDto.ProductName))
        {
            contracts = contracts.Where(c => c.SoftwareSystem.Name == revenueCalculationDto.ProductName);
        }

        var totalRevenue = await contracts.Where(c => c.IsSigned).SumAsync(c => c.Price);
        
        if (!string.IsNullOrEmpty(revenueCalculationDto.Currency) && revenueCalculationDto.Currency != "PLN")
        {
            var exchangeRate = await GetExchangeRateAsync(revenueCalculationDto.Currency);
            totalRevenue *= exchangeRate;
        }

        return new OkObjectResult(totalRevenue);
    }
    
    public async Task<IActionResult> CalculatePredictedRevenueAsync(RevenueDto revenueCalculationDto)
    {
        var contracts = _context.Contracts.AsQueryable();

        if (!string.IsNullOrEmpty(revenueCalculationDto.ProductName))
        {
            contracts = contracts.Where(c => c.SoftwareSystem.Name == revenueCalculationDto.ProductName);
        }

        var totalRevenue = await contracts.Where(c => !c.IsSigned && c.EndDate>DateTime.Today).SumAsync(c => c.Price);

        if (!string.IsNullOrEmpty(revenueCalculationDto.Currency) && revenueCalculationDto.Currency != "PLN")
        {
            var exchangeRate = await GetExchangeRateAsync(revenueCalculationDto.Currency);
            totalRevenue *= exchangeRate;
        }

        return new OkObjectResult(totalRevenue);
    }
    private async Task<decimal> GetExchangeRateAsync(string toCurrency)
    {
        var url = $"https://api.freecurrencyapi.com/v1/latest?apikey=fca_live_huKK91cxycJNu20enVyTXrxtgM4r9giDi9HzR1gu&currencies={toCurrency}&base_currency=PLN";
        var response = await _httpClient.GetFromJsonAsync<ExchangeRateResponse>(url);
    
        if (response?.Data != null && response.Data.ContainsKey(toCurrency))
        {
            return response.Data[toCurrency];
        }
    
        return 1; // Default to 1 if the exchange rate is not found
    }
}

public class ExchangeRateResponse
{
    public Dictionary<string, decimal> Data { get; set; }
}