using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using APBDProject.Context;
using APBDProject.DTOs;
using APBDProject.Models;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace APBDProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractsController : ControllerBase
    {
        private readonly IContractService _contractService;

        public ContractsController(IContractService contractService)
        {
            _contractService = contractService;
        }

        [HttpPost("pay")]
        [Authorize(Policy = "RequireUserOrAdminRole")]
        public async Task<IActionResult> IssuePayment([FromBody] PaymentDto paymentDto)
        {
            var result = await _contractService.IssuePaymentAsync(paymentDto);
            return result;
        }
        
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Policy = "RequireUserOrAdminRole")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateContract([FromBody] ContractDto contractDto)
        {
            var result = await _contractService.CreateContractAsync(contractDto);
            return result;
        }

        [Authorize(Policy = "RequireUserOrAdminRole")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> RemoveContract(int id)
        {
            var result = await _contractService.RemoveContractAsync(id);
            return result;
        }
    }
}
