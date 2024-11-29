using BankingSolution.Dtos;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Create([FromBody] CreateAccountDto dto)
        {
            if (dto.InitialBalance < 0)
                return BadRequest("Initial balance must be positive.");

            _accountService.Create(dto);

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAccount([FromQuery] int id)
        {
            var account = _accountService.GetAccountDtoById(id);
            
            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpGet]
        [Route("api/accounts")]
        [AllowAnonymous]
        public IActionResult GetAccounts()
        {
            return Ok(_accountService.GetAccounts());
        }
    }
}
