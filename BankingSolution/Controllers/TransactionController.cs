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
        private IActionResult ExecuteSafely(Action action)
    {
        try
        {
            action();
            return Ok("Operation successful");
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while processing your request.");
        }
    }

    [HttpPost("deposit")]
    public IActionResult Deposit([FromBody] DepositDto dto) =>
        ExecuteSafely(() => _transactionService.Deposit(dto.AccountId, dto.Amount));

    [HttpPost("withdraw")]
    public IActionResult Withdraw([FromBody] WithdrawDto dto) =>
        ExecuteSafely(() => _transactionService.Withdraw(dto.AccountId, dto.Amount));

    [HttpPost("transfer")]
    public IActionResult Transfer([FromBody] TransferDto dto) =>
        ExecuteSafely(() => _transactionService.Transfer(dto.FromAccountId, dto.ToAccountId, dto.Amount));
}
}
