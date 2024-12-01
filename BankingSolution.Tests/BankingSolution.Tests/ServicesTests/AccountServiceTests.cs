using BankingSolution.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BankingSolution.Interfaces.Repositories;
using BankingSolution.Services;
using BankingSolution.Interfaces.Services;
using Moq;
using BankingSolution.Models;

namespace BankingSolution.Tests.ServicesTests
{
    public class AccountServiceTests
    {
        private readonly IAccountService _accountService;
        private readonly Mock<IAccountRepository> _mockRepository;

        public AccountServiceTests()
        {
            _mockRepository = new Mock<IAccountRepository>();

            _accountService = new AccountService(_mockRepository.Object);
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
            _mockRepository.Verify(repo => repo.Add(It.Is<Account>(a =>
                a.Owner == "John Doe" && a.Balance == 500)), Times.Once);

            // Можна додати додаткові перевірки
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
            var account = new Account { Id = 1, Owner = "John Doe", Balance = 500 };
            _mockRepository.Setup(repo => repo.GetById(1)).Returns(account);

            // Act
            var accountDto = _accountService.GetAccountDtoById(1);

            // Assert
            Assert.NotNull(accountDto);
            Assert.Equal("John Doe", accountDto.Owner);
            Assert.Equal(500, accountDto.Balance);
        }

        [Fact]
        public void GetAccountDtoById_ShouldReturnNull_WhenAccountDoesNotExist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetById(It.IsAny<int>())).Returns((Account)null);

            // Act
            var accountDto = _accountService.GetAccountDtoById(999);

            // Assert
            Assert.Null(accountDto);
        }

        [Fact]
        public void GetAccounts_ShouldReturnAllAccounts()
        {
            // Arrange
            var accounts = new List<Account>
            {
                new Account { Id = 1, Owner = "John Doe", Balance = 500 },
                new Account { Id = 2, Owner = "Jane Smith", Balance = 1000 }
            };
            _mockRepository.Setup(repo => repo.GetAll()).Returns(accounts);

            // Act
            var result = _accountService.GetAccounts();

            // Assert
            Assert.Equal(2, result.Count());
            Assert.Equal("John Doe", result.First().Owner);
            Assert.Equal("Jane Smith", result.Last().Owner);
        }

        [Fact]
        public void UpdateAccount_ShouldCallRepositoryUpdate_WhenAccountIsValid()
        {
            // Arrange
            var account = new Account { Id = 1, Owner = "John Doe", Balance = 500 };

            // Act
            _accountService.UpdateAccount(account);

            // Assert
            _mockRepository.Verify(repo => repo.Update(It.Is<Account>(a =>
                a.Id == 1 && a.Owner == "John Doe" && a.Balance == 500)), Times.Once);
        }

        [Fact]
        public void UpdateAccount_ShouldThrowException_WhenAccountIsNull()
        {
            // Arrange
            Account account = null;

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => _accountService.UpdateAccount(account));
            Assert.Equal("Value cannot be null. (Parameter 'account')", exception.Message);

            // Переконаємось, що метод репозиторію не викликався
            _mockRepository.Verify(repo => repo.Update(It.IsAny<Account>()), Times.Never);
        }

    }
}