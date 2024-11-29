using BankingSolution.Interfaces;

namespace BankingSolution.Services;

public class TransactionService : ITransactionService
{
    public bool Deposit(int accountId, decimal deposit)
    {
        throw new NotImplementedException();
    }

    public bool Withdraw(int accountId, decimal withdraw)
    {
        throw new NotImplementedException();
    }

    public bool Transfer(int fromAccountId, int toAccountId, decimal amount)
    {
        throw new NotImplementedException();
    }
}