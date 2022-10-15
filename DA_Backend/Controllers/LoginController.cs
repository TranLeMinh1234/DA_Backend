using BL;
using BL.Interface;
using ClassModel;
using ClassModel.User;
using DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController<User>
    {
        protected IBLLogin _ibLLogin;
        protected IConfiguration _configuration;
        public LoginController(IBLLogin bLLogin, IConfiguration configuration) : base(bLLogin,configuration)
        {
            _ibLLogin = bLLogin;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public ServiceResult RegisterUser([FromBody]User newUser)
        { 
            ServiceResult result = new ServiceResult();
            try {
                result = _ibLLogin.Register(newUser);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public ServiceResult Login([FromBody] Account account)
        {
            ServiceResult result = new ServiceResult();
            try
            {
                result = _ibLLogin.Login(account, _configuration["Jwt:KeyLoggin"]);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return result;
        }
    }
}
