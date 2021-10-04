using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Extensions.Logging;
using Moq;
using PlutoRover.Api.Application;
using PlutoRover.Api.Infrastructure;
using Xunit;

namespace PlutoRover.Tests
{
    public class CommanderTests
    {
        [Theory] 
        [MemberData(nameof(CommanderInstanceParameters))]
        public void Commander_Should_Throw_When_NullParamsPassed(IRover rover, IReader reader, ILogger<Commander> logger)
        {
            //Arrange & Act
            var exception = Record.Exception(() => new Commander(rover, reader, logger));
            
            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        
        public static IEnumerable<object[]> CommanderInstanceParameters =>
            new List<object[]>
            {
                new object[] { null, Mock.Of<IReader>(), Mock.Of<ILogger<Commander>>() },
                new object[] { Mock.Of<IRover>(), Mock.Of<IReader>(), null },
                new object[] { Mock.Of<IRover>(), null,  Mock.Of<ILogger<Commander>>() }
            };

        [Theory]
        [InlineData("FFRBLR1")]
        [InlineData("13234")]
        [InlineData("  ")]
        [InlineData("!@#$")]
        public void Start_Should_Throw_When_TryParseCommands(string commands)
        {
            //Arrange
            var readerMock = new Mock<IReader>();
            var loggerMock = new Mock<ILogger<Commander>>();
            var roverMock = new Mock<IRover>();
            
            readerMock
                .Setup(x => x.Read())
                .Returns(commands);
            var commander = new Commander(roverMock.Object, readerMock.Object, loggerMock.Object);
            
            //Act
            var exception = Record.Exception(() => commander.Start());
            
            //Assert
            Assert.NotNull(exception);
            Assert.IsType<InvalidEnumArgumentException>(exception);
        }
        
        //TODO: Should work with lower letters too
        //TODO Successful test
    }
}