using BankingSolution.Dtos;
using BankingSolution.Dtos.Account;
using BankingSolution.Models;

namespace BankingSolution.Interfaces;

public interface IAccountService
{
    void Create(CreateAccountDto account);
    AccountDto? GetAccountDtoById(int id);
    public Account? GetAccountById(int id);
    IEnumerable<AccountDto> GetAccounts();
}