using BankingSolution.Dtos;

namespace BankingSolution.Interfaces;

public interface ITransactionService
{
    bool Deposit(int accountId, decimal deposit);
    bool Withdraw(int accountId, decimal withdraw);
    bool Transfer(int fromAccountId, int toAccountId, decimal amount);
}