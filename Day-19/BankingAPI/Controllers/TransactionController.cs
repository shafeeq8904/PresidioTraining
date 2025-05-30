using BankingAPI.DTOs;
using BankingAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BankingAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionLogService _transactionService;

        public TransactionController(ITransactionLogService transactionService)
        {
            _transactionService = transactionService;
        }

        [HttpPost("deposit")]
        public async Task<IActionResult> Deposit(TransactionLogRequestDto dto)
        {
            var result = await _transactionService.Deposit(dto);
            return Ok(result);
        }

        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw(TransactionLogRequestDto dto)
        {
            var result = await _transactionService.Withdraw(dto);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _transactionService.GetAllTransactions();
            return Ok(result);
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetByAccount(int accountId)
        {
            var result = await _transactionService.GetTransactionsForAccount(accountId);
            return Ok(result);
        }

    }
}
