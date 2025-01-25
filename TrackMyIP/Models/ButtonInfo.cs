using System.Windows.Input;

namespace TrackMyIP.Models
{
    /// <summary>
    /// Represents information about a button, including its content, command, command parameter, and tooltip.
    /// Inherits from <see cref="BaseModel"/>.
    /// </summary>
    /// <param name="content">The text displayed on the button.</param>
    /// <param name="command">The command executed when the button is clicked.</param>
    /// <param name="commandParameter">An optional parameter passed to the command when executed.</param>
    /// <param name="toolTip">The tooltip displayed when hovering over the button.</param>
    public class ButtonInfo(string content, ICommand command, object? commandParameter = null, string? toolTip = null) : BaseModel
    {
        /// <summary>
        /// Gets the text displayed on the button.
        /// </summary>
        public string Content { get; private set; } = content;

        /// <summary>
        /// Gets the command executed when the button is clicked.
        /// </summary>
        public ICommand Command { get; private set; } = command;

        /// <summary>
        /// Gets the optional parameter passed to the command when executed.
        /// </summary>
        public object? CommandParameter { get; private set; } = commandParameter;

        /// <summary>
        /// Gets the tooltip displayed when hovering over the button.
        /// </summary>
        public string? ToolTip { get; private set; } = toolTip;
    }
}
