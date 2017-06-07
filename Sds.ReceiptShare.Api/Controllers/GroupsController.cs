using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sds.ReceiptShare.Api.Models;
using Sds.ReceiptShare.Api.Mocks;

namespace Sds.ReceiptShare.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Groups")]
    public class GroupsController : Controller
    {
        /// <summary>
        /// Returns all the groups
        /// TODO: Need to identify the user and return the groups for a specific user
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IEnumerable<Group> Get()
        {
            var groups = ModelGenerator.GenerateGroupList();
            return groups;
        }

        /// <summary>
        /// Returns details for a given group id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var groups = ModelGenerator.GenerateGroupList();
            var matchedGroup = groups.FirstOrDefault(group => group.Id == id);

            if (matchedGroup != null) {
                return Ok(matchedGroup);
            }

            return NotFound();
        }

        /// <summary>
        /// Adds a new group
        /// TODO - need to identify the user and set them as the owner
        /// </summary>
        /// <param name="value"></param>
        [HttpPost]
        public void Post([FromBody]string value)
        {
            
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
    }
}