using BankingSolution.Interfaces;
using BankingSolution.Interfaces.Services;
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
        ValidateAmount(deposit);

        var account = GetValidAccount(accountId);
        account.Balance += deposit;

        // Рекомендується викликати метод для оновлення стану акаунту в репозиторії через сервіс акаунтів
        _accountService.UpdateAccount(account);
        return true;
    }

    public bool Withdraw(int accountId, decimal withdraw)
    {
        ValidateAmount(withdraw);

        var account = GetValidAccount(accountId);

        if (account.Balance < withdraw)
        {
            throw new InvalidOperationException("Insufficient funds in your account");
        }

        account.Balance -= withdraw;
        _accountService.UpdateAccount(account);
        return true;
    }

    public bool Transfer(int fromAccountId, int toAccountId, decimal amount)
    {
        ValidateAmount(amount);

        if (fromAccountId == toAccountId)
        {
            throw new ArgumentException("From and to account cannot be the same", nameof(toAccountId));
        }

        var fromAccount = GetValidAccount(fromAccountId);
        var toAccount = GetValidAccount(toAccountId);

        if (fromAccount.Balance < amount)
        {
            throw new InvalidOperationException("Insufficient funds in from account");
        }

        fromAccount.Balance -= amount;
        toAccount.Balance += amount;

        _accountService.UpdateAccount(fromAccount);
        _accountService.UpdateAccount(toAccount);

        return true;
    }

    private static void ValidateAmount(decimal amount)
    {
        if (amount <= 0)
        {
            throw new ArgumentException("Amount must be greater than zero", nameof(amount));
        }
    }

    private Account GetValidAccount(int accountId)
    {
        return _accountService.GetAccountById(accountId)
               ?? throw new ArgumentException("Account not found", nameof(accountId));
    }
}