using BL.Interface;
using ClassModel;
using ClassModel.Email;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Service.Interface;
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
        public TaskController(IBLTask iBLTask, IConfiguration configuration) : base(iBLTask, configuration)
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

        [HttpPut("description")]
        public IActionResult UpdateDescription([FromBody] Dictionary<string,string> paramUpdate)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.UpdateDescription(paramUpdate);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("insertcustom")]
        public IActionResult InsertCustom([FromBody] Task newTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.InsertCustom(newTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }


        [HttpGet("getfullinfo/{taskId}")]
        public IActionResult GetFullInfo(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.GetFullInfo(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("dailytask")]
        public IActionResult GetDailyTask([FromBody] ParamDailyTask ParamDailyTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.GetDailyTask(ParamDailyTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpDelete("deleteCustom/{taskId}")]
        public IActionResult DeleteCustom(Guid taskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.DeleteCustom(taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPut("deadline")]
        public IActionResult UpdateDeadline([FromBody] ParamUpdateDeadline paramUpdateDeadline)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.UpdateDeadline(paramUpdateDeadline.typeDeadline, paramUpdateDeadline.newDeadline, paramUpdateDeadline.taskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }


        /*[HttpPost("testemail")]
        public async System.Threading.Tasks.Task<IActionResult> SendMail([FromBody] MailRequest request)
        {
            try
            {
                await _mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        [HttpPost("remind")]
        public async System.Threading.Tasks.Task<IActionResult> RemindTask([FromBody] ParamRemindTask request)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult = await _iBLTask.RemindTask(request);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok(serviceResult);
        }

        [HttpPut("processbatch")]
        public IActionResult UpdateTaskProcessBatch([FromBody] List<ParamUpdateTaskProcessBatch> listParam)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.UpdateTaskProcessBatch(listParam);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok(serviceResult);
        }

        
        [HttpPut("assignforuser/{groupTaskId}/{taskId}/{email}")]
        public IActionResult UpdateAssignForUser(Guid groupTaskId, Guid taskId,string email)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTask.UpdateAssignForUser(taskId, groupTaskId, email);
            }
            catch (Exception ex)
            {
                throw;
            }
            return Ok(serviceResult);
        }
    }
}
