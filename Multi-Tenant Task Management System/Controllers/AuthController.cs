using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Multi_Tenant_Task_Management_System.Data;
using Multi_Tenant_Task_Management_System.DTOs;
using Multi_Tenant_Task_Management_System.Helpers;

namespace Multi_Tenant_Task_Management_System.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public AuthController(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequestDto loginDto)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == loginDto.Email && u.PasswordHash == loginDto.Password);

            if (user == null)
                return Unauthorized("Invalid credentials.");

            var token = JwtTokenGenerator.GenerateToken(user, _config);

            return Ok(new { token });
        }
    }
}
