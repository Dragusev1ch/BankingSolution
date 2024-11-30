using BankingSolution.Dtos.Deposit;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;

        public TransactionController(ITransactionService transactionService)
        {
            _transactionService = transactionService;
        }

        private IActionResult ExecuteSafely(Action action)
        {
            try
            {
                action();
                return Ok(new { Message = "Operation successful" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    Error = "Invalid argument",
                    Details = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new
                {
                    Error = "Operation failed",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = "Internal Server Error",
                    Details = "An unexpected error occurred. Please try again later.",
                    ExceptionMessage = ex.Message // Опціонально: для дебагу
                });
            }
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null."
                });
            }

            return ExecuteSafely(() => _transactionService.Deposit(dto.AccountId, dto.Amount));
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null."
                });
            }

            return ExecuteSafely(() => _transactionService.Withdraw(dto.AccountId, dto.Amount));
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferDto dto)
        {
            if (dto == null)
            {
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null."
                });
            }

            return ExecuteSafely(() =>
                _transactionService.Transfer(dto.FromAccountId, dto.ToAccountId, dto.Amount));
        }
    }
}
