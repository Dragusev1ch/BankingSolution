using BankingSolution.Interfaces.Repositories;
using BankingSolution.Models;

namespace BankingSolution.Repositories
{
    public class InMemoryAccountRepository : IAccountRepository
    {
        // List to store accounts in memory
        private readonly List<Account> _accounts = new();
        private int _nextId = 1; // Auto-incremented ID for new accounts

        // Adds a new account to the in-memory repository
        public void Add(Account account)
        {
            account.Id = _nextId++; // Assign a unique ID
            _accounts.Add(account); // Add the account to the list
        }

        // Retrieves an account by its ID, or null if not found
        public Account? GetById(int id) => _accounts.FirstOrDefault(a => a.Id == id);

        // Retrieves all accounts
        public IEnumerable<Account> GetAll() => _accounts;

        // Updates an existing account's details
        public void Update(Account account)
        {
            var existingAccount = _accounts
                                      .FirstOrDefault(a => a.Id == account.Id)
                                  ?? throw new ArgumentException("Account not found", nameof(account)); // Throws if account not found

            // Update account details
            existingAccount.Owner = account.Owner;
            existingAccount.Balance = account.Balance;
        }
    }
}