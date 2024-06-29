using APBDProject.Context;
using APBDProject.DTOs;
using APBDProject.Models;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBDProject.Service.Implementation;

public class ClientService : IClientService
{
    private readonly RevenueRecognitionContext _context;

    public ClientService(RevenueRecognitionContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> AddClientAsync(ClientDto clientDto)
    {
        
        var client = new Client
        {
            ClientType = "Individual",
            FirstName = clientDto.FirstName,
            LastName = clientDto.LastName,
            Address = clientDto.Address,
            Email = clientDto.Email,
            PhoneNumber = clientDto.PhoneNum,
            PESEL = clientDto.PESEL,
            IsDeleted = false
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new OkObjectResult("Individual client created successfully");
    }

    public async Task<IActionResult> AddClientAsync(CompanyDto companyDto)
    {
        var client = new Client
        {
            ClientType = "Company",
            CompanyName = companyDto.CompanyName,
            Address = companyDto.Address,
            Email = companyDto.Email,
            PhoneNumber = companyDto.PhoneNum,
            KRS = companyDto.KRS, 
            IsDeleted = false
        };

        _context.Clients.Add(client);
        await _context.SaveChangesAsync();

        return new OkObjectResult("Company client created successfully");
    }

    public async Task<IActionResult> RemoveClientAsync(int id)
    {
        var client = await _context.Clients.SingleOrDefaultAsync(c => c.Id == id);
        if (client == null)
        {
            return new NotFoundObjectResult("No client with such id found");
        }
        if (client.ClientType != "Individual")
        {
            return new BadRequestObjectResult("Cannot delete client with clientType='Company'");
        }
        client.IsDeleted = true;
        await _context.SaveChangesAsync();
        return new OkObjectResult("Client deleted successfully");
    }

    public async Task<IActionResult> UpdateClientAsync(int id, ClientDto clientDto)
    {
        var client = await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();
        if (client == null)
        {
            return new NotFoundObjectResult("No client with such id found");
        }
        if (client.ClientType != "Individual")
        {
            return new BadRequestObjectResult("Client is not individual");
        }

        client.FirstName = clientDto.FirstName;
        client.LastName = clientDto.LastName;
        client.Address = clientDto.Address;
        client.Email = clientDto.Email;
        client.PhoneNumber = clientDto.PhoneNum;

        await _context.SaveChangesAsync();
        return new OkObjectResult("Client updated");
    }
    
    public async Task<IActionResult> UpdateClientAsync(int id, CompanyDto companyDto)
    {
        var client = await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();
        if (client == null)
        {
            return new NotFoundObjectResult("No client with such id found");
        }
        if (client.ClientType != "Company")
        {
            return new BadRequestObjectResult("Client is not company");
        }

        client.CompanyName = companyDto.CompanyName;
        client.Address = companyDto.Address;
        client.Email = companyDto.Email;
        client.PhoneNumber = companyDto.PhoneNum;

        await _context.SaveChangesAsync();
        return new OkObjectResult("Client updated");
    }
    
    public async Task<string?> GetClientType(int id)
    {
        var client = await _context.Clients.Where(c => c.Id == id).FirstOrDefaultAsync();
        if (client == null) return null;
        return client.ClientType;
    }
}