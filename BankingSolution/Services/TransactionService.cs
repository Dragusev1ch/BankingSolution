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
        if (withdraw <= 0)
        {
            throw new ArgumentException("Withdraw must be greater than  zero", nameof(withdraw));
        }
        var account = _accountService.GetAccountById(accountId)
                      ?? throw new ArgumentException("Account not found", nameof(accountId));

        account.Balance -= withdraw;
        return true;
    }

    public bool Transfer(int fromAccountId, int toAccountId, decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than  zero", nameof(amount));
        }

        if (fromAccountId == toAccountId)
        {
            throw new ArgumentException("From and to account cannot be same", nameof(toAccountId));
        }

        var fromAccount = _accountService.GetAccountById(fromAccountId);
        var toAccount = _accountService.GetAccountById(toAccountId);

        if (fromAccount == null || toAccount == null)
        {
            throw new ArgumentException("One of both accounts not found");
        }

        if (fromAccount.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds in from account");  
        }

        fromAccount.Balance -= amount;
        toAccount.Balance += amount;

        return true;
    }
}