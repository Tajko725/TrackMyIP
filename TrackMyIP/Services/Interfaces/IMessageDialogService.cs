using MahApps.Metro.Controls.Dialogs;
using TrackMyIP.Models;

namespace TrackMyIP.Services.Interfaces
{
    /// <summary>
    /// Provides methods for displaying various types of dialogs within the application.
    /// </summary>
    public interface IMessageDialogService
    {
        /// <summary>
        /// Initializes the dialog service with a specified context.
        /// </summary>
        /// <param name="context">The context object used for displaying dialogs.</param>
        void Initialize(object context);

        /// <summary>
        /// Displays a simple message dialog with a title and message content.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="message">The message content of the dialog.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's response to the dialog.</returns>
        Task<MessageDialogResult> ShowMessageAsync(string title, string message);

        /// <summary>
        /// Displays a message dialog with a title, message content, and a specified dialog style.
        /// </summary>
        /// <param name="title">The title of the dialog.</param>
        /// <param name="message">The message content of the dialog.</param>
        /// <param name="style">The style of the dialog, specifying the buttons or options available.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's response to the dialog.</returns>
        Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style);

        /// <summary>
        /// Displays a message dialog using a <see cref="MessageInfo"/> object.
        /// </summary>
        /// <param name="messageInfo">The message information, including title and content.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's response to the dialog.</returns>
        Task<MessageDialogResult> ShowMessageAsync(MessageInfo messageInfo);

        /// <summary>
        /// Displays a message dialog using a <see cref="MessageInfo"/> object and a specified dialog style.
        /// </summary>
        /// <param name="messageInfo">The message information, including title and content.</param>
        /// <param name="style">The style of the dialog, specifying the buttons or options available.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the user's response to the dialog.</returns>
        Task<MessageDialogResult> ShowMessageAsync(MessageInfo messageInfo, MessageDialogStyle style);
    }
}