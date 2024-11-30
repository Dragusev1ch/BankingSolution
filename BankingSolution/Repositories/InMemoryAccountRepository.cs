using BankingSolution.Interfaces.Repositories;
using BankingSolution.Models;

namespace BankingSolution.Repositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        private readonly List<Account> _accounts = [];
        private int _nextId = 1;

        public void Add(Account account)
        {
            account.Id = _nextId++;
            _accounts.Add(account);
        }

        public Account? GetById(int id) => _accounts.FirstOrDefault(a => a.Id == id);

        public IEnumerable<Account> GetAll() => _accounts;

        public void Update(Account account)
        {
            var existingAccount = _accounts.
                FirstOrDefault(a => a.Id == account.Id) 
                                  ?? throw new ArgumentException("Account not found", nameof(account));

            existingAccount.Owner = account.Owner;
            existingAccount.Balance = account.Balance;
        }
    }
}
