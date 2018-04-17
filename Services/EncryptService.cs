using Microsoft.AspNetCore.DataProtection;
using securityservice.Services.Interfaces;

namespace securityservice.Services
{
    public class EncryptService : IEncryptService {
        private readonly IDataProtector _protector;

        public EncryptService (IDataProtectionProvider provider) {
            _protector = provider.CreateProtector (GetType ().FullName);
        }

        public string Decrypt (string cipherString) {
            return _protector.Unprotect (cipherString);
        }

        public string Encrypt (string toEncrypt) {
            return _protector.Protect (toEncrypt);

        }
    }
}