using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Libraries.Base;
using Bank.Issuer.Library.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bank.Issuer.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;

        public AccountController(ILogger<AccountController> logger,
            IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        // POST: Account/
        [HttpPost]
        public async Task<IActionResult> Account([FromBody] BaseRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var accountModel = await _accountService.GetAccountBalanceAsync(Convert.ToInt64(request.AccountId));

                if (accountModel == null)
                {
                    return Ok(new BaseResponse() { Balance = 0, HasError = false, Message = "Account Not Found" });
                }

                return Ok(new BaseResponse(){Balance = accountModel.Balance,HasError = false,Message = "Successful"});
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} - StackTrace : {e.StackTrace}");
                return Ok(new BaseResponse() { Balance = 0, HasError = true, Message = "Error Occurred" });
            }
        }
    }
}
