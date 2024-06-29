namespace APBDProject.Service.Interfaces;

public interface ISecurityService
{
    Tuple<string, string> GetHashedPasswordAndSalt(string password);
}