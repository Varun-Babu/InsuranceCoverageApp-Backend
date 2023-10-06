using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using FluentAssertions;
using AutoFixture;
using Microsoft.AspNetCore.Mvc;
using PolicyCoverageApi.Controllers;
using PolicyCoverageApi.interfaces;
using PolicyCoverageApi.Models;
using System.Diagnostics;
using PolicyCoverageApi.models;
using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Xunit.Sdk;
using Xunit.Abstractions;

namespace PolicyCoverageApi.Tests.Controllers
{
    public class UserPolicyControllerTests
    {
        private readonly IFixture _fixture;
        private readonly Mock<IUserPolicy> _mockUserPolicy;
        private readonly UserPolicyController _controller;

        public UserPolicyControllerTests()
        {
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
            .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            _mockUserPolicy = new Mock<IUserPolicy>();
            _controller = new UserPolicyController(
                userPolicy: _mockUserPolicy.Object
            );
        }

        [Fact]
        public async Task DeletePolicyNumber_ReturnsOk_OnSuccessfulDeletion()
        {
            // Arrange
            var policyNo = _fixture.Create<int>();

            var expectedResult = new OkObjectResult(new { message = "Policy Deleted" });
            _mockUserPolicy.Setup(repo => repo.DeleteUserPolicy(policyNo))
                           .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.DeletePolicyNumber(policyNo);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
                .Which.Value.Should().BeEquivalentTo(new { message = "Policy Deleted" });

            // Verify
            _mockUserPolicy.Verify(repo => repo.DeleteUserPolicy(policyNo), Times.Once);
        }

        [Fact]
        public async Task DeletePolicyNumber_ReturnsBadRequest_OnException()
        {
            // Arrange
            var policyNo = _fixture.Create<int>();

            var expectedErrorMessage = "An error occurred while deleting the policy.";
            _mockUserPolicy.Setup(repo => repo.DeleteUserPolicy(policyNo))
                           .ThrowsAsync(new Exception(expectedErrorMessage));

            // Act
            var result = await _controller.DeletePolicyNumber(policyNo);

            // Assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().BeEquivalentTo(expectedErrorMessage);

            // Verify
            _mockUserPolicy.Verify(repo => repo.DeleteUserPolicy(policyNo), Times.Once);
        }


        [Fact]
        public async Task AddPolicyNumberAndValidate_ReturnsOkObjectResult_WhenPolicyIsValid()
        {
            // Arrange
            var policyList = _fixture.Create<UserPolicyList>();
            var chasisNumber = _fixture.Create<string>();
            var expectedResult = new OkObjectResult(new { message = "ok" });
            _mockUserPolicy.Setup(repo => repo.AddPolicyNumberAndValidate(policyList, chasisNumber))
                .ReturnsAsync(expectedResult);

            // Act
            var result = await _controller.AddPolicyNumberAndValidate(policyList, chasisNumber);

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>()
                .Which.Should().BeEquivalentTo(expectedResult);

            // Verify
            _mockUserPolicy.Verify(repo => repo.AddPolicyNumberAndValidate(policyList, chasisNumber), Times.Once);
        }

        [Fact]
        public async Task AddPolicyNumberAndValidate_ReturnsBadRequestObject_WhenExceptionOccurs()
        {
            // Arrange
            var policyList = _fixture.Create<UserPolicyList>();
            var chasisNumber = _fixture.Create<string>();
            var exceptionMessage = "An error occurred while processing the request.";
            _mockUserPolicy.Setup(repo => repo.AddPolicyNumberAndValidate(policyList, chasisNumber))
                .ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.AddPolicyNumberAndValidate(policyList, chasisNumber);

            // Assert
            result.Should().NotBeNull().And.BeOfType<BadRequestObjectResult>()
                .Which.Value.Should().Be(exceptionMessage);

            // Verify
            _mockUserPolicy.Verify(repo => repo.AddPolicyNumberAndValidate(policyList, chasisNumber), Times.Once);
        }


        [Fact]
        public async Task GetAllPolicyNumbers_ReturnsOkObjectResult_WithValidUserId()
        {
            // Arrange
            int userId = _fixture.Create<int>();
            var expectedPolicyNumbers = _fixture.CreateMany<UserPolicyList>().ToList();

            _mockUserPolicy.Setup(repo => repo.GetAllPolicyNumbers(userId))
                           .ReturnsAsync(expectedPolicyNumbers);

            // Act
            var result = await _controller.GetAllPolicyNumbers(userId);

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<List<UserPolicyList>>>()
       .Which.Result.Should().BeOfType<OkObjectResult>();

            //verify
            _mockUserPolicy.Verify(repo => repo.GetAllPolicyNumbers(userId), Times.Once);
        }


        [Fact]
        public async Task GetAllPolicyNumbers_ReturnsBadRequestResult_WhenExceptionOccurs()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var errorMessage = "An error occurred.";
            _mockUserPolicy.Setup(repo => repo.GetAllPolicyNumbers(userId))
                           .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetAllPolicyNumbers(userId);

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<List<UserPolicyList>>>()
          .Which.Result.Should().BeOfType<BadRequestObjectResult>()
          .Which.Value.Should().Be(errorMessage);

            // Verify
            _mockUserPolicy.Verify(repo => repo.GetAllPolicyNumbers(userId), Times.Once);
        }


        [Fact]
        public async Task GetCoveragesByPolicyNumber_ReturnsBadRequestResult_WhenExceptionOccurs()
        {
            // Arrange
            var policyNumber = _fixture.Create<int>();
            var errorMessage = "An error occurred.";
            _mockUserPolicy.Setup(repo => repo.GetCoveragesByPolicyNumber(policyNumber))
                           .ThrowsAsync(new Exception(errorMessage));

            // Act
            var result = await _controller.GetCoveragesByPolicyNumber(policyNumber);

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<List<Coverage>>>()
         .Which.Result.Should().BeOfType<BadRequestObjectResult>()
         .Which.Value.Should().Be(errorMessage);

            // Verify
            _mockUserPolicy.Verify(repo => repo.GetCoveragesByPolicyNumber(policyNumber), Times.Once);
        }


        [Fact]
        public async Task GetCoveragesByPolicyNumber_ReturnsOkObjectResult_WhenCoveragesExit()
        {
            // Arrange
            var policyNumber = _fixture.Create<int>();
            var coverages = _fixture.CreateMany<Coverage>().ToList();

            _mockUserPolicy.Setup(repo => repo.GetCoveragesByPolicyNumber(policyNumber))
                           .ReturnsAsync(coverages);

            // Act
            var result = await _controller.GetCoveragesByPolicyNumber(policyNumber);

            // Assert
            result.Should().NotBeNull().And.BeOfType<ActionResult<List<Coverage>>>()
        .Which.Result.Should().BeOfType<OkObjectResult>();


            // Verify
            _mockUserPolicy.Verify(repo => repo.GetCoveragesByPolicyNumber(policyNumber), Times.Once);
        }

    }

}