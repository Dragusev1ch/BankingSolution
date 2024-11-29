using BankingSolution.Dtos.Deposit;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    public class TransactionController : Controller
    {
        ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost]
        [Route("api/transaction/deposit")]
        public IActionResult Deposit([FromBody] DepositDto dto)
        {
            try
            {
                _transactionService.Deposit(dto.AccountId, dto.Amount);
                return Ok("Deposit successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
