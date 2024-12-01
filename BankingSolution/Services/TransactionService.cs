using BankingSolution.Interfaces;
using BankingSolution.Interfaces.Services;
using BankingSolution.Models;

namespace BankingSolution.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountService _accountService;

        // Constructor to initialize the account service dependency
        public TransactionService(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // Handles deposit operation by increasing the account balance
        public bool Deposit(int accountId, decimal deposit)
        {
            ValidateAmount(deposit); // Ensure deposit amount is valid

            var account = GetValidAccount(accountId); // Retrieve and validate account
            account.Balance += deposit;

            // Update account state in the repository through the account service
            _accountService.UpdateAccount(account);
            return true;
        }

        // Handles withdraw operation by decreasing the account balance
        public bool Withdraw(int accountId, decimal withdraw)
        {
            ValidateAmount(withdraw); // Ensure withdrawal amount is valid

            var account = GetValidAccount(accountId); // Retrieve and validate account

            if (account.Balance < withdraw)
            {
                throw new InvalidOperationException("Insufficient funds in your account"); // Check for sufficient balance
            }

            account.Balance -= withdraw;
            _accountService.UpdateAccount(account); // Update the account with the new balance
            return true;
        }

        // Handles fund transfer between two accounts
        public bool Transfer(int fromAccountId, int toAccountId, decimal amount)
        {
            ValidateAmount(amount); // Ensure transfer amount is valid

            if (fromAccountId == toAccountId)
            {
                throw new ArgumentException("From and to account cannot be the same", nameof(toAccountId)); // Prevent self-transfer
            }

            var fromAccount = GetValidAccount(fromAccountId); // Validate sender's account
            var toAccount = GetValidAccount(toAccountId);     // Validate receiver's account

            if (fromAccount.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds in from account"); // Check for sufficient balance
            }

            fromAccount.Balance -= amount;
            toAccount.Balance += amount;

            // Update both accounts in the repository
            _accountService.UpdateAccount(fromAccount);
            _accountService.UpdateAccount(toAccount);

            return true;
        }

        // Validates the provided transaction amount
        private static void ValidateAmount(decimal amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Amount must be greater than zero", nameof(amount));
            }
        }

        // Retrieves an account by ID and ensures it exists
        private Account GetValidAccount(int accountId)
        {
            return _accountService.GetAccountById(accountId)
                   ?? throw new ArgumentException("Account not found", nameof(accountId));
        }
    }
}
