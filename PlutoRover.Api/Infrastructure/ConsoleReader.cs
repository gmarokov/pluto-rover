using System;

namespace PlutoRover.Api.Infrastructure
{
    /// <inheritdoc />
    public class ConsoleReader : IReader
    {
        /// <inheritdoc />
        public string Read()
        {
            return Console.ReadLine();
        }
    }
}