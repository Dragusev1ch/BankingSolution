using BankingSolution.Dtos;
using BankingSolution.Interfaces;
using BankingSolution.Models;

namespace BankingSolution.Services;

public class AccountService : IAccountService
{
    private static readonly List<Account> Accounts = [];
    private static int _nextId = 1;

    public void Create(CreateAccountDto account)
    {
        if (string.IsNullOrWhiteSpace(account.Owner))
            throw new ArgumentException("Owner name cannot be empty.", nameof(account.Owner));
        if (account.InitialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(account.InitialBalance));

        var newAccount = new Account
        {
            Id = _nextId++,
            Owner = account.Owner,
            Balance = account.InitialBalance
        };

        Accounts.Add(newAccount);
    }

    public AccountDto? Get(int id)
    {
        var account = Accounts.FirstOrDefault(a => a.Id == id);
        
        if (account == null) return null;

        var accountDto = new AccountDto
        {
            Id = account.Id,
            Owner = account.Owner,
            Balance = account.Balance
        };

        return accountDto;
    }

    public IEnumerable<AccountDto> GetAccounts()
    {
        return Accounts.
            Select(account => new AccountDto { Id = account.Id, Owner = account.Owner, 
                Balance = account.Balance }).ToList();
    }
}