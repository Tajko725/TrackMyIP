using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.Views;

namespace TrackMyIP.ViewModels
{
    /// <summary>
    /// Represents the view model for searching geolocation data.
    /// Provides properties, commands, and methods to manage the search process and user interactions.
    /// Inherits from <see cref="BaseModel"/>.
    /// </summary>
    public partial class SearchGeolocationViewModel : BaseModel
    {
        #region Properties
        private readonly IIpStackService? _ipStackService;
        private readonly IMessageDialogService? _dialogService;

        /// <summary>
        /// Gets or sets the address or IP entered by the user for geolocation search.
        /// </summary>
        [ObservableProperty]
        private string? _addressSearched;

        /// <summary>
        /// Gets or sets the geolocation data retrieved from the search.
        /// </summary>
        [ObservableProperty]
        private GeolocationData? _geolocationData;

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
        public IAsyncRelayCommand? SearchCommandAsync { get; private set; }

        /// <summary>
        /// Command for adding the retrieved geolocation data.
        /// </summary>
        public IRelayCommand? AddCommand { get; private set; }

        /// <summary>
        /// Command for closing the search window.
        /// </summary>
        public IRelayCommand? CloseCommand { get; private set; }

        /// <summary>
        /// Command for handling the Enter key press during address input.
        /// </summary>
        public IAsyncRelayCommand? AddressSearchedKeydownCommandAsync { get; private set; }
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
        /// Initializes a new instance of the <see cref="SearchGeolocationViewModel" /> class.
        /// </summary>
        public SearchGeolocationViewModel()
        {
            InitializeCommands();
            InitializeButtons();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SearchGeolocationViewModel" /> class with the specified dependencies.
        /// </summary>
        /// <param name="ipStackService">The service for fetching geolocation data from the ipstack API.</param>
        /// <param name="dialogService">Service for dialogs.</param>
        public SearchGeolocationViewModel(IIpStackService ipStackService, IMessageDialogService dialogService) : this()
        {
            _ipStackService = ipStackService;
            _dialogService = dialogService;

            _dialogService.Initialize(this);
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Initializes all commands used in the search view model.
        /// </summary>
        private void InitializeCommands()
        {
            SearchCommandAsync = new AsyncRelayCommand(SearchAsync, () => CanSearch);
            AddCommand = new RelayCommand<object?>(Add, _ => CanAdd);
            CloseCommand = new RelayCommand<object?>(Close);
            AddressSearchedKeydownCommandAsync = new AsyncRelayCommand<object?>(AddressSearchedKeydownAsync);
        }

        /// <summary>
        /// Initializes all buttons used in the search view model.
        /// </summary>
        private void InitializeButtons()
        {
            SearchButton = new ButtonInfo("Wyszukaj", SearchCommandAsync!, toolTip: "Wyszukaj dane geolokalizacyjne.");
            AddButton = new ButtonInfo("Dodaj", AddCommand!, toolTip: "Dodaj dane geolokalizacyjne.");
            CloseButton = new ButtonInfo("Zamknij", CloseCommand!, toolTip: "Zamknij okno.");
        }

        /// <summary>
        /// Executes the search command by fetching geolocation data asynchronously.
        /// </summary>
        private async Task SearchAsync()
        {
            await FetchLocationAsync();
        }

        /// <summary>
        /// Fetches geolocation data from the service based on the address or IP entered.
        /// Updates the <see cref="GeolocationData"/> property with the retrieved data.
        /// </summary>
        private async Task FetchLocationAsync()
        {
            if (string.IsNullOrWhiteSpace(AddressSearched))
                return;

            try
            {
                GeolocationData = await _ipStackService!.FetchLocationAsync(AddressSearched);
                OnPropertyChanged(nameof(GeolocationData));
                AddCommand?.NotifyCanExecuteChanged();
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message.Contains("host")
                    ? "Problem z połączeniem internetowym."
                    : ex.Message;

                await _dialogService!.ShowMessageAsync(new MessageInfo("Odczytywanie geolokalizacji", errorMessage));
            }
        }

        /// <summary>
        /// Confirms adding the retrieved geolocation data and closes the search window.
        /// </summary>
        /// <param name="obj">The search view instance.</param>
        private void Add(object? obj)
        {
            if (obj is SearchGeolocationView sgv)
                Closing(sgv, true);
        }

        /// <summary>
        /// Cancels the operation and closes the search window.
        /// </summary>
        /// <param name="obj">The search view instance.</param>
        private void Close(object? obj)
        {
            if (obj is SearchGeolocationView sgv)
                Closing(sgv, false);
        }

        /// <summary>
        /// Handles the Enter key press event in the address input field by delegating
        /// the action to <see cref="OnKeyPressedAsync"/> for processing specific key behaviors.
        /// </summary>
        /// <param name="obj">The key event arguments passed from the UI.</param>
        private async Task AddressSearchedKeydownAsync(object? obj)
        {
            if (obj is KeyEventArgs args && args.Key == Key.Enter)
                await OnKeyPressedAsync(args.Key);
        }

        /// <summary>
        /// Invoked whenever the <see cref="AddressSearched"/> property value changes.
        /// Updates the execution state of the <see cref="SearchCommandAsync"/> command based on the new value.
        /// </summary>
        /// <param name="value">The new value of the <see cref="AddressSearched"/> property.</param>
        partial void OnAddressSearchedChanged(string? value)
        {
            SearchCommandAsync?.NotifyCanExecuteChanged();
        }

        /// <summary>
        /// Handles specific key press events and performs the associated actions.
        /// For example, when the Enter key is pressed, it triggers the search functionality.
        /// </summary>
        /// <param name="key">The key that was pressed.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task OnKeyPressedAsync(Key key)
        {
            if (key == Key.Enter)
                await SearchAsync();
        }

        /// <summary>
        /// Handles the closing of the <see cref="SearchGeolocationView"/> dialog and resets the dialog context to the default context.
        /// </summary>
        /// <param name="view">The <see cref="SearchGeolocationView"/> instance that is being closed.</param>
        /// <param name="dialogResultValue">The value to set as the dialog result.</param>
        private void Closing(SearchGeolocationView view, bool dialogResultValue)
        {
            view.DialogResult = dialogResultValue;

            // Set default MainWindowViewModel as context
            _dialogService!.Initialize(App.Current.MainWindow.DataContext);
        }
        #endregion Methods
    }
}