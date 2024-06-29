using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using APBDProject.DTOs;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace APBDProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        // private readonly RevenueRecognitionContext _context;
        private readonly IClientService _clientService;

        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }
        
        [HttpPut("updateClient/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> PutClient(int id, [FromBody] JsonElement clientDtoElement)
        {
            var type = await _clientService.GetClientType(id);
            if (type == null)
            {
                return NotFound("No user with such id found");
            }
            try
            {
                if (type == "Individual")
                {
                    var clientDto = JsonSerializer.Deserialize<ClientDto>(clientDtoElement.GetRawText());
                    if (!TryValidateModel(clientDto))
                    {
                        return BadRequest(ModelState);
                    }
                    return await _clientService.UpdateClientAsync(id, clientDto);
                }
                if (type == "Company")
                {
                    var companyDto = JsonSerializer.Deserialize<CompanyDto>(clientDtoElement.GetRawText());
                    if (!TryValidateModel(companyDto))
                    {
                        return BadRequest(ModelState);
                    }
                    return await _clientService.UpdateClientAsync(id, companyDto);
                }
                return BadRequest("Invalid client type.");
            }
            catch (JsonException ex)
            {
                return BadRequest("Invalid input for clientType='"+type+"': " + ex.Message);
            }
        }
        
        [HttpPost("addClient")]
        [Authorize(Policy = "RequireUserOrAdminRole")]
        public async Task<IActionResult> AddClient([FromQuery] string clientType, [FromBody] JsonElement clientDtoElement)
        {
            try
            {
                if (clientType == "Individual")
                {
                    var clientDto = JsonSerializer.Deserialize<ClientDto>(clientDtoElement.GetRawText());
                    if (!TryValidateModel(clientDto))
                    {
                        return BadRequest(ModelState);
                    }
                    return await _clientService.AddClientAsync(clientDto);
                }
                if (clientType == "Company")
                {
                    var companyDto = JsonSerializer.Deserialize<CompanyDto>(clientDtoElement.GetRawText());
                    if (!TryValidateModel(companyDto))
                    {
                        return BadRequest(ModelState);
                    }
                    return await _clientService.AddClientAsync(companyDto);
                }
                return BadRequest("Invalid client type.");
            }
            catch (JsonException ex)
            {
                return BadRequest("Invalid input for clientType='"+clientType+"': " + ex.Message);
            }
        }

        [HttpDelete("deleteClient/{id}")]
        [Authorize(Policy = "RequireAdminRole")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            return await _clientService.RemoveClientAsync(id);
        }
    }
}
