using BL.Interface;
using ClassModel.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : BaseController<Role>
    {
        private IBLRole _iBLRole;
        private IConfiguration _configuration;
        public RoleController(IBLRole iBLRole, IConfiguration configuration) : base(iBLRole, configuration)
        {
            _iBLRole = iBLRole;
            _configuration = configuration;
        }
    }
}
