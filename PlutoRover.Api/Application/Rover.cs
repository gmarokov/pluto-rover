using System;
using Microsoft.Extensions.Logging;
using PlutoRover.Api.Application.Options;

namespace PlutoRover.Api.Application
{
    /// <inheritdoc />
    public class Rover : IRover
    {
        private readonly ILogger<Rover> _logger;

        public Rover(Map map, ILogger<Rover> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("Rover is initializing..");
            MapLayout = map.MapLayout ?? throw new ArgumentNullException(nameof(map));
            CurrentCompassPoint = CompassPoints.South;
            map.SetRoverInitialPosition(0, 0);
            _logger.LogInformation("Rover is ready");
        }
        
        /// <inheritdoc />
        public void Move(char direction)
        {
            var isBackwards = direction == Commands.MoveBackward;
            var directionToggle = isBackwards ? -1 : 1;
            switch (CurrentCompassPoint)
            {
                case CompassPoints.North:
                {
                    TryMove(CurrentRow, CurrentCol - 1 * directionToggle,  isBackwards, isRow: false);
                    break;
                }
                case CompassPoints.South:
                {
                    TryMove(CurrentRow, CurrentCol + 1 * directionToggle, !isBackwards, isRow: false);
                    break;
                }
                case CompassPoints.West:
                {
                    TryMove(CurrentRow - 1 * directionToggle, CurrentCol, isBackwards, isRow: true);
                    break;
                }
                case CompassPoints.East:
                {
                    TryMove(CurrentRow + 1 * directionToggle, CurrentCol, !isBackwards, isRow: true);
                    break;
                }
            }
        }

        private void TryMove(int targetRow, int targetCol, bool isPositiveMove, bool isRow)
        {
            switch (isRow)
            {
                case true:
                    targetRow = isPositiveMove switch
                    {
                        false when targetRow < 0 => MapLayout.GetUpperBound(0),
                        true when targetRow > MapLayout.GetUpperBound(0) => 0,
                        _ => targetRow
                    };
                    break;
                case false:
                    targetCol = isPositiveMove switch
                    {
                        false when targetCol < 0 => MapLayout.GetUpperBound(1),
                        true when targetCol > MapLayout.GetUpperBound(1) => 0,
                        _ => targetCol
                    };
                    break;
            }

            if (MapLayout[targetRow, targetCol] == MapElements.Obstacle)
            {
                _logger.LogWarning("Obstacle ahead..staying in place.");
                return;
            }
                    
            MapLayout[targetRow, targetCol] = MapElements.Rover;
            MapLayout[CurrentRow, CurrentCol] = MapElements.Empty;
            CurrentCol = targetCol;
            CurrentRow = targetRow;
        }
        
        /// <inheritdoc />
        public void Turn(char direction)
        {
            CurrentCompassPoint = direction switch
            {
                Commands.TurnLeft => CurrentCompassPoint switch
                {
                    CompassPoints.North => CompassPoints.West,
                    CompassPoints.South => CompassPoints.East,
                    CompassPoints.West => CompassPoints.South,
                    CompassPoints.East => CompassPoints.North,
                    _ => CurrentCompassPoint
                },
                Commands.TurnRight => CurrentCompassPoint switch
                {
                    CompassPoints.North => CompassPoints.East,
                    CompassPoints.South => CompassPoints.West,
                    CompassPoints.West => CompassPoints.North,
                    CompassPoints.East => CompassPoints.South,
                    _ => CurrentCompassPoint
                },
                _ => CurrentCompassPoint
            };
        }
        
        /// <inheritdoc />
        public int[,] MapLayout { get; }

        /// <inheritdoc />
        public string CurrentPosition => $"{CurrentRow},{CurrentCol},{CurrentCompassPoint}";
        
        private char CurrentCompassPoint { get; set; }
        
        private int CurrentRow { get; set; }
        
        private int CurrentCol { get; set; }
    }
}