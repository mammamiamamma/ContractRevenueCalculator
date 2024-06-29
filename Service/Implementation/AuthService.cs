using APBDProject.Context;
using APBDProject.Controllers;
using APBDProject.Models;
using APBDProject.Service.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace APBDProject.Service.Implementation;

public class AuthService : IAuthService
{
    private readonly RevenueRecognitionContext _context;
    private readonly IJwtService _jwtService;
    private readonly ISecurityService _securityService;

    public AuthService(RevenueRecognitionContext context, IJwtService jwtService, ISecurityService securityService)
    {
        _context = context;
        _jwtService = jwtService;
        _securityService = securityService;
    }

    protected virtual bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] salt = Convert.FromBase64String(storedSalt);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA1,
            iterationCount: 10000,
            numBytesRequested: 256 / 8));
        return hashed == storedHash;
    }

    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users.Where(user => user.Username == request.Username).FirstOrDefaultAsync();
        if (user == null || !VerifyPassword(request.Password, user.PasswordHash, user.Salt))
        {
            return new UnauthorizedObjectResult("Invalid username or password");
        }

        var roles = await _context.UserRoles
            .Where(ur => ur.User_Id == user.Id)
            .Include(userRole => userRole.Role)
            .ToListAsync();
        var accessToken = _jwtService.GenerateAccessToken(user.Username, roles.Select(role => role.Role.Name).ToList());
        var refreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        await _context.SaveChangesAsync();
            
        return new OkObjectResult(new
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        });
    }

    public async Task<IActionResult> Refresh(RefreshRequest request)
    {
        var user = await _context.Users.SingleOrDefaultAsync(u => u.RefreshToken == request.RefreshToken);
        if (user == null)
        {
            return new UnauthorizedObjectResult("Invalid refresh token");
        }

        var roles = await _context.UserRoles
            .Where(ur => ur.User_Id == user.Id)
            .Include(userRole => userRole.Role)
            .ToListAsync();
        var accessToken = _jwtService.GenerateAccessToken(user.Username, roles.Select(
                role => new string(role.Role.Name)).ToList()
        );
        var newRefreshToken = _jwtService.GenerateRefreshToken();

        user.RefreshToken = newRefreshToken;
        await _context.SaveChangesAsync();

        return new OkObjectResult(new
        {
            AccessToken = accessToken,
            RefreshToken = newRefreshToken
        });
    }

    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var existingUser = await _context.Users.Where(user => user.Username == request.Username).FirstOrDefaultAsync();
        if (existingUser != null)
        {
            return new BadRequestObjectResult("User with such username already exists");
        }

        var existingRole = await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == request.Role);
        if (existingRole == null)
        {
            return new BadRequestObjectResult("Role with such name not found");
        }
            
        var hashedPasswordAndSalt = _securityService.GetHashedPasswordAndSalt(request.Password);
           
        var user = new User { 
            Username = request.Username, 
            PasswordHash = hashedPasswordAndSalt.Item1,
            Salt = hashedPasswordAndSalt.Item2,
            RefreshToken = _jwtService.GenerateRefreshToken(),
        };

        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        await _context.UserRoles.AddAsync(new User_Role
        {
            Role_Id = existingRole.Id,
            User_Id = user.Id
        });
        await _context.SaveChangesAsync();
            
        return new OkObjectResult("User registered successfully");
    }
}