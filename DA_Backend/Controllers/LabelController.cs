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
    public class LabelController : BaseController<Label>
    {
        private IBLLabel _iBLLabel;
        private IConfiguration _configuration;
        public LabelController(IBLLabel iBLLabel, IConfiguration configuration) : base(iBLLabel,configuration) { 
            _iBLLabel = iBLLabel;
            _configuration = configuration;
        }

        [HttpPost("insertCustom")]
        public IActionResult InsertLabelCustom([FromBody] Label newRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLLabel.InsertLabelCustom(newRecord);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPut("updateCustom")]
        public IActionResult UpdateLabelCustom([FromBody] Label newRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLLabel.UpdateLabelCustom(newRecord);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
