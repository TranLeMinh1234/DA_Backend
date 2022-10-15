using BL.Interface;
using ClassModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseController<ClassModel.User.User>
    {
        private IBLUser _iBLUser;
        private IConfiguration _configuration;
        public UserController(IBLUser iBLUser, IConfiguration configuration) : base(iBLUser, configuration)
        {
            _iBLUser = iBLUser;
            _configuration = configuration;
        }

        [HttpGet("getUserInfo")]
        public IActionResult GetUserInfo()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLUser.GetUserInfo();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
