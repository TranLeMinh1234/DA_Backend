using BL;
using ClassModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DA_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T> : ControllerBase
    {
        protected IBLBase _blBase;
        public BaseController(IBLBase bLBase) {
            _blBase = bLBase;
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

    }
}
