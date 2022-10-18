using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Exceptions
{
    public class RecoredNotFoundException :Exception
    {
        public RecoredNotFoundException()
        {
            base.Data.Add("ErrorCode", StatusCodes.Status204NoContent);
        }

        public RecoredNotFoundException(string message) : base(message)
        {
            base.Data.Add("ErrorCode", StatusCodes.Status204NoContent);
        }

        public RecoredNotFoundException(string message, Exception inner) : base(message, inner)
        {
            base.Data.Add("ErrorCode", StatusCodes.Status204NoContent);
        }
    }
}
