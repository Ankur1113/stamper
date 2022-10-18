using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Interfaces
{
    public interface IEmployeeService
    {
        Task<string>  pdffile(IFormFile pdffile);
    }
}
