using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Models;
using Swashbuckle.SwaggerGen.Annotations;
using TaskAPI.AspNetCore.Web.Models.Persistent;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    public class TaskListController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskListController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/tasklist/8ab4fcbd993f49ce8a21103c713bf47a
        [HttpGet("{userId}")]
        public List<TaskList> GetAll(string userId)
        {
            return _taskService.TaskLists.Where(p => p.UserId == userId && p.IsDeleted != true).ToList();
        }


        // POST api/tasklist
        [HttpPost]
        public ActionResult Post([FromBody]CreateTaskListRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            else
            {
                var userExists = _taskService.Users.Any(i => i.userId == request.UserId);
                if (userExists)
                {
                    var itemExists = _taskService.TaskLists.Any(i => i.Title == request.TaskListTitle && i.UserId == request.UserId);
                    if (itemExists)
                    {
                        return BadRequest();
                    }
                    
                    TaskList item = new Models.TaskList();
                    item.TaskListId = Guid.NewGuid().ToString().Replace("-", "");
                    item.UserId = request.UserId;
                    item.CreatedOnUtc = DateTime.UtcNow;
                    item.UpdatedOnUtc = DateTime.UtcNow;
                    item.Title = request.TaskListTitle;
                    _taskService.AddTaskList(item);
                    
                    HttpContext.Response.StatusCode = 201;
                    return Ok();
                }
                else
                {
                    return BadRequest();
                }
            }
        }

        // DELETE api/tasklist/5ab4fcbd993f49ce8a21103c713bf47a
        [HttpDelete]
        [SwaggerResponse(System.Net.HttpStatusCode.NoContent)]
        public IActionResult Delete([FromBody]DeleteTaskListRequest request)
        {
            var item = _taskService.TaskLists.FirstOrDefault(x => x.TaskListId == request.TaskListId
            && x.UserId == request.UserId && x.IsDeleted != true);
            if (item == null)
            {
                return NotFound();
            }
            item.IsDeleted = true;
            item.UpdatedOnUtc = DateTime.UtcNow;
            _taskService.RemoveTaskList(item);
            return new StatusCodeResult(204); // 201 No Content
        }
    }
}
