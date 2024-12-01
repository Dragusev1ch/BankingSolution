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

        public AccountController(IAccountService accountService, ILogger<AccountController> logger)
        {
            _accountService = accountService;
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAccountDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for creating an account.");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating a new account.");
            _accountService.Create(dto);

            return Ok(new { Message = "Account created successfully." });
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            _logger.LogInformation("Retrieving account with ID {Id}.", id);

            var account = _accountService.GetAccountDtoById(id);

            if (account != null) return Ok(account);
            
            _logger.LogWarning("Account with ID {Id} not found.", id);
            return NotFound(new { Message = "Account not found" });
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            _logger.LogInformation("Retrieving all accounts.");
            return Ok(_accountService.GetAccounts());
        }
    }
}