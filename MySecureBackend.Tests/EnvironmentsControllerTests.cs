using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using MySecureBackend.WebApi.Controllers;
using MySecureBackend.WebApi.Models.DTOs;
using MySecureBackend.WebApi.Repositories;
using MySecureBackend.WebApi.Repositories.Interfaces;
using System.Threading.Tasks;

[TestClass]
public class EnvironmentsControllerTests
{
    private Mock<IEnvironmentRepository> _repoMock;
    private EnvironmentsController _controller;

    [TestInitialize]
    public void Setup()
    {
        _repoMock = new Mock<IEnvironmentRepository>();
        _controller = new EnvironmentsController(_repoMock.Object);
    }

    [TestMethod]
    public async Task Create_MoreThan5Environments_ReturnsBadRequest()
    {
        var request = new CreateEnvironmentRequest
        {
            UserId = 1,
            Name = "World6",
            Width = 50,
            Height = 50
        };

        _repoMock
            .Setup(r => r.CountByUser(1))
            .ReturnsAsync(5);

        _repoMock
            .Setup(r => r.DoesNameExists(1, "World6"))
            .ReturnsAsync(false);

        var result = await _controller.Create(request);

        Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
    }
}