using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using securityservice.Services.Interfaces;
using System.Linq;

namespace securityservice.Services
{
 public class PermisionService : IPermisionService {
        private readonly IConfiguration _configuration;

        public PermisionService (IConfiguration configuration) {
            _configuration = configuration;
        }

        public Dictionary<string, string> getPermisions () {

            Dictionary<string, string> permissions = new Dictionary<string, string> ();
            if (_configuration.GetSection ("PossiblePermissions") != null)
                permissions = _configuration.GetSection ("PossiblePermissions").GetChildren ()
                .Select (item => new KeyValuePair<string, string> (item.Key, item.Value))
                .ToDictionary (x => x.Key, x => x.Value);
            return permissions;
        }
    }
}