using BankingSolution.Dtos;
using BankingSolution.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSolution.Services;

namespace BankingSolution.Tests.ServicesTests
{
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;

        public AccountServiceTests()
        {
            _accountService = new AccountService();
        }

        [Fact]
        public void Create_ShouldAddNewAccount_WhenValidDataProvided()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                Owner = "John Doe",
                InitialBalance = 500
            };

            // Act
            _accountService.Create(createAccountDto);

            // Assert
            var accounts = _accountService.GetAccounts();
            Assert.Single(accounts); // Перевіряємо, що створено 1 акаунт
            var account = accounts.First();
            Assert.Equal("John Doe", account.Owner); // Ім'я власника збігається
            Assert.Equal(500, account.Balance); // Баланс збігається
        }

        [Fact]
        public void Create_ShouldThrowException_WhenOwnerIsEmpty()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                Owner = "",
                InitialBalance = 500
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _accountService.Create(createAccountDto));
            Assert.Equal("Owner name cannot be empty. (Parameter 'Owner')", exception.Message);
        }

        [Fact]
        public void Create_ShouldThrowException_WhenInitialBalanceIsNegative()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                Owner = "John Doe",
                InitialBalance = -100
            };

            // Act & Assert
            var exception = Assert.Throws<ArgumentException>(() => _accountService.Create(createAccountDto));
            Assert.Equal("Initial balance cannot be negative. (Parameter 'InitialBalance')", exception.Message);
        }

        [Fact]
        public void GetAccountDtoById_ShouldReturnAccountDto_WhenAccountExists()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                Owner = "John Doe",
                InitialBalance = 500
            };
            _accountService.Create(createAccountDto);

            // Act
            var accountDto = _accountService.GetAccountDtoById(1);

            // Assert
            Assert.NotNull(accountDto); // Перевіряємо, що об'єкт не null
            Assert.Equal("John Doe", accountDto.Owner); // Ім'я власника збігається
            Assert.Equal(500, accountDto.Balance); // Баланс збігається
        }

        [Fact]
        public void GetAccountDtoById_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Act
            var accountDto = _accountService.GetAccountDtoById(999);

            // Assert
            Assert.Null(accountDto); // Перевіряємо, що об'єкт null
        }

        [Fact]
        public void GetAccountById_ShouldReturnAccount_WhenAccountExists()
        {
            // Arrange
            var createAccountDto = new CreateAccountDto
            {
                Owner = "Jane Smith",
                InitialBalance = 1000
            };
            _accountService.Create(createAccountDto);

            // Act
            var account = _accountService.GetAccountById(1);

            // Assert
            Assert.NotNull(account); // Перевіряємо, що об'єкт не null
            Assert.Equal("Jane Smith", account.Owner); // Ім'я власника збігається
            Assert.Equal(1000, account.Balance); // Баланс збігається
        }

        [Fact]
        public void GetAccountById_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Act
            var account = _accountService.GetAccountById(999);

            // Assert
            Assert.Null(account); // Перевіряємо, що об'єкт null
        }

        [Fact]
        public void GetAccounts_ShouldReturnAllAccounts()
        {
            // Arrange
            var accountsToCreate = new[]
            {
            new CreateAccountDto { Owner = "John Doe", InitialBalance = 500 },
            new CreateAccountDto { Owner = "Jane Smith", InitialBalance = 1000 }
        };

            foreach (var dto in accountsToCreate)
            {
                _accountService.Create(dto);
            }

            // Act
            var accounts = _accountService.GetAccounts();

            // Assert
            Assert.Equal(2, accounts.Count()); // Перевіряємо кількість акаунтів
            var firstAccount = accounts.First();
            var lastAccount = accounts.Last();
            Assert.Equal("John Doe", firstAccount.Owner); // Перший акаунт
            Assert.Equal("Jane Smith", lastAccount.Owner); // Другий акаунт
        }
    }
}
