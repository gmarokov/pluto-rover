namespace PlutoRover.Api.Application
{
    /// <summary>
    /// Rover interface defining common rover properties and methods
    /// </summary>
    public interface IRover
    {
        /// <summary>
        /// Move the rover by giving character direction
        /// </summary>
        /// <param name="direction"></param>
        void Move(char direction);

        /// <summary>
        /// Turn the rover by giving character direction
        /// </summary>
        /// <param name="direction"></param>
        void Turn(char direction);
        
        /// <summary>
        /// Layout of the map the rover is on
        /// </summary>
        int[,] MapLayout { get; }

        /// <summary>
        /// Current position of the rover in format "0,0,N"
        /// </summary>
        string CurrentPosition { get; }
    }
}