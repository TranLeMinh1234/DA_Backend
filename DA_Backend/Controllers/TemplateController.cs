using BL.Interface;
using ClassModel;
using ClassModel.ParamApi;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

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

        [HttpPut("process")]
        public IActionResult UpdateProcess([FromBody] Process process)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.UpdateProcess(process);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost("insertprocess")]
        public IActionResult InsertProcess([FromBody] Process process)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.InsertProcess(process);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpDelete("process/{processId}/{columnSettingId}")]
        public IActionResult DeleteProcess(Guid processId, Guid columnSettingId)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.DeleteProcess(processId, columnSettingId);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPut("process/sortorder")]
        public IActionResult UpdateSortOrderProcesses(List<ParamUpdateSortOrderProcess> listParam)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLTemplateGroupTask.UpdateSortOrderProcesses(listParam);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
