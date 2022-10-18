using BL.Interface;
using ClassModel;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Xml;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : BaseController<Comment>
    {
        private IBLComment _iBLComment;
        private IConfiguration _configuration;

        public CommentController(IBLComment iBLComment, IConfiguration configuration) : base(iBLComment, configuration)
        {
            _iBLComment = iBLComment;
            _configuration = configuration;
        }

        [HttpPost("{taskId}")]
        public IActionResult InsertCustom(Guid taskId, [FromBody] Comment comment)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _iBLComment.InsertCustom(taskId, comment);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

    }
}
