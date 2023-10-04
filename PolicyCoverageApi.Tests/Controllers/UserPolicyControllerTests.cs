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



    }
}




