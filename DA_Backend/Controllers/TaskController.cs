using BL.Interface;
using ClassModel;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

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

        
        [HttpPost("label")]
        public IActionResult InsertLabelsTask(Dictionary<string,string> paramRequest)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                string listLabelId = null;
                string taskId = null;
                var getlistSuccess = paramRequest.TryGetValue("listLabelId",out listLabelId);
                var getTaskIdSuccess = paramRequest.TryGetValue("taskId", out taskId);
                List<string> listLabelIdParsed = JsonConvert.DeserializeObject<List<string>>(listLabelId);

                if (getlistSuccess && getTaskIdSuccess && listLabelIdParsed != null && listLabelIdParsed.Count > 0)
                {
                    serviceResult.Data = _iBLTask.InsertLabelsTask(Guid.Parse(taskId.ToString()), listLabelIdParsed);
                }
                else
                {
                    serviceResult.Success = false;
                    serviceResult.ErrorCode.Add("EmptyData");
                }

            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("label/{taskId}")]
        public IActionResult GetLabelsTask(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.GetLabelsTask(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpDelete("label/{taskId}/{labelId}")]
        public IActionResult DeleteLabelsTask(Guid taskId,Guid labelId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.DeleteLabelsTask(taskId, labelId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("comment/{taskId}")]
        public IActionResult GetCommentsTask(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.GetCommentsTask(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
