using System.Collections.Generic;
using System.Linq;

namespace PlutoRover.Api.Application.Options
{
    /// <summary>
    /// Commands class for all available commands for the rover
    /// </summary>
    public static class Commands
    {
        public const char TurnLeft = 'L';
        public const char TurnRight = 'R';
        
        /// <summary>
        /// Commands for turning Left 'L' or Right 'R'
        /// </summary>
        public static readonly char[] DirectionCommands = { TurnLeft, TurnRight };

        public const char MoveForward = 'F';
        public const char MoveBackward = 'B';

        /// <summary>
        /// Commands for moving Forward 'F' or Backward 'B'
        /// </summary>
        public static readonly char[] MovingCommands = { MoveForward, MoveBackward };

        /// <summary>
        /// Command for Stopping the rover 'S'
        /// </summary>
        public const char StopCommand = 'S';
        
        /// <summary>
        /// Get aggregated list of all available commands
        /// </summary>
        public static IEnumerable<char> AvailableCommands => DirectionCommands.Concat(MovingCommands)
            .Append(StopCommand)
            .ToArray();
    }
}