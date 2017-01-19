using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Data.Entity;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Models;
using TaskAPI.AspNetCore.Web.Models.Persistent;
using System.Net;

namespace TaskAPI.AspNetCore.Web
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly ITaskService _taskService;

        public UserController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/user
        [HttpGet]
        public IEnumerable<User> GetAll()
        {
            return _taskService.Users.Where(p=> p.IsDeleted != true).ToList();
        }

        // GET api/user/sample@mail.com
        [HttpGet("{email}")]
        public User Get(string email)
        {
            var item = _taskService.Users.Find(obj => (string.Compare(email, obj.EmailAddress, true) == 0) && !obj.IsDeleted.GetValueOrDefault());
            if (item != null)
            {
                return item;
            }
            else
            {
                HttpContext.Response.StatusCode = (int)HttpStatusCode.NotFound;
                return item;
            }          
        }

        // POST api/user
        [HttpPost]
        public ActionResult Post([FromBody]CreateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            else
            {
                var itemExists = _taskService.Users.Any(i => (string.Compare(i.EmailAddress, request.EmailAddress, true) == 0));
                if (itemExists)
                {
                    return BadRequest();
                }
                User item = new User();
                item.userId= Guid.NewGuid().ToString().Replace("-", "");
                item.CreatedOnUtc = DateTime.UtcNow;
                item.UpdatedOnUtc = DateTime.UtcNow;
                item.EmailAddress = request.EmailAddress;
                if (_taskService.AddUser(item))
                {
                    HttpContext.Response.StatusCode = 201;
                    return Ok();
                }
                else
                {
                    return BadRequest();   
                }
            }
        }

        // DELETE api/user/3ab4fcbd993f49ce8a21103c713bf47a
        [HttpDelete]
        public IActionResult Delete([FromBody]DeleteUserRequest request)
        {
            var item = _taskService.Users.FirstOrDefault(x => x.userId == request.UserId && x.IsDeleted != true);
            if (item == null)
            {
                return NotFound();
            }
            item.IsDeleted = true;
            item.UpdatedOnUtc = DateTime.UtcNow;
            _taskService.RemoveUser(item);

            return new StatusCodeResult(204); // 201 No Content
        }
    }



}
