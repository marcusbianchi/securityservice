using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using securityservice.Model;
using securityservice.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace securityservice.Controllers {
    [Route ("api/[controller]")]
    public class UserGroupsController : Controller {
        private readonly IGroupService _userGroupService;

        public UserGroupsController (IGroupService userGroupService) {
            _userGroupService = userGroupService;
        }

        [HttpGet]
        [ResponseCache (CacheProfileName = "accesscache")]
        public async Task<IActionResult> Get ([FromQuery] int startat, [FromQuery] int quantity, [FromQuery] string fieldFilter, [FromQuery] string fieldValue, [FromQuery] string orderField, [FromQuery] string order) {

            var fieldFilterEnum = GroupFieldEnum.Default;
            Enum.TryParse (fieldFilter, true, out fieldFilterEnum);
            var orderFieldEnum = GroupFieldEnum.Default;
            Enum.TryParse (orderField, true, out orderFieldEnum);
            var orderEnumValue = OrderEnum.Ascending;
            Enum.TryParse (order, true, out orderEnumValue);

            if (quantity == 0)
                quantity = 50;

            var (groups, total) = await _userGroupService.getUserGroups (startat, quantity, fieldFilterEnum, fieldValue, orderFieldEnum, orderEnumValue);

            return Ok (new { values = groups, total = total });
        }

        [HttpGet("name/{groupName}")]
        [ResponseCache (CacheProfileName = "accesscache")]
        public async Task<IActionResult> GetByName (string groupName) {

            var group = await _userGroupService.getUserByName (groupName);
            if (group == null)
                NotFound ();
            return Ok (group);
        }

        [HttpGet ("list/")]
        [ResponseCache (CacheProfileName = "accesscache")]
        public async Task<IActionResult> GetList ([FromQuery] int[] usergroupId) {
            var groups = await _userGroupService.getUserGroupssById (usergroupId);
            return Ok (groups);
        }

        [HttpGet ("{id}")]
        [ResponseCache (CacheProfileName = "accesscache")]
        public async Task<IActionResult> Get (int id) {
            var groups = await _userGroupService.getGroup (id);
            if (groups == null)
                return NotFound ();
            return Ok (groups);
        }

        [HttpPost]
        public async Task<IActionResult> Post ([FromBody] UserGroup group) {
            if (ModelState.IsValid) {
                group = await _userGroupService.createUserGroup (group);
                return Created ($"api/users/{group.userGroupId}", group);
            }
            return BadRequest (ModelState);
        }

        [HttpPut ("{id}")]
        public async Task<IActionResult> Put (int id, [FromBody] UserGroup group) {
            if (ModelState.IsValid) {
                group = await _userGroupService.updateUserGroup (id, group);
                if (group == null && id != group.userGroupId) {
                    return NotFound ();
                }
                return NoContent ();
            }
            return BadRequest (ModelState);
        }

        [HttpDelete ("{id}")]
        public async Task<IActionResult> Delete (int id) {
            var user = await _userGroupService.deleteUserGroup (id);
            if (user != null) {
                return NoContent ();
            }
            return NotFound ();
        }

    }
}