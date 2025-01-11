using MahApps.Metro.Controls.Dialogs;
using System.Windows.Input;
using TrackMyIP.Models;
using TrackMyIP.Services;
using TrackMyIP.Views;

namespace TrackMyIP.ViewModels
{
    /// <summary>
    /// Represents the view model for searching geolocation data.
    /// Provides properties, commands, and methods to manage the search process and user interactions.
    /// Inherits from <see cref="BaseModel"/>.
    /// </summary>
    public class SearchGeolocationViewModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// Gets or sets the dialog coordinator for displaying dialogs in the application.
        /// </summary>
        public IDialogCoordinator? DialogCoordinator { get; set; }
        private IpStackService _ipStackService { get; set; }

        private string? _addressSearched;
        /// <summary>
        /// Gets or sets the address or IP entered by the user for geolocation search.
        /// </summary>
        public string? AddressSearched
        {
            get => _addressSearched;
            set
            {
                if (_addressSearched != value)
                {
                    _addressSearched = value;
                    OnPropertyChanged();
                }
            }
        }

        private GeolocationData? _geolocationData;
        /// <summary>
        /// Gets or sets the geolocation data retrieved from the search.
        /// </summary>
        public GeolocationData? GeolocationData
        {
            get => _geolocationData;
            set
            {
                if (_geolocationData != value)
                {
                    _geolocationData = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the search command can be executed.
        /// The search is enabled when the <see cref="AddressSearched"/> property is not null or whitespace.
        /// </summary>
        private bool CanSearch => !string.IsNullOrWhiteSpace(AddressSearched);

        /// <summary>
        /// Gets a value indicating whether the add command can be executed.
        /// The add command is enabled when the <see cref="GeolocationData"/> property is not null.
        /// </summary>
        private bool CanAdd => GeolocationData != null;

        #endregion Properties

        #region Commands
        /// <summary>
        /// Command for initiating a geolocation search.
        /// </summary>
        public ICommand? SearchCommand { get; private set; }

        /// <summary>
        /// Command for adding the retrieved geolocation data.
        /// </summary>
        public ICommand? AddCommand { get; private set; }

        /// <summary>
        /// Command for closing the search window.
        /// </summary>
        public ICommand? CloseCommand { get; private set; }

        /// <summary>
        /// Command for handling the Enter key press during address input.
        /// </summary>
        public ICommand? AddressSearchedKeydownCommand { get; private set; }
        #endregion Commands

        #region Buttons
        /// <summary>
        /// Button for initiating a geolocation search.
        /// </summary>
        public ButtonInfo? SearchButton { get; private set; }

        /// <summary>
        /// Button for adding geolocation data to the collection.
        /// </summary>
        public ButtonInfo? AddButton { get; private set; }

        /// <summary>
        /// Button for closing the search window.
        /// </summary>
        public ButtonInfo? CloseButton { get; private set; }
        #endregion Buttons

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SearchGeolocationViewModel"/> class.
        /// Configures commands, buttons, and initializes the geolocation service.
        /// </summary>
        public SearchGeolocationViewModel()
        {
            InitializeCommands();
            InitializeButtons();

            _ipStackService = new IpStackService();
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Initializes all commands used in the search view model.
        /// </summary>
        private void InitializeCommands()
        {
            SearchCommand = new RelayCommand(async _ => await Search(), _ => CanSearch);
            AddCommand = new RelayCommand(Add, _ => CanAdd);
            CloseCommand = new RelayCommand(Close);
            AddressSearchedKeydownCommand = new RelayCommand(AddressSearchedKeydown);
        }

        /// <summary>
        /// Initializes all buttons used in the search view model.
        /// </summary>
        private void InitializeButtons()
        {
            SearchButton = new ButtonInfo("Wyszukaj", SearchCommand!, null!, "Wyszukaj dane geolokalizacyjne.");
            AddButton = new ButtonInfo("Dodaj", AddCommand!, null!, "Dodaj dane geolokalizacyjne.");
            CloseButton = new ButtonInfo("Zamknij", CloseCommand!, null!, "Zamknij okno.");
        }

        /// <summary>
        /// Executes the search command by fetching geolocation data asynchronously.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task Search()
        {
            await FetchLocationAsync();
        }

        /// <summary>
        /// Fetches geolocation data from the service based on the address or IP entered.
        /// Updates the <see cref="GeolocationData"/> property with the retrieved data.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        private async Task FetchLocationAsync()
        {
            if (string.IsNullOrWhiteSpace(AddressSearched))
                return;

            try
            {
                GeolocationData = await _ipStackService.FetchLocationAsync(AddressSearched);
                OnPropertyChanged(nameof(GeolocationData));
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Contains("host")
                    ? "Problem z połączeniem internetowym."
                    : ex.Message;

                await MessageBoxEx.ShowMessageAsync(new MessageInfo("Odczytywanie geolokalizacji", errorMessage), this, DialogCoordinator);
            }
        }

        /// <summary>
        /// Confirms adding the retrieved geolocation data and closes the search window.
        /// </summary>
        /// <param name="obj">The search view instance.</param>
        private void Add(object? obj)
        {
            if (obj is SearchGeolocationView sgv)
                sgv.DialogResult = true;
        }

        /// <summary>
        /// Cancels the operation and closes the search window.
        /// </summary>
        /// <param name="obj">The search view instance.</param>
        private void Close(object? obj)
        {
            if (obj is SearchGeolocationView sgv)
                sgv.DialogResult = false;
        }

        /// <summary>
        /// Handles the Enter key press event in the address input field and triggers the search.
        /// </summary>
        /// <param name="obj">The key event arguments.</param>
        private async void AddressSearchedKeydown(object? obj)
        {
            if (obj is KeyEventArgs args && args.Key == Key.Enter)
                await Search();
        }

        #endregion Methods
    }
}