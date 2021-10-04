using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Microsoft.Extensions.Logging;
using PlutoRover.Api.Application.Options;
using PlutoRover.Api.Infrastructure;

namespace PlutoRover.Api.Application
{
    /// <summary>
    /// Commander class for managing the application flow and commands
    /// </summary>
    public class Commander
    {
        private readonly ILogger<Commander> _logger;
        private readonly IReader _reader;
        private readonly IRover _rover;
        
        public Commander(
            IRover rover, 
            IReader reader,
            ILogger<Commander> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("Commander is initializing..");
            _reader = reader ?? throw new ArgumentNullException(nameof(reader));
            _rover = rover ?? throw new ArgumentNullException(nameof(rover));
        }
        
        /// <summary>
        /// Starting the application and accept commands
        /// </summary>
        public void Start()
        {
            _logger.LogInformation($"Commander is starting at {DateTime.UtcNow}");
            _logger.LogInformation($"Rover is at starting position: {_rover.CurrentPosition}");
            _logger.LogInformation("Commander is ready to accept commands. Use 'F' for forward, 'B' for backward, 'L' for left, 'R' for right and 'S' for stopping the rover:");
            var hasStopCommand = false;
            while(!hasStopCommand)
            {
                var commands = ReadCommands();
                _logger.LogInformation($"Proccessing commands: {string.Join("", commands)}");
                hasStopCommand = TryExecuteCommands(commands);
            }
            Stop();
        }
        
        /// <summary>
        /// Handler error by logging it and try to restart the application
        /// </summary>
        /// <param name="ex"></param>
        public void HandleError(Exception ex)
        {
            _logger.LogError($"Rover encountered error at {DateTime.UtcNow} & error is: {ex.Message}");
            _logger.LogInformation($"Rover attempts to restart at {DateTime.UtcNow}");
            Start();
        }

        private bool TryExecuteCommands(IEnumerable<char> commands)
        {
            foreach (var command in commands)
            {
                if (command != Commands.StopCommand)
                    CallRover(command);
                else
                    return true;
            }

            return false;
        }

        private void CallRover(char command)
        {
            if (Commands.DirectionCommands.Contains(command))
                _rover.Turn(command);
            else if (Commands.MovingCommands.Contains(command))
                _rover.Move(command);
            _logger.LogInformation($"Rover performed an action and now is at: {_rover.CurrentPosition}");
        }

        private IEnumerable<char> ReadCommands()
        {
            var commands = _reader.Read().ToUpper().ToCharArray();
            
            foreach (var command in commands)
            {
                if (!Commands.AvailableCommands.Contains(command))
                    throw new InvalidEnumArgumentException($"Invalid command character '{command}'");
            }

            return commands;
        }

        private void Stop() =>
            _logger.LogInformation($"Rover stopped at {DateTime.UtcNow}");
    }
}