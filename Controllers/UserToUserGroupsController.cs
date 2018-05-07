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
    [Route ("api/usergroups/users/")]
    public class UserToUserGroupsController : Controller {
        private readonly IGroupService _userGroupService;
        public UserToUserGroupsController (IGroupService userGroupService) {
            _userGroupService = userGroupService;
        }

        [HttpPost ("{usergroupId}")]
        [SecurityFilter ("usergroups__allow_update")]

        public async Task<IActionResult> Post (int usergroupId, [FromBody] User user) {
            if (ModelState.IsValid) {
                var group = await _userGroupService.AddUserToUserGroup (usergroupId, user);
                if (group != null)
                    return Ok (group);
                return NotFound ();
            }
            return BadRequest (ModelState);
        }

        [HttpDelete ("{usergroupId}")]
        [SecurityFilter ("usergroups__allow_update")]
        public async Task<IActionResult> Delete (int usergroupId, [FromBody] User user) {
            if (ModelState.IsValid) {
                var group = await _userGroupService.RemoveUserFromUserGroup (usergroupId, user);
                if (group != null)
                    return Ok (group);
                return NotFound ();
            }
            return BadRequest (ModelState);
        }

    }
}