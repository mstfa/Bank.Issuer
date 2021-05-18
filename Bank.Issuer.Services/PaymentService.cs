using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bank.Issuer.Library.Entities;
using Bank.Issuer.Library.Interfaces;
using Bank.Issuer.Library.Libraries;
using Bank.Issuer.Library.Libraries.Base;
using Bank.Issuer.Library.Models;
using Bank.Issuer.Services.Base;
using MessageTypeEnum = Bank.Issuer.Library.Enums.MessageType;
using OriginEnum = Bank.Issuer.Library.Enums.Origin;

namespace Bank.Issuer.Services
{
    public class PaymentService: BaseService, IPaymentService
    {
        private readonly IRepository<Account> _accountRepository;
        private readonly IRepository<Transaction> _transactionRepository;
        private readonly IRepository<MessageType> _messageTypeRepository;
        private readonly IRepository<Origin> _originRepository;
        private readonly ICalculationService _calculationService;

        public PaymentService(IServiceProvider provider,
            IRepository<Account> accountRepository,
            IRepository<Transaction> transactionRepository,
            IRepository<MessageType> messageTypeRepository,
            IRepository<Origin> originRepository,
            ICalculationService calculationService
        ) : base(provider)
        {
            _accountRepository = accountRepository;
            _transactionRepository = transactionRepository;
            _messageTypeRepository = messageTypeRepository;
            _originRepository = originRepository;
            _calculationService = calculationService;
        }

        public async Task<PaymentResponse> PaymentActionAsync(BaseRequest paymentRequest)
        {
           
            var account = await _accountRepository.GetByConditionAsync(x=>x.AccountNumber== Convert.ToInt64(paymentRequest.AccountId));

            Enum.TryParse(paymentRequest.MessageType, out MessageTypeEnum messageType);

            var acc = account?.FirstOrDefault();
            if (acc != null)
            {
                switch (messageType)
                {
                    case MessageTypeEnum.ADJUSTMENT:
                        return await Adjustment(acc,paymentRequest);
                    case MessageTypeEnum.PAYMENT:
                        return await Payment(acc,paymentRequest);
                }
            }

            return new PaymentResponse() { HasError = false, Message = "Something is wrong" };
        }


        #region PrivateMethods
        private async Task<PaymentResponse> Adjustment(Account account, BaseRequest paymentRequest)
        {
            var transaction=await _transactionRepository.GetAsync(Guid.Parse(paymentRequest.TransactionId));
            var messageTypeFromDb = await _messageTypeRepository.GetByConditionAsync(x => x.Type.ToLower() == paymentRequest.MessageType.ToLower());
            var originFromDb = await _originRepository.GetByConditionAsync(x => x.Name.ToLower() == paymentRequest.Origin.ToLower());

            if (transaction==null)
            {
                return new PaymentResponse() { HasError = false, Message = "Transaction not found"};
            }
            
            if (transaction.Amount==Convert.ToDecimal(paymentRequest.Amount))
            {
                Enum.TryParse(paymentRequest.Origin, out OriginEnum origin);
                var paymentAmount = Convert.ToDecimal(paymentRequest.Amount);
                var amountWithCommission =await  _calculationService.CalculateCommissionByOrigin(origin, paymentAmount);
                account.Balance += (double)(paymentAmount + amountWithCommission);

                await _accountRepository.UpdateAsync(account);

                var newTransaction = await _transactionRepository.InsertAsync(new Transaction()
                {
                    AccountId = account.Id,
                    Amount = Convert.ToDecimal(paymentRequest.Amount),
                    DateCreated = DateTime.Now,
                    MessageTypeId = messageTypeFromDb.FirstOrDefault().Id,
                    OriginId = originFromDb.FirstOrDefault().Id
                });
                return new PaymentResponse()
                {
                    HasError = false,
                    Message = "Successful",
                    Balance = account.Balance,
                    TransactionId = newTransaction.ToString()
                };
            }

            return new PaymentResponse() { HasError = false, Message = "Transaction amount does not match" };
        }

        private async Task<PaymentResponse> Payment(Account account, BaseRequest paymentRequest)
        {
            var messageTypeFromDb = await _messageTypeRepository.GetByConditionAsync(x => x.Type.ToLower() == paymentRequest.MessageType.ToLower());
            var originFromDb = await _originRepository.GetByConditionAsync(x => x.Name.ToLower() == paymentRequest.Origin.ToLower());
            Enum.TryParse(paymentRequest.Origin, out OriginEnum origin);
            var paymentAmount = Convert.ToDecimal(paymentRequest.Amount);
            var amountWithCommission =await  _calculationService.CalculateCommissionByOrigin(origin, paymentAmount);
            account.Balance -= (double)(paymentAmount+amountWithCommission);

            await _accountRepository.UpdateAsync(account);

            var transaction = await _transactionRepository.InsertAsync(new Transaction()
            {
                AccountId = account.Id, 
                Amount = Convert.ToDecimal(paymentRequest.Amount),
                DateCreated = DateTime.Now,
                MessageTypeId = messageTypeFromDb.FirstOrDefault().Id,
                OriginId = originFromDb.FirstOrDefault().Id
            });

            return new PaymentResponse()
            {
                HasError = false, 
                Message = "Successful",
                Balance = account.Balance,
                TransactionId = transaction.ToString()
            };
        }
        #endregion

    }
}
