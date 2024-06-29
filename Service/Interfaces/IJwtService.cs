namespace APBDProject.Service.Interfaces;

public interface IJwtService
{
    string GenerateAccessToken(string username, IList<string> roles); 
    string GenerateRefreshToken();
}