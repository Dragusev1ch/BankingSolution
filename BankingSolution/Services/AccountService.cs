using BankingSolution.Dtos;
using BankingSolution.Dtos.Account;
using BankingSolution.Interfaces.Repositories;
using BankingSolution.Interfaces.Services;
using BankingSolution.Models;

namespace BankingSolution.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;

    public AccountService(IAccountRepository repository)
    {
        _repository = repository;
    }

    public void Create(CreateAccountDto account)
    {
        ValidateAccount(account);

        var newAccount = new Account
        {
            Owner = account.Owner,
            Balance = account.InitialBalance
        };

        _repository.Add(newAccount);
    }

    public AccountDto? GetAccountDtoById(int id)
    {
        var account = _repository.GetById(id);
        return account == null ? null : MapToDto(account);
    }

    public IEnumerable<AccountDto> GetAccounts()
    {
        return _repository.GetAll().Select(MapToDto);
    }

    public Account? GetAccountById(int id)
    {
        return _repository.GetById(id);
    }
    public void UpdateAccount(Account account)
    {
        if (account == null)
            throw new ArgumentNullException(nameof(account));

        _repository.Update(account); // Виклик методу репозиторію для оновлення
    }

    private static AccountDto MapToDto(Account account)
    {
        return new AccountDto
        {
            Id = account.Id,
            Owner = account.Owner,
            Balance = account.Balance
        };
    }

    private static void ValidateAccount(CreateAccountDto account)
    {
        if (string.IsNullOrWhiteSpace(account.Owner))
            throw new ArgumentException("Owner name cannot be empty.", nameof(account.Owner));
        if (account.InitialBalance < 0)
            throw new ArgumentException("Initial balance cannot be negative.", nameof(account.InitialBalance));
    }
}