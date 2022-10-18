using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Exceptions
{
    public class InternalServerErrorException :Exception
    {
        public static Int16 Error(Int16 errorCode)
        {
            return (errorCode > 0 ? errorCode : Convert.ToInt16(StatusCodes.Status500InternalServerError));
        }
    }
}
