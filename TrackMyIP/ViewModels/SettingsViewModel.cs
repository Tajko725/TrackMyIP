using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Input;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;

namespace TrackMyIP.ViewModels
{
    /// <summary>
    /// Represents the view model for managing application settings.
    /// Provides properties, commands, and methods for loading, saving, and validating settings.
    /// Inherits from <see cref="BaseModel"/>.
    /// </summary>
    public partial class SettingsViewModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the dialog coordinator for displaying dialogs in the application.
        /// </summary>
        public IDialogCoordinator DialogCoordinator { get; }
        private readonly IIpStackService _ipStackService;

        /// <summary>
        /// Gets or sets the API key for accessing the ipstack service.
        /// </summary>
        [ObservableProperty]
        private string? _ipStackApiKey;

        /// <summary>
        /// Gets a value indicating whether the settings can be saved.
        /// The API key must not be null or whitespace.
        /// </summary>
        private bool CanSave => !string.IsNullOrWhiteSpace(IpStackApiKey);

        #endregion Properties

        #region Commands
        /// <summary>
        /// Command for loading settings from the application configuration file.
        /// </summary>
        public IRelayCommand? LoadCommand { get; private set; }

        /// <summary>
        /// Command for saving settings to the application configuration file.
        /// </summary>
        public IRelayCommand? SaveCommand { get; private set; }

        /// <summary>
        /// Command for validating the API key through a test request to the ipstack service.
        /// </summary>
        public IRelayCommand? CheckApiKeyIsValidCommand { get; private set; }

        /// <summary>
        /// Command for validating the API key through a test request to the ipstack service.
        /// </summary>
        public ICommand? GoToWwwCommand { get; private set; }

        #endregion Commands

        #region Buttons
        /// <summary>
        /// Button for loading settings.
        /// </summary>
        public ButtonInfo? LoadButton { get; private set; }

        /// <summary>
        /// Button for saving settings.
        /// </summary>
        public ButtonInfo? SaveButton { get; private set; }

        /// <summary>
        /// Button for validating the API key.
        /// </summary>
        public ButtonInfo? CheckApiKeyIsValidButton { get; private set; }
        #endregion Buttons

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsViewModel" /> class with the specified dependencies.
        /// </summary>
        /// <param name="ipStackService">The service used for managing interactions with the ipstack API.</param>
        /// <param name="dialogCoordinator">The dialog coordinator used for displaying dialogs in the application.</param>
        public SettingsViewModel(IIpStackService ipStackService, IDialogCoordinator dialogCoordinator)
        {
            _ipStackService = ipStackService;
            DialogCoordinator = dialogCoordinator;

            Initialize();
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initializes the view model by loading settings, initializing commands, and configuring buttons.
        /// </summary>
        private void Initialize()
        {
            Load(null!);
            InitializeCommands();
            InitializeButtons();
        }

        /// <summary>
        /// Initializes all commands used in the settings view model.
        /// </summary>
        private void InitializeCommands()
        {
            LoadCommand = new RelayCommand<object?>(Load);
            SaveCommand = new AsyncRelayCommand(SaveAsync, () => CanSave);
            CheckApiKeyIsValidCommand = new AsyncRelayCommand(CheckApiKeyIsValidAsync, () => CanSave);
            GoToWwwCommand = new RelayCommand<object?>(GoToWww);
        }

        /// <summary>
        /// Opens a URL in the system's default web browser.
        /// </summary>
        /// <param name="obj">The URL to open, passed as an object.</param>
        private void GoToWww(object? obj)
        {
            if (obj is string url)
                UrlNavigator.OpenUrl(url);
        }

        /// <summary>
        /// Initializes all buttons used in the settings view model.
        /// </summary>
        private void InitializeButtons()
        {
            LoadButton = new ButtonInfo("Wczytaj", LoadCommand!, toolTip: "Wczytaj ustawienia.");
            SaveButton = new ButtonInfo("Zapisz", SaveCommand!, toolTip: "Zapisz ustawienia.");
            CheckApiKeyIsValidButton = new ButtonInfo("Sprawdź", CheckApiKeyIsValidCommand!, toolTip: "Sprawdź poprawność klucza API poprzed pojedyncze zapytanie o www.google.pl.");
        }

        /// <summary>
        /// Loads settings from the application configuration file.
        /// </summary>
        /// <param name="obj">Optional parameter for the command.</param>
        private void Load(object? obj)
        {
            IpStackApiKey = AppConfigHelper.GetAppSetting("IPStackApiKey");
        }

        /// <summary>
        /// Saves the current settings to the application configuration file.
        /// </summary>
        private async Task SaveAsync()
        {
            AppConfigHelper.UpdateAppSetting("IPStackApiKey", IpStackApiKey!);

            await ShowMessageAsync("Zapis ustawień", "Ustawienia zapisane pomyślnie.");
        }

        /// <summary>
        /// Validates the API key by making a test request to the ipstack service.
        /// Displays the result in a dialog.
        /// </summary>
        private async Task CheckApiKeyIsValidAsync()
        {
            try
            {
                string resultMessage = await _ipStackService.ValidateApiKeyAsync();
                await ShowMessageAsync("Sprawdzenie poprawności klucza API", resultMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Contains("host")
                    ? "Problem z połączeniem internetowym."
                    : $"Błąd podczas odczytywania informacji:\n{ex.Message}";

                await ShowMessageAsync("Sprawdzenie poprawności klucza API", errorMessage);
            }
        }

        /// <summary>
        /// Displays a message dialog asynchronously using the specified dialog coordinator.
        /// </summary>
        /// <param name="messageInfo">The message details, including title and content, to display in the dialog.</param>
        /// <param name="dataContext">The data context of the current window, typically used for binding.</param>
        /// <param name="dialogCoordinator">The dialog coordinator used to display the dialog.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task ShowMessageAsync(string title, string message)
            => await MessageBoxEx.ShowMessageAsync(new MessageInfo(title, message), Application.Current.MainWindow.DataContext, DialogCoordinator);


        /// <summary>
        /// Invoked whenever the <see cref="IpStackApiKey"/> property value changes.
        /// Updates the execution state of the <see cref="SaveCommand"/> command based on the new value.
        /// </summary>
        /// <param name="value">The new value of the <see cref="IpStackApiKey"/> property.</param>
        partial void OnIpStackApiKeyChanged(string? value)
        {
            SaveCommand?.NotifyCanExecuteChanged();
            CheckApiKeyIsValidCommand?.NotifyCanExecuteChanged();
        }
        #endregion Methods
    }
}