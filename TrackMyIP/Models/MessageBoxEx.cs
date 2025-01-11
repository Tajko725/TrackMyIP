using MahApps.Metro.Controls.Dialogs;

namespace TrackMyIP.Models
{
    /// <summary>
    /// Provides extended functionality for displaying message dialogs using <see cref="IDialogCoordinator"/>.
    /// </summary>
    public class MessageBoxEx
    {
        /// <summary>
        /// Displays a message dialog with a title and message content.
        /// </summary>
        /// <param name="message">The information about the message, including the title and content.</param>
        /// <param name="context">The context object required by the <see cref="IDialogCoordinator"/> for displaying the dialog. Defaults to null.</param>
        /// <param name="dialogCoordinator">The dialog coordinator instance used to display the message. Defaults to null.</param>
        /// <returns>
        /// A <see cref="MessageDialogResult"/> indicating the user's response to the dialog.
        /// Returns <see cref="MessageDialogResult.Canceled"/> if either the context or dialog coordinator is null.
        /// </returns>
        public static async Task<MessageDialogResult> ShowMessageAsync(MessageInfo message, object? context = null, IDialogCoordinator? dialogCoordinator = null)
        {
            if (dialogCoordinator == null || context == null)
                return MessageDialogResult.Canceled;

            return await dialogCoordinator.ShowMessageAsync(context, message.Title, message.Message);
        }

        /// <summary>
        /// Displays a message dialog with a title, message content, and a specified dialog style.
        /// </summary>
        /// <param name="message">The information about the message, including the title and content.</param>
        /// <param name="messageDialogStyle">The dialog style specifying the buttons or options available in the dialog.</param>
        /// <param name="context">The context object required by the <see cref="IDialogCoordinator"/> for displaying the dialog. Defaults to null.</param>
        /// <param name="dialogCoordinator">The dialog coordinator instance used to display the message. Defaults to null.</param>
        /// <returns>
        /// A <see cref="MessageDialogResult"/> indicating the user's response to the dialog.
        /// Returns <see cref="MessageDialogResult.Canceled"/> if either the context or dialog coordinator is null.
        /// </returns>
        public static async Task<MessageDialogResult> ShowMessageAsync(MessageInfo message, MessageDialogStyle messageDialogStyle, object? context = null, IDialogCoordinator? dialogCoordinator = null)
        {
            if (dialogCoordinator == null || context == null)
                return MessageDialogResult.Canceled;

            return await dialogCoordinator.ShowMessageAsync(context, message.Title, message.Message, messageDialogStyle);
        }
    }
}