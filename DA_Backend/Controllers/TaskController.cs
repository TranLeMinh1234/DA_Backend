using BL.Interface;
using ClassModel;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DA_Backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaskController : BaseController<Task>
    {
        private IBLTask _iBLTask;
        private IConfiguration _configuration;
        public TaskController(IBLTask iBLTask, IConfiguration configuration) : base(iBLTask,configuration)
        {
            _iBLTask = iBLTask;
            _configuration = configuration;
        }

        [HttpPost("insertChildTask")]
        public IActionResult InsertTaskCustom([FromBody] Task newRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.InsertChildTask(newRecord);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("getChildTask/{taskId}")]
        public IActionResult GetChildTask(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.GetChildTask(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
