using System;
using System.Collections.Generic;
using System.Text;
using Bank.Issuer.Library.Models.Base;

namespace Bank.Issuer.Library.Models
{
    public class AccountModel:BaseModel
    {
        public double Balance { get; set; }
    }
}
