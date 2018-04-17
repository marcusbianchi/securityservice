namespace securityservice.Services.Interfaces
{
    public interface IEncryptService {
        string Encrypt (string toEncrypt);
        string Decrypt (string cipherString);
    }
}