using BankingSolution.Interfaces;
using BankingSolution.Models;

namespace BankingSolution.Services;

public class TransactionService : ITransactionService
{
    private readonly IAccountService _accountService;

    public TransactionService(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public bool Deposit(int accountId, decimal deposit)
    {
        if (deposit <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero", nameof(deposit));
        }

        var account = _accountService.GetAccountById(accountId) 
                      ?? throw new ArgumentException("Account not found", nameof(accountId));
        
        account.Balance += deposit;
        return true;
    }

    public bool Withdraw(int accountId, decimal withdraw)
    {
        throw new NotImplementedException();
    }

    public bool Transfer(int fromAccountId, int toAccountId, decimal amount)
    {
        throw new NotImplementedException();
    }
}