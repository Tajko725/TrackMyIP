using MahApps.Metro.Controls.Dialogs;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;

namespace TrackMyIP.Services
{
    /// <summary>
    /// Initializes a new instance of the <see cref="MessageDialogService"/> class.
    /// </summary>
    /// <param name="dialogCoordinator">The dialog coordinator instance used to display dialogs.</param>
    public class MessageDialogService(IDialogCoordinator dialogCoordinator) : IMessageDialogService
    {
        private readonly IDialogCoordinator _dialogCoordinator = dialogCoordinator;
        private object? _context;

        /// <inheritdoc />
        public void Initialize(object context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<MessageDialogResult> ShowMessageAsync(string title, string message)
        {
            if (_context == null)
                throw new InvalidOperationException("Kontekst okna dialogowego nie został zainicjowany. Przed wyświetleniem komunikatu należy wywołać Initialize(context).");

            return await _dialogCoordinator.ShowMessageAsync(_context, title, message);
        }

        /// <inheritdoc />
        public async Task<MessageDialogResult> ShowMessageAsync(string title, string message, MessageDialogStyle style)
        {
            if (_context == null)
                throw new InvalidOperationException("Kontekst okna dialogowego nie został zainicjowany. Przed wyświetleniem komunikatu należy wywołać Initialize(context).");

            return await _dialogCoordinator.ShowMessageAsync(_context, title, message, style);
        }

        /// <inheritdoc />
        public async Task<MessageDialogResult> ShowMessageAsync(MessageInfo messageInfo)
        {
            if (_context == null)
                throw new InvalidOperationException("Kontekst okna dialogowego nie został zainicjowany. Przed wyświetleniem komunikatu należy wywołać Initialize(context).");

            return await _dialogCoordinator.ShowMessageAsync(_context, messageInfo.Title, messageInfo.Message);
        }

        /// <inheritdoc />
        public async Task<MessageDialogResult> ShowMessageAsync(MessageInfo messageInfo, MessageDialogStyle style)
        {
            if (_context == null)
                throw new InvalidOperationException("Kontekst okna dialogowego nie został zainicjowany. Przed wyświetleniem komunikatu należy wywołać Initialize(context).");

            return await _dialogCoordinator.ShowMessageAsync(_context, messageInfo.Title, messageInfo.Message, style);
        }
    }
}