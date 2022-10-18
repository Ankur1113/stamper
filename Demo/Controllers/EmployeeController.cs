using Demo.Common;
using Demo.Exceptions;
using Demo.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Threading.Tasks;
using static Demo.Common.CommonEnum;

namespace Demo.Controllers
{
    
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService EmployeeService;
 
        public EmployeeController(IEmployeeService employeeService)
        {
            this.EmployeeService = employeeService;
        }
       
        [HttpPost, DisableRequestSizeLimit]
        public async Task<IActionResult> DocumentAdd(IFormFile pdffile)
        {
            ResponseHelper objHelper = new ResponseHelper();
            try
            {
                var fileBase64 = await EmployeeService.pdffile(pdffile);
                objHelper.Status = StatusCodes.Status200OK;
                objHelper.Message = ErrrorMessageEnum.AddedSuccessfully;
                objHelper.Data = fileBase64;

                return Ok(objHelper);
            }
            catch (Exception ex)
            {
                objHelper.Status = InternalServerErrorException.Error(Convert.ToInt16(ex.Data["ErrorCode"]));
                objHelper.Message = ex.Message;
                return StatusCode(objHelper.Status, objHelper);
            }
        }

    }
}
