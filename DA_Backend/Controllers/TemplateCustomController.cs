using BL.Interface;
using ClassModel.TaskRelate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TemplateCustomController : BaseController<TemplateCustom>
    {
        private IBLTemplateCustom _iBLTemplateCustom;
        private IConfiguration _configuration;
        public TemplateCustomController(IBLTemplateCustom iBLTemplateCustom, IConfiguration configuration) : base(iBLTemplateCustom, configuration)
        {
            _iBLTemplateCustom = iBLTemplateCustom;
            _configuration = configuration;
        }
    }
}
