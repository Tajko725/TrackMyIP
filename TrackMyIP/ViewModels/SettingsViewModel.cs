using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private readonly IIpStackService _ipStackService;
        private readonly IMessageDialogService _dialogService;
        private readonly IUrlNavigatorService _urlNavigatorService;

        /// <summary>
        /// Gets or sets a value indicating whether any property in the view model has been modified.
        /// This property is set to <c>true</c> whenever a property is updated,
        /// and set to <c>false</c> after saving or loading the settings.
        /// </summary>
        /// <remarks>
        /// Used to track changes in the view model to prompt the user or perform specific actions
        /// when unsaved changes exist.
        /// </remarks>
        [ObservableProperty]
        private bool _isChanged;

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
        public IAsyncRelayCommand? SaveCommandAsync { get; private set; }

        /// <summary>
        /// Command for validating the API key through a test request to the ipstack service.
        /// </summary>
        public IAsyncRelayCommand? CheckApiKeyIsValidCommandAsync { get; private set; }

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
        /// <param name="dialogService">Service for dialogs.</param>
        /// <param name="urlNavigatorService">The service for navigating to specified URLs in the default web browser.</param>
        public SettingsViewModel(IIpStackService ipStackService, IMessageDialogService dialogService, IUrlNavigatorService urlNavigatorService)
        {
            _ipStackService = ipStackService;
            _dialogService = dialogService;
            _urlNavigatorService = urlNavigatorService;

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
            
            _dialogService.Initialize(this);
        }

        /// <summary>
        /// Initializes all commands used in the settings view model.
        /// </summary>
        private void InitializeCommands()
        {
            LoadCommand = new RelayCommand<object?>(Load);
            SaveCommandAsync = new AsyncRelayCommand(SaveAsync, () => CanSave);
            CheckApiKeyIsValidCommandAsync = new AsyncRelayCommand(CheckApiKeyIsValidAsync, () => CanSave);
            GoToWwwCommand = new RelayCommand<object?>(GoToWww);
        }

        /// <summary>
        /// Opens a URL in the system's default web browser.
        /// </summary>
        /// <param name="obj">The URL to open, passed as an object.</param>
        private void GoToWww(object? obj)
        {
            if (obj is string url)
                _urlNavigatorService.OpenUrl(url);
        }

        /// <summary>
        /// Initializes all buttons used in the settings view model.
        /// </summary>
        private void InitializeButtons()
        {
            LoadButton = new ButtonInfo("Wczytaj", LoadCommand!, toolTip: "Wczytaj ustawienia.");
            SaveButton = new ButtonInfo("Zapisz", SaveCommandAsync!, toolTip: "Zapisz ustawienia.");
            CheckApiKeyIsValidButton = new ButtonInfo("Sprawdź", CheckApiKeyIsValidCommandAsync!, toolTip: "Sprawdź poprawność klucza API poprzed pojedyncze zapytanie o www.google.pl.");
        }

        /// <summary>
        /// Loads settings from the application configuration file.
        /// </summary>
        /// <param name="obj">Optional parameter for the command.</param>
        private void Load(object? obj)
        {
            IpStackApiKey = AppConfigHelper.GetAppSetting("IPStackApiKey");

            IsChanged = false;
        }

        /// <summary>
        /// Saves the current settings to the application configuration file.
        /// </summary>
        private async Task SaveAsync()
        {
            AppConfigHelper.UpdateAppSetting("IPStackApiKey", IpStackApiKey!);

            await _dialogService.ShowMessageAsync("Zapis ustawień", "Ustawienia zapisane pomyślnie.");

            IsChanged = false;
        }

        /// <summary>
        /// Validates the API key by making a test request to the ipstack service.
        /// Displays the result in a dialog.
        /// </summary>
        private async Task CheckApiKeyIsValidAsync()
        {
            try
            {
                if(IsChanged)
                {
                    var result = await _dialogService.ShowMessageAsync("Sprawdzenie poprawności klucza API", "Wprowadzono zmiany, zapisz zmiany i spróbuj ponownie. Zapisać?", MahApps.Metro.Controls.Dialogs.MessageDialogStyle.AffirmativeAndNegative);
                    if (result == MahApps.Metro.Controls.Dialogs.MessageDialogResult.Negative)
                        return;

                    await SaveAsync();
                }

                string resultMessage = await _ipStackService.ValidateApiKeyAsync();
                await _dialogService.ShowMessageAsync("Sprawdzenie poprawności klucza API", resultMessage);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Contains("host")
                    ? "Problem z połączeniem internetowym."
                    : $"Błąd podczas odczytywania informacji:\n{ex.Message}";

                await _dialogService.ShowMessageAsync("Sprawdzenie poprawności klucza API", errorMessage);
            }
        }

        /// <summary>
        /// Invoked whenever the <see cref="IpStackApiKey"/> property value changes.
        /// Updates the execution state of the <see cref="SaveCommandAsync"/> command based on the new value.
        /// </summary>
        /// <param name="value">The new value of the <see cref="IpStackApiKey"/> property.</param>
        partial void OnIpStackApiKeyChanged(string? value)
        {
            SaveCommandAsync?.NotifyCanExecuteChanged();
            CheckApiKeyIsValidCommandAsync?.NotifyCanExecuteChanged();
            _isChanged = true;
        }
        #endregion Methods
    }
}