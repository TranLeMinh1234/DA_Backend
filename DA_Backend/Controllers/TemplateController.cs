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
                throw;
            }
            return Ok(serviceResult);
        }
    }
}
