using BankingSolution.Dtos;
using BankingSolution.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILogger<AccountController> _logger;

        // Constructor initializes the account service and logger dependencies
        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        // Handles the creation of a new account
        [HttpPost]
        public IActionResult Create([FromBody] CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating an account."); // Log invalid input
                return BadRequest(ModelState); // Return validation errors
            }

            _logger.LogInformation("Creating a new account."); // Log operation start
            _accountService.Create(dto); // Call service method to create account

            return Ok(new { Message = "Account created successfully." }); // Return success message
        }

        // Retrieves a specific account by ID
        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            _logger.LogInformation("Retrieving account with ID {Id}.", id); // Log operation start

            var account = _accountService.GetAccountDtoById(id); // Fetch account by ID

            if (account != null)
                return Ok(account); // Return account details if found

            _logger.LogWarning("Account with ID {Id} not found.", id); // Log account not found
            return NotFound(new { Message = "Account not found" }); // Return not found message
        }

        // Retrieves all accounts
        [HttpGet]
        public IActionResult GetAccounts()
        {
            _logger.LogInformation("Retrieving all accounts."); // Log operation start
            return Ok(_accountService.GetAccounts()); // Return all accounts
        }
    }
}
