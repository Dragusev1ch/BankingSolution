using BankingSolution.Dtos;

namespace BankingSolution.Interfaces;

public interface IAccountService
{
    void Create(CreateAccountDto account);
    AccountDto? Get(int id);
    IEnumerable<AccountDto> GetAccounts();
}