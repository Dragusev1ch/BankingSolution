using BankingSolution.Dtos;
using BankingSolution.Dtos.Account;
using BankingSolution.Interfaces.Repositories;
using BankingSolution.Interfaces.Services;
using BankingSolution.Models;

namespace BankingSolution.Services
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _repository;

        // Constructor to initialize the repository dependency
        public AccountService(IAccountRepository repository)
        {
            _repository = repository;
        }

        // Creates a new account and adds it to the repository
        public void Create(CreateAccountDto account)
        {
            ValidateAccount(account); // Validate the input data

            var newAccount = new Account
            {
                Owner = account.Owner,
                Balance = account.InitialBalance
            };

            _repository.Add(newAccount); // Save the new account to the repository
        }

        // Retrieves account details by ID and maps it to AccountDto
        public AccountDto? GetAccountDtoById(int id)
        {
            var account = _repository.GetById(id);
            return account == null ? null : MapToDto(account);
        }

        // Retrieves all accounts and maps them to AccountDto
        public IEnumerable<AccountDto> GetAccounts()
        {
            return _repository.GetAll().Select(MapToDto);
        }

        // Retrieves an account model by ID (returns null if not found)
        public Account? GetAccountById(int id)
        {
            return _repository.GetById(id);
        }

        // Updates an existing account in the repository
        public void UpdateAccount(Account account)
        {
            if (account == null)
                throw new ArgumentNullException(nameof(account));

            _repository.Update(account); // Calls repository method to update the account
        }

        // Helper method to map Account model to AccountDto
        private static AccountDto MapToDto(Account account)
        {
            return new AccountDto
            {
                Id = account.Id,
                Owner = account.Owner,
                Balance = account.Balance
            };
        }

        // Validates the account data before creation
        private static void ValidateAccount(CreateAccountDto account)
        {
            if (string.IsNullOrWhiteSpace(account.Owner))
                throw new ArgumentException("Owner name cannot be empty.", nameof(account.Owner));
            if (account.InitialBalance < 0)
                throw new ArgumentException("Initial balance cannot be negative.", nameof(account.InitialBalance));
        }
    }
}