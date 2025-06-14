using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Multi_Tenant_Task_Management_System.Data;
using Multi_Tenant_Task_Management_System.DTOs;
using Multi_Tenant_Task_Management_System.Models;

namespace Multi_Tenant_Task_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TaskCommentsController : BaseController
    {
        private readonly ApplicationDbContext _context;

        public TaskCommentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // POST: api/TaskComments
        [HttpPost]
        public IActionResult AddComment([FromBody] AddCommentDto dto)
        {
            var companyId = GetCompanyId();
            var userId = GetUserId();

            var task = _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == dto.TaskEntityId && t.Project.CompanyId == companyId && !t.IsDeleted);

            if (task == null)
                return NotFound("Task not found or does not belong to your company.");

            var comment = new TaskComment
            {
                Comment = dto.Comment,
                TaskEntityId = dto.TaskEntityId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskComments.Add(comment);
            _context.SaveChanges();

            return Ok(comment);
        }

        // GET: api/TaskComments/task/5
        [HttpGet("task/{taskId}")]
        public IActionResult GetCommentsForTask(int taskId)
        {
            var companyId = GetCompanyId();

            var task = _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == taskId && t.Project.CompanyId == companyId && !t.IsDeleted);

            if (task == null)
                return NotFound("Task not found or does not belong to your company.");

            var comments = _context.TaskComments
                .Include(c => c.User)
                .Where(c => c.TaskEntityId == taskId)
                .Select(c => new
                {
                    c.Id,
                    c.Comment,
                    c.CreatedAt,
                    User = new { c.User.FullName, c.User.Email }
                })
                .ToList();

            return Ok(comments);
        }
    }
}
