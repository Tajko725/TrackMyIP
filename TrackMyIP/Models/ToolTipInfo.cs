
namespace TrackMyIP.Models
{
    /// <summary>
    /// Represents information for a tooltip, including its title and description.
    /// </summary>
    /// <param name="title">The title of the tooltip.</param>
    /// <param name="description">The description or content of the tooltip.</param>
    public class ToolTipInfo(string title, string description)
    {
        /// <summary>
        /// Gets the title of the tooltip.
        /// </summary>
        public string Title { get; private set; } = title;

        /// <summary>
        /// Gets the description or content of the tooltip.
        /// </summary>
        public string Description { get; private set; } = description;
    }

}