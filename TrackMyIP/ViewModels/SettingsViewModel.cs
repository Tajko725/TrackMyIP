using MahApps.Metro.Controls.Dialogs;
using System.Windows;
using System.Windows.Input;
using TrackMyIP.Models;
using TrackMyIP.Services;

namespace TrackMyIP.ViewModels
{
    /// <summary>
    /// Represents the view model for managing application settings.
    /// Provides properties, commands, and methods for loading, saving, and validating settings.
    /// Inherits from <see cref="BaseModel"/>.
    /// </summary>
    public class SettingsViewModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the dialog coordinator for displaying dialogs in the application.
        /// </summary>
        public IDialogCoordinator? DialogCoordinator { get; set; }
        private readonly IpStackService _ipStackService;

        private string? _ipStackApiKey;
        /// <summary>
        /// Gets or sets the API key for accessing the ipstack service.
        /// </summary>
        public string? IpStackApiKey
        {
            get => _ipStackApiKey;
            set
            {
                if (_ipStackApiKey != value)
                {
                    _ipStackApiKey = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the settings can be saved.
        /// The API key must not be null or whitespace.
        /// </summary>
        private bool CanSave => !string.IsNullOrWhiteSpace(_ipStackApiKey);
        
        #endregion Properties

        #region Commands
        /// <summary>
        /// Command for loading settings from the application configuration file.
        /// </summary>
        public ICommand? LoadCommand { get; private set; }

        /// <summary>
        /// Command for saving settings to the application configuration file.
        /// </summary>
        public ICommand? SaveCommand { get; private set; }

        /// <summary>
        /// Command for validating the API key through a test request to the ipstack service.
        /// </summary>
        public ICommand? CheckApiKeyIsValidCommand { get; private set; }

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
        /// Initializes a new instance of the <see cref="SettingsViewModel"/> class.
        /// Loads the initial settings and initializes commands and buttons.
        /// </summary>
        /// <param name="dialogCoordinator">The dialog coordinator for displaying dialogs.</param>
        public SettingsViewModel(IDialogCoordinator dialogCoordinator)
        {
            DialogCoordinator = dialogCoordinator;
            Load(null!);
            InitializeCommands();
            InitializeButtons();

            _ipStackService = new IpStackService();
        }
        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initializes all commands used in the settings view model.
        /// </summary>
        private void InitializeCommands()
        {
            LoadCommand = new RelayCommand(Load);
            SaveCommand = new RelayCommand(async _ => await Save(), x => CanSave);
            CheckApiKeyIsValidCommand = new RelayCommand(CheckApiKeyIsValid, x => CanSave);
            GoToWwwCommand = new RelayCommand(GoToWww);
        }

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
            LoadButton = new ButtonInfo("Wczytaj", LoadCommand!, null!, "Wczytaj ustawienia.");
            SaveButton = new ButtonInfo("Zapisz", SaveCommand!, null!, "Zapisz ustawienia.");
            CheckApiKeyIsValidButton = new ButtonInfo("Sprawdź", CheckApiKeyIsValidCommand!, null!, "Sprawdź poprawność klucza API poprzed pojedyncze zapytanie o www.google.pl.");
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
        private async Task Save()
        {
            AppConfigHelper.UpdateAppSetting("IPStackApiKey", IpStackApiKey!);

            await MessageBoxEx.ShowMessageAsync(new MessageInfo("Zapis ustawień", "Ustawienia zapisane pomyślnie."), Application.Current.MainWindow.DataContext, DialogCoordinator);
        }

        /// <summary>
        /// Validates the API key by making a test request to the ipstack service.
        /// Displays the result in a dialog.
        /// </summary>
        /// <param name="obj">Optional parameter for the command.</param>
        private async void CheckApiKeyIsValid(object? obj)
        {
            try
            {
                bool isOK = await _ipStackService.CheckApiKeyAsync();
                string resultMessage = !isOK ? "Nieprawidłowy klucz API." : "Podano prawidłowy klucz.";
                await MessageBoxEx.ShowMessageAsync(new MessageInfo("Sprawdzenie poprawności klucza API", resultMessage), Application.Current.MainWindow.DataContext, DialogCoordinator);
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Contains("host")
                    ? "Problem z połączeniem internetowym."
                    : $"Błąd podczas odczytywania informacji:\n{ex.Message}";

                await MessageBoxEx.ShowMessageAsync(new MessageInfo("Sprawdzenie poprawności klucza API", errorMessage), Application.Current.MainWindow.DataContext, DialogCoordinator);
            }
        }

        #endregion Methods
    }
}