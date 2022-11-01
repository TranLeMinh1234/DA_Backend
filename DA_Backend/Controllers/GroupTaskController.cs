using BL.Interface;
using ClassModel;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupTaskController : BaseController<GroupTask>
    {
        private IBLGroupTask _iBLGroupTask;
        private IConfiguration _configuration;
        public GroupTaskController(IBLGroupTask iBLGroupTask, IConfiguration configuration) : base(iBLGroupTask, configuration)
        {
            _iBLGroupTask = iBLGroupTask;
            _configuration = configuration;
        }

        [HttpPost("insertcustom")]
        public IActionResult InsertCustom([FromBody] ParamInserGroupTask paramInserGroupTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.InsertCustom(paramInserGroupTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        
        [HttpGet("havejoined")]
        public IActionResult GetGroupTaskHaveJoined()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetGroupTaskHaveJoined();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("usersjoined/{groupTaskId}")]
        public IActionResult GetUserJoined(Guid groupTaskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetUserJoined(groupTaskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("infotemplate/{groupTaskId}/{templateReferenceId}")]
        public IActionResult GetInfoTemplate(Guid groupTaskId, Guid templateReferenceId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetInfoTemplate(groupTaskId, templateReferenceId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("alltask")]
        public IActionResult GetAllTask([FromBody] ParamGetAllTask paramGetAllTask )
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetAllTask(paramGetAllTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
