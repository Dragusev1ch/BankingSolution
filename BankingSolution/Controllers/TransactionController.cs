using BankingSolution.Dtos.Deposit;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly ILogger<TransactionController> _logger;

        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        private IActionResult ExecuteSafely(Action action, string operationName)
        {
            try
            {
                _logger.LogInformation("Starting operation: {Operation}.", operationName);
                action();
                _logger.LogInformation("Operation {Operation} completed successfully.", operationName);

                return Ok(new { Message = "Operation successful" });
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument during operation: {Operation}.", operationName);
                return BadRequest(new
                {
                    Error = "Invalid argument",
                    Details = ex.Message
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation {Operation} failed due to conflict.", operationName);
                return Conflict(new
                {
                    Error = "Operation failed",
                    Details = ex.Message
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during operation: {Operation}.", operationName);
                return StatusCode(500, new
                {
                    Error = "Internal Server Error",
                    Details = "An unexpected error occurred. Please try again later.",
                    ExceptionMessage = ex.Message
                });
            }
        }

        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Deposit request body is null.");
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null."
                });
            }

            return ExecuteSafely(() => _transactionService.Deposit(dto.AccountId, dto.Amount), "Deposit");
        }

        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Withdraw request body is null.");
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null."
                });
            }

            return ExecuteSafely(() => _transactionService.Withdraw(dto.AccountId, dto.Amount), "Withdraw");
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Transfer request body is null.");
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null."
                });
            }

            return ExecuteSafely(() => _transactionService.Transfer(dto.FromAccountId, dto.ToAccountId, dto.Amount), "Transfer");
        }
    }
}