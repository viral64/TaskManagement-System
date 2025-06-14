using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Multi_Tenant_Task_Management_System.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BaseController : ControllerBase
    {
        protected int GetCompanyId()
        {
            var companyIdClaim = User.Claims.FirstOrDefault(c => c.Type == "CompanyId");
            return int.Parse(companyIdClaim?.Value ?? throw new UnauthorizedAccessException("CompanyId missing in token"));
        }

        protected int GetUserId()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return int.Parse(userIdClaim?.Value ?? throw new UnauthorizedAccessException("UserId missing in token"));
        }
    }
}
