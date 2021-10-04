using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;
using ConfigurationProvider = PlutoRover.Api.Configuration.ConfigurationProvider;

namespace PlutoRover.Tests
{
    public class ConfigurationProviderTests
    {
        [Fact]
        public void ConfigurationProvider_Should_Throw_When_NullParametersPassed()
        {
            //Arrange & Act
            var exception = Record.Exception(() => new ConfigurationProvider(null));
            
            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentNullException>(exception);
        }
        
        [Theory]
        [InlineData(10, 10)]
        [InlineData(200, 100)]
        [InlineData(int.MaxValue, int.MaxValue - 100)]
        public void MapDimensions_Has_ValidValues(int rows, int cols)
        {
            //Arrange
            var collection = new Dictionary<string, string>()
            {
                {"MapDimensions:Rows", rows.ToString()},
                {"MapDimensions:Columns", cols.ToString()}
            };
        
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(collection)
                .Build();
            
            var configProvider = new ConfigurationProvider(config);
            
            //Act
            var result = configProvider.PlutoMapOptions;
        
            //Assert
            Assert.NotNull(result);
            Assert.True(result.MapDimensions.Columns == cols);
            Assert.True(result.MapDimensions.Rows == rows);
        }
        
        [Fact]
        public void MapDimensions_Has_InvalidValues()
        {
            //Arrange
            const int rows = -10;
            const int cols = -10;
            var collection = new Dictionary<string, string>()
            {
                {"MapDimensions:Rows", rows.ToString()},
                {"MapDimensions:Columns", cols.ToString()}
            };
        
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(collection)
                .Build();

            //Act
            var exception = Record.Exception(() => new ConfigurationProvider(config));
            
            //Assert
            Assert.NotNull(exception);
            Assert.IsType<ArgumentException>(exception);
        }
    }
}