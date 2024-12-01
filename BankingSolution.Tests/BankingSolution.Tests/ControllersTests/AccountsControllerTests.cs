using BankingSolution.Dtos.Account;
using BankingSolution.Dtos;
using Microsoft.AspNetCore.Mvc;
using Moq;
using BankingSolution.Controllers;
using BankingSolution.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace BankingSolution.Tests.ControllersTests;

public class AccountsControllerTests
{
    private readonly Mock<IAccountService> _accountServiceMock;
    private readonly Mock<ILogger<AccountController>> _loggerMock;
    private readonly AccountController _controller;

    public AccountsControllerTests()
    {
        _accountServiceMock = new Mock<IAccountService>();
        _loggerMock = new Mock<ILogger<AccountController>>();
        _controller = new AccountController(_accountServiceMock.Object, _loggerMock.Object);
    }

    [Fact]
    public void Create_ShouldReturnOk_WhenValidDtoProvided()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            Owner = "John Doe",
            InitialBalance = 500
        };

        // Act
        var result = _controller.Create(createAccountDto);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        _accountServiceMock.Verify(s => s.Create(createAccountDto), Times.Once);
    }

    [Fact]
    public void Create_ShouldReturnBadRequest_WhenInitialBalanceIsNegative()
    {
        // Arrange
        var createAccountDto = new CreateAccountDto
        {
            Owner = "John Doe",
            InitialBalance = -100
        };

        var controller = new AccountController(_accountServiceMock.Object, _loggerMock.Object);
        controller.ModelState.AddModelError("InitialBalance", "Initial balance must be a positive value");

        // Act
        var result = controller.Create(createAccountDto);

        // Assert
        Assert.IsType<BadRequestObjectResult>(result);

        var badRequest = result as BadRequestObjectResult;
        Assert.NotNull(badRequest);
        Assert.IsAssignableFrom<SerializableError>(badRequest.Value);

        var errors = badRequest.Value as SerializableError;
        Assert.Contains("InitialBalance", errors.Keys);
        Assert.Contains("Initial balance must be a positive value", errors["InitialBalance"] as string[]);

        _accountServiceMock.Verify(s => s.Create(It.IsAny<CreateAccountDto>()), Times.Never);
    }

    [Fact]
    public void GetAccount_ShouldReturnOk_WhenAccountExists()
    {
        // Arrange
        var accountId = 1;
        var accountDto = new AccountDto
        {
            Id = accountId,
            Owner = "John Doe",
            Balance = 500
        };

        _accountServiceMock
            .Setup(s => s.GetAccountDtoById(accountId))
            .Returns(accountDto);

        // Act
        var result = _controller.GetAccount(accountId);

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(accountDto, okResult.Value);
        _accountServiceMock.Verify(s => s.GetAccountDtoById(accountId), Times.Once);
    }

    [Fact]
    public void GetAccount_ShouldReturnNotFound_WhenAccountDoesNotExist()
    {
        // Arrange
        var accountId = 1;

        _accountServiceMock
        .Setup(s => s.GetAccountDtoById(accountId))
            .Returns((AccountDto?)null);

        // Act
        var result = _controller.GetAccount(accountId);

        // Assert
        Assert.IsType<NotFoundObjectResult>(result);
        _accountServiceMock.Verify(s => s.GetAccountDtoById(accountId), Times.Once);
    }

    [Fact]
    public void GetAccounts_ShouldReturnOk_WithListOfAccounts()
    {
        // Arrange
        var accounts = new List<AccountDto>
        {
            new AccountDto { Id = 1, Owner = "John Doe", Balance = 500 },
            new AccountDto { Id = 2, Owner = "Jane Doe", Balance = 1000 }
        };

        _accountServiceMock
            .Setup(s => s.GetAccounts())
            .Returns(accounts);

        // Act
        var result = _controller.GetAccounts();

        // Assert
        Assert.IsType<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.NotNull(okResult);
        Assert.Equal(accounts, okResult.Value);
        _accountServiceMock.Verify(s => s.GetAccounts(), Times.Once);
    }
}
