using System.Collections.Generic;

namespace PlutoRover.Api.Application.Options
{
    /// <summary>
    /// Map options mapped from appsettings.json
    /// </summary>
    public class MapOptions
    {
        public MapDimensions MapDimensions { get; set; } = new MapDimensions();

        public List<ObstaclePosition> ObstaclesPositions { get; set; } = new List<ObstaclePosition>();
    }
    
    public class MapDimensions
    {
        public int Rows { get; set; }
        public int Columns { get; set; }
    }
    
    public class ObstaclePosition
    {
        public int Row { get; set; }
        public int Column { get; set; }
    }
}