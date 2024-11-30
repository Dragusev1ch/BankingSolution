using BankingSolution.Controllers;
using BankingSolution.Dtos.Deposit;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace BankingSolution.Tests.ControllersTests
{
    public class TransactionsControllerTests
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly TransactionController _controller;

        public TransactionsControllerTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _controller = new TransactionController(_transactionServiceMock.Object);
        }

        [Fact]
        public void Deposit_ShouldReturnOk_WhenDepositIsSuccessful()
        {
            // Arrange
            var depositDto = new DepositDto { AccountId = 1, Amount = 500 };

            // Act
            var result = _controller.Deposit(depositDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal("Operation successful", okResult.Value);
            _transactionServiceMock.Verify(s => s.Deposit(depositDto.AccountId, depositDto.Amount), Times.Once);
        }

        [Fact]
        public void Deposit_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var depositDto = new DepositDto { AccountId = 1, Amount = 500 };
            _transactionServiceMock
                .Setup(s => s.Deposit(depositDto.AccountId, depositDto.Amount))
                .Throws(new Exception("An error occurred while processing your request."));

            // Act
            var result = _controller.Deposit(depositDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value);
            _transactionServiceMock.Verify(s => s.Deposit(depositDto.AccountId, depositDto.Amount), Times.Once);
        }

        [Fact]
        public void Withdraw_ShouldReturnOk_WhenWithdrawIsSuccessful()
        {
            // Arrange
            var withdrawDto = new WithdrawDto { AccountId = 2, Amount = 200 };

            // Act
            var result = _controller.Withdraw(withdrawDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal("Operation successful", okResult.Value);
            _transactionServiceMock.Verify(s => s.Withdraw(withdrawDto.AccountId, withdrawDto.Amount), Times.Once);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var withdrawDto = new WithdrawDto { AccountId = 2, Amount = 200 };
            _transactionServiceMock
                .Setup(s => s.Withdraw(withdrawDto.AccountId, withdrawDto.Amount))
                .Throws(new Exception("An error occurred while processing your request.")); // Загальне повідомлення

            // Act
            var result = _controller.Withdraw(withdrawDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value); // Оновлена перевірка
            _transactionServiceMock.Verify(s => s.Withdraw(withdrawDto.AccountId, withdrawDto.Amount), Times.Once);
        }

        [Fact]
        public void Transfer_ShouldReturnOk_WhenTransferIsSuccessful()
        {
            // Arrange
            var transferDto = new TransferDto
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 300
            };

            // Act
            var result = _controller.Transfer(transferDto);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.NotNull(okResult);
            Assert.Equal("Operation successful", okResult.Value);
            _transactionServiceMock.Verify(s => s.Transfer(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount), Times.Once);
        }

        [Fact]
        public void Transfer_ShouldReturnBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var transferDto = new TransferDto
            {
                FromAccountId = 1,
                ToAccountId = 2,
                Amount = 300
            };
            _transactionServiceMock
                .Setup(s => s.Transfer(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount))
                .Throws(new Exception("Invalid transfer amount")); // Виняток із будь-яким повідомленням

            // Act
            var result = _controller.Transfer(transferDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal("An error occurred while processing your request.", badRequestResult.Value); // Оновлена перевірка
            _transactionServiceMock.Verify(s => s.Transfer(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount), Times.Once);
        }
    }
}
