using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Multi_Tenant_Task_Management_System.Data;
using Multi_Tenant_Task_Management_System.DTOs;
using Multi_Tenant_Task_Management_System.Models;
using Multi_Tenant_Task_Management_System.Models.QueryParameters;

namespace Multi_Tenant_Task_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        public TasksController(ApplicationDbContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllTasks([FromQuery] TaskQueryParameters query)
        {
            var companyId = GetCompanyId();

            var tasksQuery = _context.Tasks
                .Include(t => t.Project)
                .Include(t => t.AssignedTo)
                .Where(t => t.Project.CompanyId == companyId && !t.IsDeleted);

            //  Search by Title (task name)
            if (!string.IsNullOrWhiteSpace(query.Title))
                tasksQuery = tasksQuery.Where(t => t.Title.Contains(query.Title));

            //  Search by Project Name
            if (!string.IsNullOrWhiteSpace(query.ProjectName))
                tasksQuery = tasksQuery.Where(t => t.Project.Name.Contains(query.ProjectName));

            //  Sorting
            tasksQuery = query.SortBy?.ToLower() switch
            {
                "title" => query.IsDescending ? tasksQuery.OrderByDescending(t => t.Title) : tasksQuery.OrderBy(t => t.Title),
                "createdat" => query.IsDescending ? tasksQuery.OrderByDescending(t => t.CreatedAt) : tasksQuery.OrderBy(t => t.CreatedAt),
                _ => tasksQuery.OrderByDescending(t => t.CreatedAt)
            };

            //  Pagination
            int skip = (query.Page - 1) * query.PageSize;
            var pagedTasks = tasksQuery.Skip(skip).Take(query.PageSize).ToList();

            var taskDtos = _mapper.Map<List<TaskDto>>(pagedTasks);

            return Ok(taskDtos);
        }
        // POST: api/tasks
        [HttpPost]
        public IActionResult CreateTask([FromBody] TaskEntity taskDto)
        {
            var companyId = GetCompanyId();

            // Validate Project belongs to company
            var project = _context.Projects.FirstOrDefault(p => p.Id == taskDto.ProjectId && p.CompanyId == companyId && !p.IsDeleted);
            if (project == null)
                return BadRequest("Invalid project for your company.");

            // Validate Assigned User belongs to same company
            var user = _context.Users.FirstOrDefault(u => u.Id == taskDto.AssignedToUserId && u.CompanyId == companyId);
            if (user == null)
                return BadRequest("Assigned user is not part of your company.");

            var task = _mapper.Map<TaskEntity>(taskDto);
            task.CreatedAt = DateTime.UtcNow;
            task.CreatedBy = User.Identity?.Name ?? "System";

            _context.Tasks.Add(task); 
            _context.SaveChanges();
            var result = _mapper.Map<TaskDto>(task);
            return Ok(result);
        }

        // PUT: api/tasks/5
        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, [FromBody] TaskDto dto)
        {
            var companyId = GetCompanyId();

            var task = _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == id && t.Project.CompanyId == companyId);

            if (task == null) return NotFound();

            _mapper.Map(dto, task);
            task.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();

            var result = _mapper.Map<TaskDto>(task);
            return Ok(result);
        }

        // DELETE: api/tasks/5 (Soft Delete)
        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var companyId = GetCompanyId();

            var task = _context.Tasks
                .Include(t => t.Project)
                .FirstOrDefault(t => t.Id == id && t.Project.CompanyId == companyId && !t.IsDeleted);

            if (task == null)
                return NotFound();

            task.IsDeleted = true;
            task.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return NoContent();
        }
        [HttpGet("assignable-users")]
        public IActionResult GetAssignableUsers()
        {
            var companyId = GetCompanyId();

            var users = _context.Users
                .Where(u => u.CompanyId == companyId)
                .Select(u => new { u.Id, u.FullName, u.Email })
                .ToList();

            return Ok(users);
        }

    }
}
