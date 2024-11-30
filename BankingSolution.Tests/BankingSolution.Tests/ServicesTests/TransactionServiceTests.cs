using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSolution.Interfaces;
using BankingSolution.Models;
using BankingSolution.Services;
using Moq;

namespace BankingSolution.Tests.ServicesTests
{
    public class TransactionServiceTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly ITransactionService _transactionService;

        public TransactionServiceTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _transactionService = new TransactionService(_accountServiceMock.Object);
        }

        [Fact]
        public void Deposit_ShouldIncreaseBalance_WhenValidDataProvided()
        {
            // Arrange
            var accountId = 1;
            var depositAmount = 500m;
            var account = new Account { Id = accountId, Balance = 1000m };

            _accountServiceMock.Setup(s => s.GetAccountById(accountId)).Returns(account);

            // Act
            var result = _transactionService.Deposit(accountId, depositAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(1500m, account.Balance); // Баланс має збільшитись на 500
        }

        [Fact]
        public void Deposit_ShouldThrowException_WhenAmountIsZeroOrNegative()
        {
            // Arrange
            var accountId = 1;
            var depositAmount = -100m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Deposit(accountId, depositAmount));
            Assert.Equal("Amount must be greater than zero (Parameter 'deposit')", exception.Message);
        }

        [Fact]
        public void Deposit_ShouldThrowException_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            var depositAmount = 500m;

            _accountServiceMock.Setup(s => s.GetAccountById(accountId)).Returns((Account)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Deposit(accountId, depositAmount));
            Assert.Equal("Account not found (Parameter 'accountId')", exception.Message);
        }

        [Fact]
        public void Withdraw_ShouldDecreaseBalance_WhenValidDataProvided()
        {
            // Arrange
            var accountId = 1;
            var withdrawAmount = 200m;
            var account = new Account { Id = accountId, Balance = 1000m };

            _accountServiceMock.Setup(s => s.GetAccountById(accountId)).Returns(account);

            // Act
            var result = _transactionService.Withdraw(accountId, withdrawAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(800m, account.Balance); // Баланс має зменшитись на 200
        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenAmountIsZeroOrNegative()
        {
            // Arrange
            var accountId = 1;
            var withdrawAmount = -100m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Withdraw(accountId, withdrawAmount));
            Assert.Equal("Withdraw must be greater than zero (Parameter 'withdraw')", exception.Message);
        }

        [Fact]
        public void Withdraw_ShouldThrowException_WhenAccountNotFound()
        {
            // Arrange
            var accountId = 1;
            var withdrawAmount = 500m;

            _accountServiceMock.Setup(s => s.GetAccountById(accountId)).Returns((Account)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Withdraw(accountId, withdrawAmount));
            Assert.Equal("Account not found (Parameter 'accountId')", exception.Message);
        }

        [Fact]
        public void Transfer_ShouldMoveFundsBetweenAccounts_WhenValidDataProvided()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var transferAmount = 300m;
            var fromAccount = new Account { Id = fromAccountId, Balance = 1000m };
            var toAccount = new Account { Id = toAccountId, Balance = 500m };

            _accountServiceMock.Setup(s => s.GetAccountById(fromAccountId)).Returns(fromAccount);
            _accountServiceMock.Setup(s => s.GetAccountById(toAccountId)).Returns(toAccount);

            // Act
            var result = _transactionService.Transfer(fromAccountId, toAccountId, transferAmount);

            // Assert
            Assert.True(result);
            Assert.Equal(700m, fromAccount.Balance); // Баланс відправника зменшено
            Assert.Equal(800m, toAccount.Balance); // Баланс отримувача збільшено
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenAmountIsZeroOrNegative()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var transferAmount = -300m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Transfer(fromAccountId, toAccountId, transferAmount));
            Assert.Equal("Amount must be greater than zero (Parameter 'amount')", exception.Message);
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenFromAndToAccountsAreSame()
        {
            // Arrange
            var accountId = 1;
            var transferAmount = 300m;

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Transfer(accountId, accountId, transferAmount));
            Assert.Equal("From and to account cannot be same (Parameter 'toAccountId')", exception.Message);
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenOneOrBothAccountsNotFound()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var transferAmount = 300m;

            _accountServiceMock.Setup(s => s.GetAccountById(fromAccountId)).Returns((Account)null);

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _transactionService.Transfer(fromAccountId, toAccountId, transferAmount));
            Assert.Equal("One of both accounts not found", exception.Message);
        }

        [Fact]
        public void Transfer_ShouldThrowException_WhenInsufficientFunds()
        {
            // Arrange
            var fromAccountId = 1;
            var toAccountId = 2;
            var transferAmount = 300m;
            var fromAccount = new Account { Id = fromAccountId, Balance = 200m };
            var toAccount = new Account { Id = toAccountId, Balance = 500m };

            _accountServiceMock.Setup(s => s.GetAccountById(fromAccountId)).Returns(fromAccount);
            _accountServiceMock.Setup(s => s.GetAccountById(toAccountId)).Returns(toAccount);

            // Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => _transactionService.Transfer(fromAccountId, toAccountId, transferAmount));
            Assert.Equal("Insufficient funds in from account", exception.Message);
        }
    }
}
