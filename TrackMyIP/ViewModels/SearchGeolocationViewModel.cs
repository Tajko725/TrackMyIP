﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MahApps.Metro.Controls.Dialogs;
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
        /// <summary>
        /// Gets or sets the dialog coordinator for displaying dialogs in the application.
        /// </summary>
        public readonly IDialogCoordinator? DialogCoordinator;
        private readonly IIpStackService? _ipStackService;

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
        public IRelayCommand? SearchCommand { get; private set; }

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
        public IRelayCommand? AddressSearchedKeydownCommand { get; private set; }
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
        /// <param name="dialogCoordinator">The dialog coordinator used for displaying dialogs in the application.</param>
        public SearchGeolocationViewModel(IIpStackService ipStackService, IDialogCoordinator dialogCoordinator) : this()
        {
            _ipStackService = ipStackService;
            DialogCoordinator = dialogCoordinator;
        }
        #endregion Constructors

        #region Methods
        /// <summary>
        /// Initializes all commands used in the search view model.
        /// </summary>
        private void InitializeCommands()
        {
            SearchCommand = new AsyncRelayCommand(SearchAsync, () => CanSearch);
            AddCommand = new RelayCommand<object?>(Add, _ => CanAdd);
            CloseCommand = new RelayCommand<object?>(Close);
            AddressSearchedKeydownCommand = new AsyncRelayCommand<object?>(AddressSearchedKeydownAsync);
        }

        /// <summary>
        /// Initializes all buttons used in the search view model.
        /// </summary>
        private void InitializeButtons()
        {
            SearchButton = new ButtonInfo("Wyszukaj", SearchCommand!, toolTip: "Wyszukaj dane geolokalizacyjne.");
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
        private async Task AddressSearchedKeydownAsync(object? obj)
        {
            if (obj is KeyEventArgs args && args.Key == Key.Enter)
                await SearchAsync();
        }

        /// <summary>
        /// Invoked whenever the <see cref="AddressSearched"/> property value changes.
        /// Updates the execution state of the <see cref="SearchCommand"/> command based on the new value.
        /// </summary>
        /// <param name="value">The new value of the <see cref="AddressSearched"/> property.</param>
        partial void OnAddressSearchedChanged(string? value)
        {
            SearchCommand?.NotifyCanExecuteChanged();
        }

        #endregion Methods
    }
}