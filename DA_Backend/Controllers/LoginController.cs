using BL;
using BL.Interface;
using ClassModel;
using ClassModel.User;
using DL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : BaseController<User>
    {
        protected IBLLogin _ibLLogin;

        public LoginController(IBLLogin bLLogin) : base(bLLogin)
        {
            _ibLLogin = bLLogin;
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
    }
}
