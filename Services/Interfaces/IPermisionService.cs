using System.Collections.Generic;

namespace securityservice.Services.Interfaces
{
    public interface IPermisionService {
        Dictionary<string, string> getPermisions ();
    }
}