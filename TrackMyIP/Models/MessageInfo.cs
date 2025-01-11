namespace TrackMyIP.Models
{
    /// <summary>
    /// Represents information about a message, including its title and content.
    /// Inherits from <see cref="BaseModel"/>.
    /// </summary>
    /// <param name="title">The title of the message.</param>
    /// <param name="message">The content of the message.</param>
    public class MessageInfo(string title, string message) : BaseModel
    {
        /// <summary>
        /// Gets the title of the message.
        /// </summary>
        public string Title { get; private set; } = title;

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public string Message { get; private set; } = message;
    }
}