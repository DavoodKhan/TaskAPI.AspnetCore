using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskAPI.Models;
using TaskAPI.AspNetCore.Web.Models.Persistent;

namespace TaskAPI.Controllers
{
    [Route("api/[controller]")]
    public class TaskController : Controller
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // GET: api/task/2ab4fcbd993f49ce8a21103c713bf47a
        [HttpGet("{taskListId}")]
        public IEnumerable<Models.Task> GetAll(string taskListId)
        {
            return _taskService.Tasks.Where(p => p.TaskListId == taskListId && p.IsDeleted != true).ToList();
        }


        // POST api/task
        [HttpPost]
        public ActionResult Post([FromBody]CreateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            else
            {
                var itemExists = _taskService.Tasks.Any(i => i.Title == request.TaskTitle && i.TaskListId == request.TaskListId && i.IsDeleted != true);
                if (itemExists)
                {
                    return BadRequest();
                }
                Models.Task item = new Models.Task();
                item.TaskListId = request.TaskListId;
                item.TaskId = Guid.NewGuid().ToString().Replace("-", ""); ;
                item.CreatedOnUtc = DateTime.UtcNow;
                item.UpdatedOnUtc = DateTime.UtcNow;
                item.Title = request.TaskTitle;

                _taskService.AddTask(item);
                
                var tasks = _taskService.Tasks.Where(i => i.TaskListId == request.TaskListId && i.IsDeleted != true).Select(p => new { Title = p.Title }).ToList();
                var getTaskList = _taskService.TaskLists.Where(i => i.TaskListId == request.TaskListId).SingleOrDefault();
                var user = _taskService.Users.Where(u => u.userId == getTaskList.UserId).SingleOrDefault();
                return Json(new { User = user.EmailAddress, Tasks = tasks, TaskList = getTaskList.Title });
            }
        }

        // PUT api/task
        [HttpPut]
        public ActionResult Put([FromBody]UpdateTaskRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            else
            {
                var itemExists = _taskService.Tasks.SingleOrDefault(i => i.TaskId == request.TaskId && i.TaskListId == request.TaskListId && i.IsDeleted != true);
                if (itemExists != null)
                {
                    // parse the updated properties
                    foreach (var item in request.Data)
                    {
                        switch (item.Key)
                        {
                            case TaskPropertyEnum.IsCompleted:
                                itemExists.IsCompleted = bool.Parse(item.Value);
                                break;
                            case TaskPropertyEnum.CompletedOn:
                                itemExists.CompletedOnUtc = DateTime.Parse(item.Value);
                                break;
                            case TaskPropertyEnum.DueOn:
                                itemExists.DueOnUtc = DateTime.Parse(item.Value);
                                break;
                            case TaskPropertyEnum.IsActive:
                                itemExists.IsActive = bool.Parse(item.Value);
                                break;
                            case TaskPropertyEnum.Title:
                                itemExists.Title = item.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    _taskService.UpdateTask(itemExists);

                    HttpContext.Response.StatusCode = 201;
                    return Ok();
                }
                return BadRequest(new { Message = "Record not found. Make sure it exists" });
            }
        }

        // DELETE api/task/1ab4fcbd993f49ce8a21103c713bf47a
        [HttpDelete]
        public IActionResult Delete([FromBody]DeleteTaskRequest request)
        {
            var item = _taskService.Tasks.FirstOrDefault(x => x.TaskId == request.TaskId && x.TaskListId == request.TaskListId && x.IsDeleted != true);
            if (item == null)
            {
                return NotFound();
            }
            item.IsDeleted = true;
            item.UpdatedOnUtc = DateTime.UtcNow;
            _taskService.RemoveTask(item);
            return new StatusCodeResult(204); // 201 No Content
        }
    }
}
