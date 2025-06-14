using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_Task_Management_System.Data;
using Multi_Tenant_Task_Management_System.DTOs;
using Multi_Tenant_Task_Management_System.Models;

namespace Multi_Tenant_Task_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectsController : BaseController

    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public ProjectsController(ApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/projects
        [HttpGet]
        public IActionResult GetProjects()
        {
            var companyId = GetCompanyId();

            var projects = _context.Projects
                .Where(p => p.CompanyId == companyId && !p.IsDeleted)
                .ToList();
            var result = _mapper.Map<List<ProjectDto>>(projects);
            return Ok(result);
        }

        // GET: api/projects/5
        [HttpGet("{id}")]
        public IActionResult GetProject(int id)
        {
            var companyId = GetCompanyId();

            var project = _context.Projects
                .FirstOrDefault(p => p.Id == id && p.CompanyId == companyId && !p.IsDeleted);

            if (project == null)
                return NotFound();

            return Ok(project);
        }

        // POST: api/projects
        [HttpPost]
        public IActionResult CreateProject([FromBody] ProjectDto dto)
        {
            var companyId = GetCompanyId();
            var project = _mapper.Map<Project>(dto);
            project.CompanyId = companyId;
            project.CreatedAt = DateTime.UtcNow;
            project.CreatedBy = User.Identity?.Name ?? "System";

            _context.Projects.Add(project);
            _context.SaveChanges();

            return Ok(_mapper.Map<ProjectDto>(project));
        }

        // PUT: api/projects/5
        [HttpPut("{id}")]
        public IActionResult UpdateProject(int id, [FromBody] ProjectDto dto)
        {
            var companyId = GetCompanyId();

            var project = _context.Projects.FirstOrDefault(p => p.Id == id && p.CompanyId == companyId && !p.IsDeleted);
            if (project == null)
                return NotFound();

            project.Name = dto.Name;
            project.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return Ok(_mapper.Map<ProjectDto>(project));
        }

        // DELETE: api/projects/5 (Soft delete)
        [HttpDelete("{id}")]
        public IActionResult DeleteProject(int id)
        {
            var companyId = GetCompanyId();

            var project = _context.Projects.FirstOrDefault(p => p.Id == id && p.CompanyId == companyId && !p.IsDeleted);
            if (project == null)
                return NotFound();

            project.IsDeleted = true;
            project.UpdatedAt = DateTime.UtcNow;

            _context.SaveChanges();
            return NoContent();
        }
    
}
}
