using BankingSolution.Controllers;
using BankingSolution.Dtos.Deposit;
using BankingSolution.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace BankingSolution.Tests.ControllersTests
{
    public class TransactionsControllerTests
    {
        private readonly Mock<ITransactionService> _transactionServiceMock;
        private readonly Mock<ILogger<TransactionController>> _loggerMock;
        private readonly TransactionController _controller;

        public TransactionsControllerTests()
        {
            _transactionServiceMock = new Mock<ITransactionService>();
            _loggerMock = new Mock<ILogger<TransactionController>>();
            _controller = new TransactionController(_transactionServiceMock.Object, _loggerMock.Object);
        }

        [Fact]
        public void Deposit_ShouldReturnBadRequest_WhenDtoIsNull()
        {
            // Act
            var result = _controller.Deposit(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            // Перетворення значення у формат JSON і десеріалізація для перевірки
            var resultValue = System.Text.Json.JsonSerializer.Serialize(badRequestResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Invalid input", deserializedResult["Error"]);
            Assert.Equal("The request body cannot be null.", deserializedResult["Details"]);
        }

        [Fact]
        public void Deposit_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
        {
            // Arrange
            var depositDto = new DepositDto { AccountId = 1, Amount = 500 };
            _transactionServiceMock
                .Setup(s => s.Deposit(depositDto.AccountId, depositDto.Amount))
                .Throws(new ArgumentException("Invalid account ID"));

            // Act
            var result = _controller.Deposit(depositDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(badRequestResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Invalid argument", deserializedResult["Error"]);
            Assert.Equal("Invalid account ID", deserializedResult["Details"]);
        }

        [Fact]
        public void Deposit_ShouldReturnConflict_WhenInvalidOperationExceptionOccurs()
        {
            // Arrange
            var depositDto = new DepositDto { AccountId = 1, Amount = 500 };
            _transactionServiceMock
                .Setup(s => s.Deposit(depositDto.AccountId, depositDto.Amount))
                .Throws(new InvalidOperationException("Account is locked"));

            // Act
            var result = _controller.Deposit(depositDto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.NotNull(conflictResult);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(conflictResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Operation failed", deserializedResult["Error"]);
            Assert.Equal("Account is locked", deserializedResult["Details"]);
        }

        [Fact]
        public void Deposit_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var depositDto = new DepositDto { AccountId = 1, Amount = 500 };
            _transactionServiceMock
                .Setup(s => s.Deposit(depositDto.AccountId, depositDto.Amount))
                .Throws(new Exception("Database error"));

            // Act
            var result = _controller.Deposit(depositDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var errorResult = result as ObjectResult;
            Assert.NotNull(errorResult);
            Assert.Equal(500, errorResult.StatusCode);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(errorResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Internal Server Error", deserializedResult["Error"]);
            Assert.Equal("An unexpected error occurred. Please try again later.", deserializedResult["Details"]);
            Assert.Equal("Database error", deserializedResult["ExceptionMessage"]);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenArgumentExceptionOccurs()
        {
            // Arrange
            var withdrawDto = new WithdrawDto { AccountId = 2, Amount = 200 };
            _transactionServiceMock
                .Setup(s => s.Withdraw(withdrawDto.AccountId, withdrawDto.Amount))
                .Throws(new ArgumentException("Invalid account ID"));

            // Act
            var result = _controller.Withdraw(withdrawDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(badRequestResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Invalid argument", deserializedResult["Error"]);
            Assert.Equal("Invalid account ID", deserializedResult["Details"]);
        }

        [Fact]
        public void Withdraw_ShouldReturnConflict_WhenInvalidOperationExceptionOccurs()
        {
            // Arrange
            var withdrawDto = new WithdrawDto { AccountId = 2, Amount = 200 };
            _transactionServiceMock
                .Setup(s => s.Withdraw(withdrawDto.AccountId, withdrawDto.Amount))
                .Throws(new InvalidOperationException("Insufficient funds"));

            // Act
            var result = _controller.Withdraw(withdrawDto);

            // Assert
            Assert.IsType<ConflictObjectResult>(result);
            var conflictResult = result as ConflictObjectResult;
            Assert.NotNull(conflictResult);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(conflictResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Operation failed", deserializedResult["Error"]);
            Assert.Equal("Insufficient funds", deserializedResult["Details"]);
        }

        [Fact]
        public void Withdraw_ShouldReturnInternalServerError_WhenUnexpectedExceptionOccurs()
        {
            // Arrange
            var withdrawDto = new WithdrawDto { AccountId = 2, Amount = 200 };
            _transactionServiceMock
                .Setup(s => s.Withdraw(withdrawDto.AccountId, withdrawDto.Amount))
                .Throws(new Exception("Unexpected error"));

            // Act
            var result = _controller.Withdraw(withdrawDto);

            // Assert
            Assert.IsType<ObjectResult>(result);
            var errorResult = result as ObjectResult;
            Assert.NotNull(errorResult);
            Assert.Equal(500, errorResult.StatusCode);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(errorResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Internal Server Error", deserializedResult["Error"]);
            Assert.Equal("An unexpected error occurred. Please try again later.", deserializedResult["Details"]);
            Assert.Equal("Unexpected error", deserializedResult["ExceptionMessage"]);
        }

        [Fact]
        public void Withdraw_ShouldReturnBadRequest_WhenDtoIsNull()
        {
            // Act
            var result = _controller.Withdraw(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(badRequestResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Invalid input", deserializedResult["Error"]);
            Assert.Equal("The request body cannot be null.", deserializedResult["Details"]);
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

            var resultValue = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Operation successful", deserializedResult["Message"]);

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

            var resultValue = System.Text.Json.JsonSerializer.Serialize(okResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Operation successful", deserializedResult["Message"]);

            _transactionServiceMock.Verify(
                s => s.Transfer(transferDto.FromAccountId, transferDto.ToAccountId, transferDto.Amount),
                Times.Once
            );
        }

        [Fact]
        public void Transfer_ShouldReturnBadRequest_WhenDtoIsNull()
        {
            // Act
            var result = _controller.Transfer(null);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);

            var resultValue = System.Text.Json.JsonSerializer.Serialize(badRequestResult.Value);
            var deserializedResult = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(resultValue);

            Assert.NotNull(deserializedResult);
            Assert.Equal("Invalid input", deserializedResult["Error"]);
            Assert.Equal("The request body cannot be null.", deserializedResult["Details"]);
        }

    }
}
