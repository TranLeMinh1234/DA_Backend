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
        public IActionResult GetAllTask([FromBody] ParamGetAllTask paramGetAllTask)
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

        [HttpPut("deleteCustom/")]
        public IActionResult DeleteCustom([FromBody] ParamDeletGroupTask paramDeletGroupTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.DeleteCustom(paramDeletGroupTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("addMembers")]
        public IActionResult AddMemebers([FromBody] ParamAddMember paramAddMember)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.AddMemebers(paramAddMember);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpDelete("members/{email}/{groupTaskId}/{nameGroupTask}")]
        public IActionResult DeleteMember(string email, Guid groupTaskId, string nameGroupTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.DeleteMember(email, groupTaskId, nameGroupTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPut("updaterolemember")]
        public IActionResult UpdateRoleMember([FromBody] ParamUpdateRoleMember paramUpdateRoleMember)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.UpdateRoleMember(paramUpdateRoleMember.Email, paramUpdateRoleMember.GroupTaskId, paramUpdateRoleMember.RoleId, paramUpdateRoleMember.NameGroupTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("generalCount/{groupTaskId}")]
        public IActionResult GetGeneralCount(Guid groupTaskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetGeneralCount(groupTaskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("taskeachmember/{groupTaskId}")]
        public IActionResult TaskEachMember(Guid groupTaskId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.TaskEachMember(groupTaskId);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("getstatusexecutetask")]
        public IActionResult GetStatusExecuteTask(ParamGetStatusExecuteTask paramGetStatusExecuteTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetStatusExecuteTask(paramGetStatusExecuteTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("numoftaskpersonal")]
        public IActionResult GetNumOfTaskPersonal(ParamGetStatusExecuteTask paramGetStatusExecuteTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLGroupTask.GetNumOfTaskPersonal(paramGetStatusExecuteTask);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
