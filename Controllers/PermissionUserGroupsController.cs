using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using securityfilter;
using securityservice.Model;
using securityservice.Services.Interfaces;

namespace securityservice.Controllers {
    [Route ("api/usergroups/permissions/")]
    public class PermissionUserGroupsController : Controller {
        private readonly IGroupService _userGroupService;
        public PermissionUserGroupsController (IGroupService userGroupService) {
            _userGroupService = userGroupService;
        }

        [HttpPost ("{usergroupId}")]
        [SecurityFilter ("usergroups__allow_update")]

        public async Task<IActionResult> Post (int usergroupId, [FromQuery] string permission) {
            if (ModelState.IsValid) {
                var group = await _userGroupService.AddPermisionToUserGroup (usergroupId, permission);
                if (group != null)
                    return Ok (group);
                return NotFound ();
            }
            return BadRequest (ModelState);
        }

        [HttpDelete ("{usergroupId}")]
        [SecurityFilter ("usergroups__allow_update")]
        public async Task<IActionResult> Delete (int usergroupId, [FromQuery] string permission) {
            if (ModelState.IsValid) {
                var group = await _userGroupService.RemovePermisionFromUserGroup (usergroupId, permission);
                if (group != null)
                    return Ok (group);
                return NotFound ();
            }
            return BadRequest (ModelState);
        }
    }
}