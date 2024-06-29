using APBDProject.Context;
using APBDProject.DTOs;
using APBDProject.Models;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace APBDProject.Service.Implementation;

public class ContractService : IContractService
    {
        private readonly RevenueRecognitionContext _context;

        public ContractService(RevenueRecognitionContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> CreateContractAsync(ContractDto contractDto)
        {
            if ((contractDto.EndDate - contractDto.StartDate).TotalDays < 3 || (contractDto.EndDate - contractDto.StartDate).TotalDays > 30)
            {
                return new BadRequestObjectResult("The time range of the contract should be from 3 to 30 days");
            }
            var client = await _context.Clients.FindAsync(contractDto.ClientId);
            if (client == null) return new NotFoundObjectResult("Client with such id not found");

            var software = await _context.SoftwareSystems.FindAsync(contractDto.SoftwareId);
            if (software == null) return new NotFoundObjectResult("Software with such id not found");

            if (contractDto.SupportYears < 0 || contractDto.SupportYears > 3)
            {
                return new BadRequestObjectResult("Support years must be between 0 and 3");
            }
            
            if (contractDto.SoftwareVersion.IsNullOrEmpty())
            {
                return new BadRequestObjectResult("No software version provided");
            }
            
            if (await _context.Contracts.AnyAsync(c => 
                    c.ClientId == contractDto.ClientId 
                    && c.SoftwareId == contractDto.SoftwareId 
                    && (!c.IsPaid || !c.IsSigned)))
                return new BadRequestObjectResult("Client already has an active contract for this product");
            
            var highestDiscount = await GetHighestDiscountAsync(contractDto.ClientId);
            var priceAfterDiscount = software.Price - (software.Price * (decimal)highestDiscount / 100);
            priceAfterDiscount += contractDto.SupportYears * 1000;
            
            var contract = new Contract
            {
                ClientId = contractDto.ClientId,
                SoftwareId = contractDto.SoftwareId,
                SoftwareVersion = contractDto.SoftwareVersion,
                StartDate = contractDto.StartDate,
                EndDate = contractDto.EndDate,
                Price = priceAfterDiscount,
                PaidAmount = 0,
                IsPaid = false,
                IsSigned = false,
                SupportYears = contractDto.SupportYears
            };

            _context.Contracts.Add(contract);
            await _context.SaveChangesAsync();

            return new OkObjectResult("Contract created successfully");
        }
        
        private async Task RemovePayments(Contract contract)
        {
            var payments = await _context.Payments.Where(p => p.ContractId == contract.Id).ToListAsync();
            foreach (var payment in payments)
            {
                _context.Payments.Remove(payment);
            }
            _context.Contracts.Remove(contract);
        }
        public async Task<IActionResult> RemoveContractAsync(int id)
        {
            var contract = await _context.Contracts.FindAsync(id);
            if (contract == null) return new NotFoundObjectResult("Contract not found");

            await RemovePayments(contract);
            _context.Contracts.Remove(contract);
            
            await _context.SaveChangesAsync();
            return new OkObjectResult("Contract removed successfully");
        }

        public async Task<IActionResult> IssuePaymentAsync(PaymentDto paymentDto)
        {
            var contract = await _context.Contracts.FindAsync(paymentDto.ContractId);
            if (contract == null) return new NotFoundObjectResult("Contract not found");
            if (contract.IsPaid) return new ConflictObjectResult("The contract is already paid for");
            if (DateTime.Now > contract.EndDate)
            {
                return new BadRequestObjectResult("Cannot accept payment after contract end date");
            }
            
            if (contract.IsPaid)
            {
                return new BadRequestObjectResult("Contract is already fully paid");
            }
            
            var newPaidAmount = contract.PaidAmount + paymentDto.Amount;
            if (newPaidAmount > contract.Price)
            {
                return new BadRequestObjectResult("Payment amount exceeds contract price");
            }
            
            var payment = new Payment
            {
                ContractId = paymentDto.ContractId,
                Amount = paymentDto.Amount,
                PaymentDate = DateTime.Now
            };

            contract.PaidAmount = newPaidAmount;
            if (contract.PaidAmount == contract.Price)
            {
                contract.IsPaid = true;
                contract.IsSigned = true;
            }

            _context.Payments.Add(payment);
            await _context.SaveChangesAsync();
            
            return new OkObjectResult("Payment successful");
        }

        private async Task<double> GetHighestDiscountAsync(int clientId)
        {
            var previousClientDiscount = 0.0;
            if (await _context.Contracts.AnyAsync(c => c.ClientId == clientId && c.IsPaid))
            {
                previousClientDiscount = 5.0; // 5% discount for returning clients
            }

            var activeDiscounts = await _context.Discounts
                .Where(d => d.OfferType == "Upfront" && DateTime.Now >= d.StartDate && DateTime.Now <= d.EndDate)
                .ToListAsync();

            var highestDiscount = activeDiscounts.Any() ? activeDiscounts.Max(d => d.Value) : 0;
            return Math.Max(previousClientDiscount, highestDiscount);
        }
    }