namespace BSM.Framework.PasswordHasher;

public interface IPasswordHasher
{
    string GenerateHash(string password);
    bool VerifyPassword(string password, string passwordHash);
}
