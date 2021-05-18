using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Bank.Issuer.Library.Enums;
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
    public class PaymentController : ControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger,
            IAccountService accountService,
            IPaymentService paymentService)
        {
            _logger = logger;
            _accountService = accountService;
            _paymentService = paymentService;
        }

        // POST: Payment/
        [HttpPost]
        public async Task<IActionResult> Make([FromBody] BaseRequest request)
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
                    return Ok(new BaseResponse { HasError = false, Message = "Account Not Found" });
                }

                if (request.MessageType.ToUpper()==MessageType.PAYMENT.ToString() && accountModel.Balance<Convert.ToDouble(request.Amount))
                {
                    return Ok(new BaseResponse {HasError = false, Message = "Not enough balance"});
                }
               
                return Ok(await _paymentService.PaymentActionAsync(request));
            }
            catch (Exception e)
            {
                _logger.LogError($"{e.Message} - StackTrace : {e.StackTrace}");
                return StatusCode(StatusCodes.Status500InternalServerError, "Error Occurred");
            }
        }
    }
}
