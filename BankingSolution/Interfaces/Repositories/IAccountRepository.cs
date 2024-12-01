using BankingSolution.Models;

namespace BankingSolution.Interfaces.Repositories;

public interface IAccountRepository
{
    void Add(Account account);
    Account? GetById(int id);
    IEnumerable<Account> GetAll();
    void Update(Account account);
}