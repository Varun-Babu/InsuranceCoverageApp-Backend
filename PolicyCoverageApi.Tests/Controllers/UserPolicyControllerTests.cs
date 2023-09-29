using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
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
                userDbContext: null,
                userPolicy: _mockUserPolicy.Object
            );
        }

        #region add Policy No

        [Fact]
        public async Task AddPolicyNumber_ShouldReturnOkResult_WhenDataValid()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var policyNumber = _fixture.Create<int>();
            var chasisNumber = _fixture.Create<string>();

            var policy = _fixture.Create<Policy>();
            var policyVehicleRecord = _fixture.Create<Policyvehicle>();
            var vehicle = _fixture.Create<Vehicle>();
            var userPolicyList = _fixture.Create<UserPolicyList>();

            _mockUserPolicy.Setup(x => x.GetPolicyAsync(policyNumber)).ReturnsAsync(policy);
            _mockUserPolicy.Setup(x => x.GetPolicyvehicleAsync(policyNumber)).ReturnsAsync(policyVehicleRecord);
            _mockUserPolicy.Setup(x => x.validateChasisNumber(policyVehicleRecord.VehicleId, chasisNumber)).ReturnsAsync(vehicle);
            _mockUserPolicy.Setup(x => x.AddPolicyNumberAsync(userId, policyNumber)).ReturnsAsync(userPolicyList);

            // Act
            var result = await _controller.AddPloicyNumber(userId, policyNumber, chasisNumber) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(userPolicyList);

            _mockUserPolicy.Verify(x => x.GetPolicyAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.GetPolicyvehicleAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.validateChasisNumber(policyVehicleRecord.VehicleId, chasisNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.AddPolicyNumberAsync(userId, policyNumber), Times.Once);
        }

        [Fact]
        public async Task AddPolicyNumber_ReturnsNotFound_WhenPolicyNoInvalid()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var policyNumber = _fixture.Create<int>();
            var chasisNumber = _fixture.Create<string>();

            _mockUserPolicy.Setup(x => x.GetPolicyAsync(policyNumber)).ReturnsAsync((Policy)null);

            // Act
            var result = await _controller.AddPloicyNumber(userId, policyNumber, chasisNumber) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be("Policy Number doesnt exist");

            _mockUserPolicy.Verify(x => x.GetPolicyAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.GetPolicyvehicleAsync(It.IsAny<int>()), Times.Never);
            _mockUserPolicy.Verify(x => x.validateChasisNumber(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockUserPolicy.Verify(x => x.AddPolicyNumberAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddPolicyNumber_ReturnsNotFound_WhenPolicyVehicleInvalid()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var policyNumber = _fixture.Create<int>();
            var chasisNumber = _fixture.Create<string>();

            var policy = _fixture.Create<Policy>();
            _mockUserPolicy.Setup(x => x.GetPolicyAsync(policyNumber)).ReturnsAsync(policy);
            _mockUserPolicy.Setup(x => x.GetPolicyvehicleAsync(policyNumber)).ReturnsAsync((Policyvehicle)null);

            // Act
            var result = await _controller.AddPloicyNumber(userId, policyNumber, chasisNumber) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be("No Vehicle Found For Corresponding PolicyNumber");

            _mockUserPolicy.Verify(x => x.GetPolicyAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.GetPolicyvehicleAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.validateChasisNumber(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            _mockUserPolicy.Verify(x => x.AddPolicyNumberAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddPolicyNumber_ReturnsNotFound_WhenChasisNoInvalid()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var policyNumber = _fixture.Create<int>();
            var chasisNumber = _fixture.Create<string>();

            var policy = _fixture.Create<Policy>();
            var policyVehicleRecord = _fixture.Create<Policyvehicle>();
            _mockUserPolicy.Setup(x => x.GetPolicyAsync(policyNumber)).ReturnsAsync(policy);
            _mockUserPolicy.Setup(x => x.GetPolicyvehicleAsync(policyNumber)).ReturnsAsync(policyVehicleRecord);
            _mockUserPolicy.Setup(x => x.validateChasisNumber(policyVehicleRecord.VehicleId, chasisNumber)).ReturnsAsync((Vehicle)null);

            // Act
            var result = await _controller.AddPloicyNumber(userId, policyNumber, chasisNumber) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be("ChasisNumber doesnt exist");

            _mockUserPolicy.Verify(x => x.GetPolicyAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.GetPolicyvehicleAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.validateChasisNumber(policyVehicleRecord.VehicleId, chasisNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.AddPolicyNumberAsync(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddPolicyNumber_ReturnsBadRequest_OnException()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var policyNumber = _fixture.Create<int>();
            var chasisNumber = _fixture.Create<string>();

            _mockUserPolicy.Setup(x => x.GetPolicyAsync(policyNumber)).ThrowsAsync(new Exception("Some error occurred."));

            // Act
            var result = await _controller.AddPloicyNumber(userId, policyNumber, chasisNumber) as ObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().Be("Some error occurred.");

            _mockUserPolicy.Verify(x => x.GetPolicyAsync(policyNumber), Times.Once);

        }



            #endregion add policy no ends here

        #region get policy numbers test

        [Fact]
        public async Task GetPolicyNumbers_ShouldReturnOkResultWithPolicyNumbers_WhenValidUserId()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var userpolicy = _fixture.CreateMany<UserPolicyList>().ToList();
            var policyNumbers = _fixture.CreateMany<int>().ToList();

            _mockUserPolicy.Setup(x => x.GetAllPolicyNumbersAsync(userId)).ReturnsAsync(userpolicy);
            // Act
            var result = await _controller.GetPolicyNumbers(userId) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(userpolicy);

            _mockUserPolicy.Verify(x => x.GetAllPolicyNumbersAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetPolicyNumbers_ShouldReturnNoContentResult_WhenInvalidUserId()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            _mockUserPolicy.Setup(x => x.GetAllPolicyNumbersAsync(userId)).ReturnsAsync(new List<UserPolicyList>());

            // Act
            var result = await _controller.GetPolicyNumbers(userId) as NoContentResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status204NoContent);

            _mockUserPolicy.Verify(x => x.GetAllPolicyNumbersAsync(userId), Times.Once);
        }

        [Fact]
        public async Task GetPolicyNumbers_ShouldReturnBadRequestResultWithErrorMessage_WhenExceptionOccurs()
        {
            // Arrange
            var userId = _fixture.Create<int>();
            var exceptionMessage = "An error occurred while fetching policy numbers.";

            _mockUserPolicy.Setup(x => x.GetAllPolicyNumbersAsync(userId)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetPolicyNumbers(userId) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().BeEquivalentTo(exceptionMessage);

            _mockUserPolicy.Verify(x => x.GetAllPolicyNumbersAsync(userId), Times.Once);
        }

        #endregion get policy number ends here

        #region get coverage by policy no
        [Fact]
        public async Task GetCoveragesByPolicyNumber_ShouldReturnOkResultWithCoverage_WithValidPolicyNumber()
        {
            // Arrange
            var policyNumber = _fixture.Create<long>();
            var policyId = Guid.NewGuid().ToString();
            var coverage = _fixture.CreateMany<Coverage>().ToList();
            var policy = _fixture.Create<Policy>();
            policy.PolicyId = policyId; 

            _mockUserPolicy.Setup(x => x.GetPolicyByPolicyNumberAsync(policyNumber)).ReturnsAsync(policy);
            _mockUserPolicy.Setup(x => x.GetCoverageByPolicyIdAsync(policy.PolicyId)).ReturnsAsync(coverage);

            // Act
            var result = await _controller.GetCoveragesByPolicyNumber(policyNumber) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(coverage);

            _mockUserPolicy.Verify(x => x.GetPolicyByPolicyNumberAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.GetCoverageByPolicyIdAsync(policyId), Times.Once);
        }

        [Fact]
        public async Task GetCoveragesByPolicyNumber_ShouldReturnNotFoundResult_WithValidPolicyNumberAndInvalidCoverage()
        {
            // Arrange
            var policyNumber = _fixture.Create<long>();
            var policyId = Guid.NewGuid().ToString(); 
            var policy = _fixture.Create<Policy>();
            policy.PolicyId = policyId; 

            _mockUserPolicy.Setup(x => x.GetPolicyByPolicyNumberAsync(policyNumber)).ReturnsAsync(policy);
            _mockUserPolicy.Setup(x => x.GetCoverageByPolicyIdAsync(policy.PolicyId)).ReturnsAsync((List<Coverage>)null);

            // Act
            var result = await _controller.GetCoveragesByPolicyNumber(policyNumber) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be("Coverage not found");

            _mockUserPolicy.Verify(x => x.GetPolicyByPolicyNumberAsync(policyNumber), Times.Once);
            _mockUserPolicy.Verify(x => x.GetCoverageByPolicyIdAsync(policyId), Times.Once);
        }

        [Fact]
        public async Task GetCoveragesByPolicyNumber_ShouldReturnNotFoundResult_WhenInvalidPolicyNumber()
        {
            // Arrange
            var policyNumber = _fixture.Create<long>();

            _mockUserPolicy.Setup(x => x.GetPolicyByPolicyNumberAsync(policyNumber)).ReturnsAsync((Policy)null);

            // Act
            var result = await _controller.GetCoveragesByPolicyNumber(policyNumber) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().BeEquivalentTo("Policy not found.");

            _mockUserPolicy.Verify(x => x.GetPolicyByPolicyNumberAsync(policyNumber), Times.Once);
        }

        [Fact]
        public async Task GetCoveragesByPolicyNumber_ShouldReturnBadRequestResultWithErrorMessage_WhenExceptionOccurs()
        {
            // Arrange
            var policyNumber = _fixture.Create<long>();
            var exceptionMessage = "An error occurred while fetching coverage.";

            _mockUserPolicy.Setup(x => x.GetPolicyByPolicyNumberAsync(policyNumber)).ThrowsAsync(new Exception(exceptionMessage));

            // Act
            var result = await _controller.GetCoveragesByPolicyNumber(policyNumber) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().BeEquivalentTo(exceptionMessage);

            _mockUserPolicy.Verify(x => x.GetPolicyByPolicyNumberAsync(policyNumber), Times.Once);
        }

        #endregion ends here

        #region delete policy no

        [Fact]
        public async Task DeleteForm_ReturnsNotFoundResult__WhenPolicyNotFound_()
        {
            // Arrange
            var policyNo = _fixture.Create<long>();
            _mockUserPolicy.Setup(x => x.GetUserPolicyByPolicyNumberAsync(policyNo)).ReturnsAsync((UserPolicyList)null);

            // Act
            var result = await _controller.DeleteForm(policyNo) as NotFoundObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().BeEquivalentTo(new { message = "Policy not found" });

            _mockUserPolicy.Verify(x => x.GetUserPolicyByPolicyNumberAsync(policyNo), Times.Once);
            _mockUserPolicy.Verify(x => x.DeleteUserPolicyAsync(It.IsAny<UserPolicyList>()), Times.Never);
        }
        [Fact]
        public async Task DeleteForm_ReturnsOkResult_WhenPolicyFoundAndDeletedSuccessfully()
        {
            // Arrange
            var policyNo = _fixture.Create<long>();
            var existingPolicy = _fixture.Create<UserPolicyList>();
            _mockUserPolicy.Setup(x => x.GetUserPolicyByPolicyNumberAsync(policyNo)).ReturnsAsync(existingPolicy);

            // Act
            var result = await _controller.DeleteForm(policyNo) as OkObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(new { message = "Policy Deleted" });

            _mockUserPolicy.Verify(x => x.GetUserPolicyByPolicyNumberAsync(policyNo), Times.Once);
            _mockUserPolicy.Verify(x => x.DeleteUserPolicyAsync(existingPolicy), Times.Once);
        }
        [Fact]
        public async Task DeleteForm_ReturnsBadRequestResult_WhenExceptionOccurs()
        {
            // Arrange
            var policyNo = _fixture.Create<long>();
            _mockUserPolicy.Setup(x => x.GetUserPolicyByPolicyNumberAsync(policyNo)).ThrowsAsync(new Exception("Some error occurred."));

            // Act
            var result = await _controller.DeleteForm(policyNo) as BadRequestObjectResult;

            // Assert
            result.Should().NotBeNull();
            result.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
            result.Value.Should().BeEquivalentTo("Some error occurred.");

            _mockUserPolicy.Verify(x => x.GetUserPolicyByPolicyNumberAsync(policyNo), Times.Once);
            _mockUserPolicy.Verify(x => x.DeleteUserPolicyAsync(It.IsAny<UserPolicyList>()), Times.Never);
        }

        #endregion delete policy no ends here















    }
}




