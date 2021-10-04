using System;
using Microsoft.Extensions.Logging;
using PlutoRover.Api.Application.Options;
using IConfigurationProvider = PlutoRover.Api.Configuration.IConfigurationProvider;

namespace PlutoRover.Api.Application
{
    /// <summary>
    /// Map class defining common map operations
    /// </summary>
    public class Map
    {
        private readonly ILogger<Map> _logger;
        private readonly MapOptions _options;

        protected Map() { }
        
        public Map(IConfigurationProvider config, ILogger<Map> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _logger.LogInformation("Map is initializing..");
            _ = config ?? throw new ArgumentNullException(nameof(config));
            _options = config.PlutoMapOptions;
            MapLayout = GenerateMap();
            _logger.LogInformation("Map is ready");
        }
        
        /// <summary>
        /// Map layout as two dimensional matrix
        /// </summary>
        public virtual int[,] MapLayout { get; }
        
        /// <summary>
        /// Set the initial position of the rover
        /// </summary>
        /// <param name="roverCol"></param>
        /// <param name="roverRow"></param>
        public void SetRoverInitialPosition(int roverCol, int roverRow) =>
            MapLayout[roverCol, roverRow] = MapElements.Rover;
        
        private int[,] GenerateMap()
        {
            _logger.LogInformation("Generating map..");
            var matrix = new int[_options.MapDimensions.Rows, _options.MapDimensions.Columns];
            
            foreach (var obstacle in _options.ObstaclesPositions)
                matrix[obstacle.Row, obstacle.Column] = MapElements.Obstacle;

            return matrix;
        }
    }
}