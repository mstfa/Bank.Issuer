using System;
using System.Collections.Generic;

namespace Bank.Issuer.Library.Libraries.Base
{
    public class BaseResponse
    {
        public bool HasError { get; set; } = false;
        public List<Exception> Errors { get; set; }
        public string Message { get; set; }
        public double Balance { get; set; }
    }
}
