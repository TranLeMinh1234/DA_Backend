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
    public class TemplateController : BaseController<TemplateGroupTask>
    {
        private IBLTemplateGroupTask _iBLTemplateGroupTask;
        private IConfiguration _configuration;
        public TemplateController(IBLTemplateGroupTask iBLTemplateGroupTask, IConfiguration configuration) : base(iBLTemplateGroupTask, configuration)
        {
            _iBLTemplateGroupTask = iBLTemplateGroupTask;
            _configuration = configuration;
        }

        [HttpPost("insertcustom")]
        public IActionResult InsertCustom([FromBody] TemplateGroupTask templateGroupTask)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.InsertCustom(templateGroupTask);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("getall/havepermission")]
        public IActionResult GetAllTemplate()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.GetAllTemplate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpDelete("deletecustom/{templateId}")]
        public IActionResult DeleteCustom(Guid templateId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.DeleteCustom(templateId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
