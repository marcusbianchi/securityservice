using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using securityservice.Model;
using securityservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using securityfilter.Services.Interfaces;

namespace securityservice.Controllers {
    [Route ("api/login")]

    public class LoginController : Controller {
        private readonly IUserService _userService;
        private readonly IEncryptService _encryptService;
        public LoginController (IUserService userService, IEncryptService encryptService) {
            _userService = userService;
            _encryptService = encryptService;
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] User user) {

            if (String.IsNullOrEmpty (user.password) && String.IsNullOrEmpty (user.username)) {
                return BadRequest ();
            }
            user = await _userService.getUserByName (user.username, user.password);
            if (user == null)
                return NotFound ();
            var jsonUser = JsonConvert.SerializeObject (user, Formatting.None,
                new JsonSerializerSettings {
                    ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                });

            var encriptedUserInfo = _encryptService.Encrypt (jsonUser);

            return Ok (new { name = user.name, security = encriptedUserInfo });
        }

    }
}