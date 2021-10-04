namespace PlutoRover.Api.Infrastructure
{
    /// <summary>
    /// Interface for reading operations
    /// </summary>
    public interface IReader
    {
        /// <summary>
        /// Reads from source and returns as a string
        /// </summary>
        /// <returns>string</returns>
        string Read();
    }
}