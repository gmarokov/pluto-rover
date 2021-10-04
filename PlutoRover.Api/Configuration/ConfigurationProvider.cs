using System;
using Microsoft.Extensions.Configuration;
using PlutoRover.Api.Application.Options;

namespace PlutoRover.Api.Configuration
{
    /// <inheritdoc />
    public class ConfigurationProvider : IConfigurationProvider
    {
        public ConfigurationProvider(IConfiguration configuration)
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            
            configuration.Bind(PlutoMapOptions);
            if (PlutoMapOptions.MapDimensions.Columns <= 0 || PlutoMapOptions.MapDimensions.Rows <= 0)
                throw new ArgumentException("Map cannot be negative or zero");
        }
        
        /// <inheritdoc />
        public MapOptions PlutoMapOptions { get; } = new MapOptions();
    }
}