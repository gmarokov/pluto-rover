using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using PlutoRover.Api.Application;
using PlutoRover.Api.Application.Options;
using Xunit;
using IConfigurationProvider = PlutoRover.Api.Configuration.IConfigurationProvider;

namespace PlutoRover.Tests
{
    public class MapTests
    {
        [Theory]
        [MemberData(nameof(MapInstanceParameters))]
        public void Map_Should_Throw_When_NullParamsPassed(IConfigurationProvider config, ILogger<Map> logger)
        {
            //Arrange & Act
            var exception = Record.Exception(() => new Map(config, logger));
            
            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        
        [Fact]
        public void Map_Should_Generate_Layout()
        {
            //Arrange
            const int expectedRows = 10;
            const int expectedCols = 20;
            var configProviderMock = new Mock<IConfigurationProvider>();
            configProviderMock.SetupGet(x => x.PlutoMapOptions)
                .Returns(new MapOptions()
                {
                    MapDimensions = new MapDimensions()
                    {
                        Rows = expectedRows,
                        Columns = expectedCols
                    }
                });

            //Act
            var map = new Map(configProviderMock.Object, Mock.Of<ILogger<Map>>());

            //Assert
            Assert.NotNull(map.MapLayout);
            Assert.True(map.MapLayout.GetLength(0) == expectedRows 
                && map.MapLayout.GetLength(1) == expectedCols);
        }
        
        [Fact]
        public void Map_Should_Generate_Layout_WithObstacles()
        {
            //Arrange
            const int expectedRows = 10;
            const int expectedCols = 20;
            const int expectedRow = 5;
            const int expectedCol = 5;
            var configProviderMock = new Mock<IConfigurationProvider>();
            configProviderMock.SetupGet(x => x.PlutoMapOptions)
                .Returns(new MapOptions()
                {
                    MapDimensions = new MapDimensions()
                    {
                        Rows = expectedRows,
                        Columns = expectedCols
                    },
                    ObstaclesPositions = new List<ObstaclePosition>()
                    {
                        new ObstaclePosition()
                        {
                            Row = expectedRow, Column = expectedCol
                        }
                    }
                });

            //Act
            var map = new Map(configProviderMock.Object, Mock.Of<ILogger<Map>>());

            //Assert
            Assert.NotNull(map.MapLayout);
            Assert.True(map.MapLayout[expectedRow, expectedCol] == MapElements.Obstacle);
        }
        
        public static IEnumerable<object[]> MapInstanceParameters =>
            new List<object[]>
            {
                new object[] { null, Mock.Of<ILogger<Map>>() },
                new object[] { Mock.Of<IConfigurationProvider>(), null },
                new object[] { null, null }
            };
    }
}