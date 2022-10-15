using BL;
using ClassModel;
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
    public class BaseController<T> : ControllerBase
    {
        protected IBLBase _blBase;
        protected IConfiguration _configuration;

        public BaseController(IBLBase bLBase, IConfiguration configuration) {
            _blBase = bLBase;
            _configuration = configuration;
        }

        [HttpGet("{idRecord}")]
        public IActionResult GetById(string idRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _blBase.GetById<T>(Guid.Parse(idRecord));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("getpaging")]
        public IActionResult GetPaging(string idRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _blBase.GetPaging();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] T newRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _blBase.Insert(newRecord);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpPut]
        public IActionResult Update([FromBody] T record)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _blBase.Update(record);
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpDelete("{idRecord}")]
        public IActionResult Delete(string idRecord)
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _blBase.Delete<T>(Guid.Parse(idRecord));
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            ServiceResult serviceResult = new ServiceResult();
            try
            {
                serviceResult.Data = _blBase.GetAll<T>();
            }
            catch (Exception ex)
            {
                System.Console.WriteLine(ex);
            }
            return Ok(serviceResult);
        }
    }
}
