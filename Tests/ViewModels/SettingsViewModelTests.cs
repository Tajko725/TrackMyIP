using FluentAssertions;
using MahApps.Metro.Controls.Dialogs;
using Moq;
using TrackMyIP.Models;
using TrackMyIP.Services.Interfaces;
using TrackMyIP.ViewModels;

namespace Tests.ViewModels
{
    public class SettingsViewModelTests
    {
        private readonly Mock<IIpStackService> _ipStackServiceMock;
        private readonly Mock<IMessageDialogService> _dialogServiceMock;
        private readonly Mock<IUrlNavigatorService> _iUrlNavigatorServiceMock;
        private readonly SettingsViewModel _settingsViewModel;

        public SettingsViewModelTests()
        {
            _ipStackServiceMock = new Mock<IIpStackService>();
            _dialogServiceMock = new Mock<IMessageDialogService>();
            _iUrlNavigatorServiceMock = new Mock<IUrlNavigatorService>();
            _settingsViewModel = new SettingsViewModel(_ipStackServiceMock.Object, _dialogServiceMock.Object, _iUrlNavigatorServiceMock.Object);
        }

        [Fact]
        public void LoadCommand_ShouldLoadSettings()
        {
            // Arrange
            AppConfigHelper.UpdateAppSetting("IPStackApiKey", "loaded-api-key");

            // Act
            _settingsViewModel.LoadCommand!.Execute(null);

            // Assert
            _settingsViewModel.IpStackApiKey.Should().Be("loaded-api-key");
        }

        [Fact]
        public async Task SaveCommandAsync_ShouldUpdateAppSettings_WhenExecuted()
        {
            // Arrange
            _settingsViewModel.IpStackApiKey = "new-api-key";
            _dialogServiceMock
                .Setup(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(MessageDialogResult.Affirmative));

            // Act
            await _settingsViewModel.SaveCommandAsync!.ExecuteAsync(null);

            // Assert
            AppConfigHelper.GetAppSetting("IPStackApiKey").Should().Be("new-api-key");
            _dialogServiceMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public void SaveCommandAsync_CannotExecute_WhenApiKeyIsNullOrWhitespace()
        {
            // Arrange
            _settingsViewModel.IpStackApiKey = string.Empty;

            // Act
            var canExecute = _settingsViewModel.SaveCommandAsync!.CanExecute(null);

            // Assert
            canExecute.Should().BeFalse();
        }

        [Fact]
        public async Task CheckApiKeyIsValidCommandAsync_ShouldShowMessage_WhenApiKeyIsValid()
        {
            // Arrange
            _ipStackServiceMock
                .Setup(x => x.ValidateApiKeyAsync())
                .ReturnsAsync("Podano prawidłowy klucz");
            _dialogServiceMock
                .Setup(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(MessageDialogResult.Affirmative));

            // Act
            await _settingsViewModel.CheckApiKeyIsValidCommandAsync!.ExecuteAsync(null);

            // Assert
            _dialogServiceMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), "Podano prawidłowy klucz"), Times.Once);
        }

        [Fact]
        public async Task CheckApiKeyIsValidCommandAsync_ShouldShowErrorMessage_WhenApiKeyIsInvalid()
        {
            // Arrange
            _ipStackServiceMock
                .Setup(x => x.ValidateApiKeyAsync())
                .ReturnsAsync("Nieprawidłowy klucz API.");
            _dialogServiceMock
                .Setup(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(MessageDialogResult.Affirmative));

            // Act
            await _settingsViewModel.CheckApiKeyIsValidCommandAsync!.ExecuteAsync(null);

            // Assert
            _dialogServiceMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), "Nieprawidłowy klucz API."), Times.Once);
        }

        [Fact]
        public async Task CheckApiKeyIsValidCommandAsync_ShouldShowExceptionMessage_WhenExceptionOccurs()
        {
            // Arrange
            _ipStackServiceMock
                .Setup(x => x.ValidateApiKeyAsync())
                .ThrowsAsync(new Exception("Błąd sieci"));
            _dialogServiceMock
                .Setup(x => x.ShowMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(MessageDialogResult.Affirmative));
            _settingsViewModel.IpStackApiKey = "valid-api-key";
            _settingsViewModel.IsChanged = false;

            // Act
            await _settingsViewModel.CheckApiKeyIsValidCommandAsync!.ExecuteAsync(null);

            // Assert
            _dialogServiceMock.Verify(x => x.ShowMessageAsync(It.IsAny<string>(), "Błąd podczas odczytywania informacji:\nBłąd sieci"), Times.Once);
        }

        [Fact]
        public void GoToWwwCommand_ShouldOpenUrl_WhenExecuted()
        {
            // Arrange
            string validUrl = "https://www.google.com";

            // Act
            _settingsViewModel.GoToWwwCommand!.Execute(validUrl);

            // Assert
            _iUrlNavigatorServiceMock.Verify(x => x.OpenUrl(validUrl), Times.Once);
        }

        [Fact]
        public void GoToWwwCommand_ShouldNotOpenUrl_WhenUrlIsEmpty()
        {
            // Arrange
            string invalidUrl = string.Empty;
            _iUrlNavigatorServiceMock
                .Setup(x => x.OpenUrl(It.IsAny<string>()))
                .Throws(new ArgumentException("URL nie może być nullem lub pusty."));

            // Act
            var act = () => _settingsViewModel.GoToWwwCommand!.Execute(invalidUrl);

            // Assert
            _iUrlNavigatorServiceMock.Verify(x => x.OpenUrl(invalidUrl), Times.Never);
            act.Should().Throw<ArgumentException>()
                .WithMessage("URL nie może być nullem lub pusty.");
        }

        [Fact]
        public void GoToWwwCommand_ShouldNotOpenUrl_WhenUrlIsInvalid()
        {
            // Arrange
            string invalidUrl = "invalid-url";
            _iUrlNavigatorServiceMock
                .Setup(x => x.OpenUrl(It.IsAny<string>()))
                .Throws(new ArgumentException("Niepoprawny URL."));

            // Act
            var act = () => _settingsViewModel.GoToWwwCommand!.Execute(invalidUrl);

            // Assert
            _iUrlNavigatorServiceMock.Verify(x => x.OpenUrl(invalidUrl), Times.Never);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Niepoprawny URL.");
        }
    }
}