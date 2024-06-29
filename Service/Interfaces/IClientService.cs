using APBDProject.DTOs;
using APBDProject.Models;
using Microsoft.AspNetCore.Mvc;

namespace APBDProject.Service.Interfaces;

public interface IClientService
{
    Task<IActionResult> AddClientAsync(ClientDto clientDto);
    Task<IActionResult> AddClientAsync(CompanyDto companyDto);
    Task<IActionResult> RemoveClientAsync(int id);
    Task<IActionResult> UpdateClientAsync(int id, ClientDto clientDto);
    Task<IActionResult> UpdateClientAsync(int id, CompanyDto clientDto);
    Task<string?> GetClientType(int id);
}