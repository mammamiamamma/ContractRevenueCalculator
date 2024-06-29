using APBDProject.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace APBDProject.Service.Interfaces;

public interface IAuthService
{
    Task<IActionResult> Login(LoginRequest request);
    Task<IActionResult> Refresh(RefreshRequest request);
    Task<IActionResult> Register(RegisterRequest request);
}