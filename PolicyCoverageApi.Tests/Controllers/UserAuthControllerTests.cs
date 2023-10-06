using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using Moq;
using FluentAssertions;
using PolicyCoverageApi.Controllers;
using PolicyCoverageApi.interfaces;
using PolicyCoverageApi.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PolicyCoverageApi.Tests.Controllers
{
    public class UserAuthControllerTests
    {

        private readonly Fixture _fixture;
        private readonly Mock<IUserAuth> _mockUserAuth;
        private readonly UserAuthController _controller;

        public UserAuthControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                    .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mockUserAuth = new Mock<IUserAuth>();
            _controller = new UserAuthController(_mockUserAuth.Object);
        }

        [Fact]
        public async Task Authenticate_ReturnsOkObjectResult_WithValidUser()
        {
            // Arrange
            var user = _fixture.Create<PortalUser>();
            _mockUserAuth.Setup(repo => repo.AuthenticateAsync(user.UserName, user.Password))
                .ReturnsAsync(new OkObjectResult(user));

            // Act
            var result = await _controller.Authenticate(user);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
            var okResult = result.As<OkObjectResult>();
            okResult.Value.Should().BeEquivalentTo(user);
        }

        [Fact]
        public async Task Authenticate_ReturnsBadRequest_WhenExceptionOccurs()
        {
            // Arrange
            var user = _fixture.Create<PortalUser>();
            var errorMessage = _fixture.Create<string>();
          _mockUserAuth.Setup(repo => repo.AuthenticateAsync(user.UserName, user.Password))
                .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.Authenticate(user);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
            var badRequestResult = result.As<BadRequestObjectResult>();
            badRequestResult.Value.Should().Be(errorMessage);
        }
    }
    }

