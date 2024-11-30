using BankingSolution.Dtos;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BankingSolution.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateAccountDto dto)
        {
            if (dto.InitialBalance < 0)
                return BadRequest("Initial balance must be positive.");

            _accountService.Create(dto);

            return Ok();
        }

        [HttpGet("{id}")]
        public IActionResult GetAccount(int id)
        {
            var account = _accountService.GetAccountDtoById(id);
            
            if (account == null)
                return NotFound();

            return Ok(account);
        }

        [HttpGet]
        public IActionResult GetAccounts()
        {
            return Ok(_accountService.GetAccounts());
        }
    }
}
