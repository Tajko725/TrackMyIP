using MahApps.Metro.Controls.Dialogs;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using TrackMyIP.Models;
using TrackMyIP.Services;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.Views;

namespace TrackMyIP.ViewModels
{
    /// <summary>
    /// Represents the main view model for the application.
    /// Provides properties, commands, and methods to manage the application state, geolocation data, and user interactions.
    /// Inherits from <see cref="BaseModel"/>.
    public class MainViewModel : BaseModel
    {
        #region Properties
        private bool _isBusy = false;

        /// <summary>
        /// Gets or sets the dialog coordinator for displaying dialogs in the application.
        /// </summary>
        public IDialogCoordinator DialogCoordinator { get; } = null!;
        private readonly IGeolocationService _geolocationService = null!;

        /// <summary>
        /// Gets or sets the collection of geolocation data displayed in the application.
        /// </summary>
        public required ObservableCollection<GeolocationData> Geolocations { get; set; } = [];

        private GeolocationData? _selectedGeolocation;
        /// <summary>
        /// Gets or sets the currently selected geolocation data.
        /// </summary>
        public GeolocationData? SelectedGeolocation
        {
            get => _selectedGeolocation;
            set
            {
                if (_selectedGeolocation != value)
                {

                    _selectedGeolocation = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _showGeolocationsVisibility = Visibility.Visible;
        /// <summary>
        /// Gets or sets the visibility state for the geolocation view.
        /// </summary>
        public Visibility ShowGeolocationsVisibility
        {
            get => _showGeolocationsVisibility;
            set
            {
                if (value != _showGeolocationsVisibility)
                {
                    _showGeolocationsVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        private Visibility _showSettingsVisibility = Visibility.Collapsed;
        /// <summary>
        /// Gets or sets the visibility state for the settings view.
        /// </summary>
        public Visibility ShowSettingsVisibility
        {
            get => _showSettingsVisibility;
            set
            {
                if (value != _showSettingsVisibility)
                {
                    _showSettingsVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Gets or sets the view model for the settings view.
        /// </summary>
        public SettingsViewModel? SettingsViewModel { get; }
        #endregion Properties

        #region Commands
        /// <summary>
        /// Command for showing the geolocation search view.
        /// </summary>
        public ICommand? ShowSearchingGeolocationCommand { get; private set; }

        /// <summary>
        /// Command for showing the geolocation list view.
        /// </summary>
        public ICommand? ShowGeolocationsCommand { get; private set; }

        /// <summary>
        /// Command for showing the settings view.
        /// </summary>
        public ICommand? ShowSettingsCommand { get; private set; }

        /// <summary>
        /// Command for closing the application.
        /// </summary>
        public ICommand? CloseCommand { get; private set; }

        /// <summary>
        /// Command for updating the selected geolocation.
        /// </summary>
        public ICommand? UpdateGeolocationCommand { get; private set; }

        /// <summary>
        /// Command for deleting the selected geolocation.
        /// </summary>
        public ICommand? DeleteGeolocationCommand { get; private set; }

        /// <summary>
        /// Command for refreshing the list of geolocations.
        /// </summary>
        public ICommand? RefreshGeoocationsCommand { get; private set; }
        #endregion Commands

        #region Buttons
        /// <summary>
        /// Button for adding a geolocation.
        /// </summary>
        public ButtonInfo? AddGeolocationButton { get; private set; }

        /// <summary>
        /// Button for deleting a geolocation.
        /// </summary>
        public ButtonInfo? DeleteGeolocationButton { get; private set; }

        /// <summary>
        /// Button for updating a geolocation.
        /// </summary>
        public ButtonInfo? UpdateGeolocationButton { get; private set; }

        /// <summary>
        /// Button for refreshing the geolocations list.
        /// </summary>
        public ButtonInfo? RefreshGeolocationsButton { get; private set; }

        /// <summary>
        /// Button for showing the geolocation search view.
        /// </summary>
        public ButtonInfo? ShowSearchingGeolocationButton { get; private set; }

        /// <summary>
        /// Button for showing the geolocations list view.
        /// </summary>
        public ButtonInfo? ShowGeolocationsButton { get; private set; }

        /// <summary>
        /// Button for showing the settings view.
        /// </summary>
        public ButtonInfo? ShowSettingsButton { get; private set; }

        /// <summary>
        /// Button for closing the application.
        /// </summary>
        public ButtonInfo? CloseButton { get; private set; }
        #endregion Buttons

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        public MainViewModel()
        {
            Initialize();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel" /> class.
        /// </summary>
        /// <param name="geolocationService">Service for managing geolocation data.</param>
        /// <param name="dialogCoordinator">Dialog coordinator for displaying dialogs in the application.</param>
        /// <param name="settingsViewModel">ViewModel for managing application settings.</param>
        public MainViewModel(IGeolocationService geolocationService, IDialogCoordinator dialogCoordinator, SettingsViewModel settingsViewModel) : this()
        {
            _geolocationService = geolocationService;
            DialogCoordinator = dialogCoordinator;
            SettingsViewModel = settingsViewModel;

            ShowGeolocations(null);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Initializes the main view model, including commands, buttons, and data services.
        /// </summary>
        private void Initialize()
        {
            InitializeCommands();
            InitializeButtons();
        }

        /// <summary>
        /// Initializes all commands used in the main view model.
        /// </summary>
        private void InitializeCommands()
        {
            ShowSearchingGeolocationCommand = new RelayCommand(ShowShowSearchingGeolocation);
            ShowGeolocationsCommand = new RelayCommand(ShowGeolocations, x => !_isBusy);
            ShowSettingsCommand = new RelayCommand(ShowSettings, x => !_isBusy);
            CloseCommand = new RelayCommand(Close);

            UpdateGeolocationCommand = new RelayCommand(async _ => await EditLocationAsync(), _ => SelectedGeolocation != null && !_isBusy);
            DeleteGeolocationCommand = new RelayCommand(async _ => await DeleteGeolocationAsync(), _ => SelectedGeolocation != null && !_isBusy);
            RefreshGeoocationsCommand = new RelayCommand(async _ => await LoadGeolocationsAsync(), _ => !_isBusy);
        }

        /// <summary>
        /// Initializes all button configurations used in the main view model.
        /// </summary>
        private void InitializeButtons()
        {
            ShowSearchingGeolocationButton = new ButtonInfo("Wyszukaj geolokalizację", ShowSearchingGeolocationCommand!, null!, "Przejdź do wyszukiwania danych geolokalizacyjnych.");
            ShowGeolocationsButton = new ButtonInfo("Geolokalizacje", ShowGeolocationsCommand!, null!, "Wyświetl dane geolokalizacyjne z bazy danych.");
            ShowSettingsButton = new ButtonInfo("Ustawienia", ShowSettingsCommand!, null!, "Przejdź do ustawień.");
            CloseButton = new ButtonInfo("Zamknij", CloseCommand!, null!, "Zamknij aplikację.");

            DeleteGeolocationButton = new ButtonInfo("Usuń", DeleteGeolocationCommand!);
            UpdateGeolocationButton = new ButtonInfo("Aktualizuj", UpdateGeolocationCommand!);
            RefreshGeolocationsButton = new ButtonInfo("Odśwież", RefreshGeoocationsCommand!);
        }

        /// <summary>
        /// Loads the list of geolocations asynchronously.
        /// </summary>
        private async Task LoadGeolocationsAsync()
        {
            var locations = await _geolocationService!.GetAllAsync();
            Geolocations.Clear();
            foreach (var location in locations)
                Geolocations.Add(location);
        }

        /// <summary>
        /// Displays the geolocation search view and adds new geolocation data if confirmed.
        /// </summary>
        /// <param name="obj">Optional parameter for the command.</param>
        private async void ShowShowSearchingGeolocation(object? obj)
        {
            var sgv = ServiceLocator.Services.GetService<SearchGeolocationView>();
            if (sgv!.ShowDialog()!.Value && sgv.DataContext is SearchGeolocationViewModel vm)
            {
                if (Geolocations.Any(x => x.IP == vm.GeolocationData!.IP))
                {
                    await MessageBoxEx.ShowMessageAsync(new MessageInfo("Dodawanie geolokalizacji", "Geolokalizacja o podanym adresie IP/URL już istnieje w bazie danych."), this, DialogCoordinator);
                    return;
                }

                await AddLocationAsync(vm.GeolocationData!);
            }
        }

        /// <summary>
        /// Displays the geolocations list view and loads data from the database.
        /// </summary>
        /// <param name="obj">Optional parameter for the command.</param>
        private async void ShowGeolocations(object? obj)
        {
            ShowGeolocationsVisibility = Visibility.Visible;
            ShowSettingsVisibility = Visibility.Collapsed;

            await LoadGeolocationsAsync();
        }

        /// <summary>
        /// Displays the settings view.
        /// </summary>
        /// <param name="obj">Optional parameter for the command.</param>
        private void ShowSettings(object? obj)
        {
            ShowSettingsVisibility = Visibility.Visible;
            ShowGeolocationsVisibility = Visibility.Collapsed;
        }

        /// <summary>
        /// Closes the main application window.
        /// </summary>
        /// <param name="obj">The main window object to close.</param>
        private void Close(object? obj)
        {
            if (obj is MainView mw)
                mw.Close();
        }

        /// <summary>
        /// Adds a new geolocation to the database and updates the collection.
        /// </summary>
        /// <param name="geolocationData">The geolocation data to add.</param>
        private async Task AddLocationAsync(GeolocationData geolocationData)
        {
            if (_isBusy)
                return;

            _isBusy = true;
            await _geolocationService!.AddAsync(geolocationData);
            Geolocations.Add(geolocationData);
            _isBusy = false;
        }

        /// <summary>
        /// Updates the selected geolocation in the database and the collection.
        /// </summary>
        private async Task EditLocationAsync()
        {
            if (_isBusy)
                return;

            _isBusy = true;
            await _geolocationService!.UpdateAsync(SelectedGeolocation!);

            var itemToUpdate = Geolocations.FirstOrDefault(l => l.Id == SelectedGeolocation!.Id);
            if (itemToUpdate != null)
            {
                itemToUpdate.IP = SelectedGeolocation!.IP;
                itemToUpdate.Country = SelectedGeolocation.Country;
                itemToUpdate.Region = SelectedGeolocation.Region;
                itemToUpdate.City = SelectedGeolocation.City;
                itemToUpdate.Latitude = SelectedGeolocation.Latitude;
                itemToUpdate.Longitude = SelectedGeolocation.Longitude;

                OnPropertyChanged(nameof(Geolocations));
            }

            _isBusy = false;
        }

        /// <summary>
        /// Deletes the selected geolocation from the database and updates the collection.
        /// </summary>
        private async Task DeleteGeolocationAsync()
        {
            if (_isBusy)
                return;

            var result = await MessageBoxEx.ShowMessageAsync(new MessageInfo("Usuwanie geolokalizacji", "Czy na pewno usunąć wybraną geolokalizację?"), MessageDialogStyle.AffirmativeAndNegative, this, DialogCoordinator!);
            if (result == MessageDialogResult.Negative)
                return;

            _isBusy = true;
            await _geolocationService!.DeleteAsync(SelectedGeolocation!.Id);

            var itemToRemove = Geolocations.FirstOrDefault(l => l.Id == SelectedGeolocation.Id);
            if (itemToRemove != null)
                Geolocations.Remove(itemToRemove);

            _isBusy = false;
        }
        #endregion Methods
    }
}