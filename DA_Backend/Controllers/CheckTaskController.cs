using BL.Interface;
using ClassModel;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckTaskController : BaseController<CheckTask>
    {
        private IBLCheckTask _iBLCheckTask;
        private IConfiguration _configuration;

        public CheckTaskController(IBLCheckTask iBLCheckTask, IConfiguration configuration) : base(iBLCheckTask, configuration)
        {
            _iBLCheckTask = iBLCheckTask;
            _configuration = configuration;
        }

        [HttpGet("getchecktasks/{taskId}")]
        public IActionResult GetChildTask(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLCheckTask.GetCheckTasks(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
