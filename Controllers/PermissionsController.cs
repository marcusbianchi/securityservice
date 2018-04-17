using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using securityservice.Model;
using securityservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace securityservice.Controllers {
    [Route ("api/[controller]")]
    public class PermissionsController : Controller {
        private readonly IPermisionService _permisionService;

        public PermissionsController (IPermisionService permisionService) {
            _permisionService = permisionService;
        }

        [HttpGet]
        [ResponseCache (CacheProfileName = "accesscache")]
        public ActionResult Get () {
            var permissions = _permisionService.getPermisions ();
            return Ok (permissions);
        }

    }
}