using BankingAPI.DTOs;
using BankingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AccountRequestDto dto)
        {
            var result = await _accountService.AddAccount(dto);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _accountService.GetAccountById(id);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _accountService.GetAllAccounts();
            return Ok(result);
        }

    
    }
}
