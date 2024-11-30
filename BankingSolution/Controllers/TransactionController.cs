using BankingSolution.Dtos.Deposit;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }
        [HttpPost("accounts/deposit")]
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

        [HttpPost("accounts/withdraw")]
        public IActionResult Withdraw([FromBody] DepositDto dto)
        {
            try
            {
                _transactionService.Withdraw(dto.AccountId, dto.Amount);
                return Ok("Withdraw successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferDto transferDto)
        {
            try
            {
                 _transactionService.Transfer(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount);
                return Ok("Transfer successful");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
