using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using PlutoRover.Api.Application;
using PlutoRover.Api.Application.Options;
using PlutoRover.Api.Configuration;
using Xunit;

namespace PlutoRover.Tests
{
    public class RoverTests
    {
        private readonly Mock<Map> _mapMock;
        
        public RoverTests()
        {
            _mapMock = new Mock<Map>();
            var matrix = new int[10, 10];
            _mapMock.SetupGet(x => x.MapLayout).Returns(matrix);
        }
        
        [Fact]
        public void Move_Forward_FromInitialState()
        {
            //Arrange
            const string expectedCurrentPosition = "1,0,S";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Move(Commands.MoveForward);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }

        [Fact]
        public void Move_Backward_FromGivenState()
        {
            //Arrange
            const string expectedCurrentPosition = "2,0,S";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveBackward);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }

        [Fact]
        public void Move_Wrap_EdgeColumn()
        {
            //Arrange
            const string expectedCurrentPosition = "9,0,S";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Move(Commands.MoveBackward);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Move_Wrap_EdgeRow()
        {
            //Arrange
            const string expectedCurrentPosition = "0,9,W";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnRight);
            rover.Move(Commands.MoveForward);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }

        [Fact]
        public void Move_When_Obstacle_LeavesOnSamePlace()
        {
            //Arrange
            const int expectedRowObstacle = 0;
            const int expectedColObstacle = 2;
            var configMock = new Mock<IConfigurationProvider>();
            var plutoOptions = new MapOptions()
            {
                MapDimensions = new MapDimensions()
                {
                    Rows = 10,
                    Columns = 10
                },
                ObstaclesPositions = new List<ObstaclePosition>()
                {
                    new ObstaclePosition()
                    {
                        Row = expectedRowObstacle,
                        Column = expectedColObstacle
                    }
                }
            };
            configMock.SetupGet(x => x.PlutoMapOptions).Returns(plutoOptions);
            var map = new Map(configMock.Object, Mock.Of<ILogger<Map>>());
            
            const string expectedCurrentPosition = "0,1,E";
            var rover = new Rover(map, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Left_FromSouth_ShouldBe_East()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,E";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Left_Twice_FromSouth_ShouldBe_North()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,N";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Left_Thrice_FromSouth_ShouldBe_West()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,W";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Left_FourTimes_FromSouth_ShouldBe_South()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,S";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Right_FromSouth_ShouldBe_West()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,W";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnRight);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Right_Twice_FromSouth_ShouldBe_North()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,N";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnRight);
            rover.Turn(Commands.TurnRight);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Right_Thrice_FromSouth_ShouldBe_East()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,E";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnRight);
            rover.Turn(Commands.TurnRight);
            rover.Turn(Commands.TurnRight);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void Turn_Right_FourTimes_FromSouth_ShouldBe_South()
        {
            //Arrange
            const string expectedCurrentPosition = "0,0,S";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);
            rover.Turn(Commands.TurnLeft);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
        
        [Fact]
        public void MoveAndTurn_ShouldHave_RightPosition()
        {
            //Arrange
            const string expectedCurrentPosition = "8,9,N";
            var rover = new Rover(_mapMock.Object, Mock.Of<ILogger<Rover>>());
            
            //Act
            rover.Turn(Commands.TurnLeft);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Turn(Commands.TurnRight);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Turn(Commands.TurnLeft);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveBackward);
            rover.Move(Commands.MoveBackward);
            rover.Move(Commands.MoveBackward);
            rover.Move(Commands.MoveBackward);
            rover.Move(Commands.MoveBackward);
            rover.Turn(Commands.TurnLeft);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);
            rover.Move(Commands.MoveForward);

            //Assert
            Assert.Equal(expectedCurrentPosition, rover.CurrentPosition);
        }
    }
}