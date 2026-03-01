using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MySecureBackend.WebApi.Controllers;
using MySecureBackend.WebApi.Models;
using MySecureBackend.WebApi.Models.DTOs;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Repositories.Interfaces;
using MySecureBackend.WebApi.Services;
using System.Threading.Tasks;

[TestClass]
public class UsersControllerTests
{
    private Mock<IUserRepository> _userRepoMock;
    private Mock<PasswordService> _passwordServiceMock;
    private UsersController _controller;

    [TestInitialize]
    public void Setup()
    {
        _userRepoMock = new Mock<IUserRepository>();
        _passwordServiceMock = new Mock<PasswordService>();

        _controller = new UsersController(
            _userRepoMock.Object,
            _passwordServiceMock.Object
        );
    }

    [TestMethod]
    public async Task Register_UsernameAlreadyExists_ReturnsBadRequest()
    {
        // Arrange
        var request = new RegisterRequest
        {
            UserName = "testuser",
            Password = "ValidPass123!"
        };

        _userRepoMock
            .Setup(r => r.GetByUserName("testuser"))
            .ReturnsAsync(new User { UserName = "testuser" });

        // Act
        var result = await _controller.Register(request);

        // Assert
        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }

    [TestMethod]
    public async Task Register_WeakPassword_ReturnsBadRequest()
    {
        var request = new RegisterRequest
        {
            UserName = "newuser",
            Password = "123"
        };

        var result = await _controller.Register(request);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
}