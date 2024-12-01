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

        // Constructor initializes the transaction service and logger dependencies
        public TransactionController(ITransactionService transactionService, ILogger<TransactionController> logger)
        {
            _transactionService = transactionService;
            _logger = logger;
        }

        // Centralized method to execute actions safely and handle exceptions
        private IActionResult ExecuteSafely(Action action, string operationName)
        {
            try
            {
                _logger.LogInformation("Starting operation: {Operation}.", operationName); // Log operation start
                action(); // Execute the action
                _logger.LogInformation("Operation {Operation} completed successfully.", operationName); // Log success

                return Ok(new { Message = "Operation successful" }); // Return success response
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument during operation: {Operation}.", operationName); // Log argument error
                return BadRequest(new
                {
                    Error = "Invalid argument",
                    Details = ex.Message // Provide error details
                });
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogWarning(ex, "Operation {Operation} failed due to conflict.", operationName); // Log conflict error
                return Conflict(new
                {
                    Error = "Operation failed",
                    Details = ex.Message // Provide conflict details
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error during operation: {Operation}.", operationName); // Log unexpected error
                return StatusCode(500, new
                {
                    Error = "Internal Server Error",
                    Details = "An unexpected error occurred. Please try again later.", // Provide generic error message
                    ExceptionMessage = ex.Message // Include detailed exception message
                });
            }
        }

        // Handles deposit requests by invoking the transaction service's deposit method
        [HttpPost("deposit")]
        public IActionResult Deposit([FromBody] DepositDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Deposit request body is null."); // Log invalid input
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null." // Provide input validation message
                });
            }

            return ExecuteSafely(() => _transactionService.Deposit(dto.AccountId, dto.Amount), "Deposit");
        }

        // Handles withdrawal requests by invoking the transaction service's withdraw method
        [HttpPost("withdraw")]
        public IActionResult Withdraw([FromBody] WithdrawDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Withdraw request body is null."); // Log invalid input
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null." // Provide input validation message
                });
            }

            return ExecuteSafely(() => _transactionService.Withdraw(dto.AccountId, dto.Amount), "Withdraw");
        }

        // Handles transfer requests by invoking the transaction service's transfer method
        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferDto dto)
        {
            if (dto == null)
            {
                _logger.LogWarning("Transfer request body is null."); // Log invalid input
                return BadRequest(new
                {
                    Error = "Invalid input",
                    Details = "The request body cannot be null." // Provide input validation message
                });
            }

            return ExecuteSafely(() => _transactionService.Transfer(dto.FromAccountId, dto.ToAccountId, dto.Amount), "Transfer");
        }
    }
}
