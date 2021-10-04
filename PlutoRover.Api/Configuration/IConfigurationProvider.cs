using System.Collections.Generic;
using PlutoRover.Api.Application.Options;

namespace PlutoRover.Api.Configuration
{
    /// <summary>
    /// Wrapper class around frameworks IConfiguration
    /// </summary>
    public interface IConfigurationProvider
    {
        /// <summary>
        /// Gets Pluto's configuration
        /// </summary>
        MapOptions PlutoMapOptions { get; }
    }
}