using BL.Interface;
using ClassModel;
using ClassModel.Notification;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : BaseController<Notification>
    {
        private IBLNotification _iBLNotification;
        private IConfiguration _configuration;

        public NotificationController(IBLNotification iBLNotification, IConfiguration configuration) : base(iBLNotification, configuration)
        {
            _iBLNotification = iBLNotification;
            _configuration = configuration;
        }

        [HttpGet("{email}/{startIndexTake}/{numberOfRecordTake}")]
        public IActionResult GetPagingCustom(string email,int startIndexTake, int numberOfRecordTake)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLNotification.GetPagingCustom(email, startIndexTake, numberOfRecordTake);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("numberofnewnotification")]
        public IActionResult GetNumberOfNewNotification()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLNotification.GetNumberOfNewNotification();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
