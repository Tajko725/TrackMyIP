# TrackMyIP

**TrackMyIP** to aplikacja desktopowa stworzona w technologii **WPF (.NET 9)**, która umożliwia wyszukiwanie, zarządzanie i przechowywanie danych geolokalizacyjnych na podstawie adresów IP lub URL. Aplikacja wykorzystuje publiczne API [ipstack](https://ipstack.com) do uzyskiwania informacji geolokalizacyjnych oraz lokalną bazę danych SQLite do przechowywania wyników.

## 📋 Funkcje

- **Wyszukiwanie geolokalizacji**:
  - Wprowadź adres IP lub URL, aby uzyskać dane geolokalizacyjne, takie jak kraj, region, miasto, szerokość i długość geograficzna.
- **Zarządzanie danymi geolokalizacyjnymi**:
  - Dodawaj, edytuj i usuwaj zapisane dane w lokalnej bazie SQLite.
- **Walidacja klucza API**:
  - Możliwość sprawdzenia poprawności klucza API w ustawieniach aplikacji.
- **Intuicyjny interfejs użytkownika**:
  - Oparty na bibliotece MahApps.Metro z obsługą wzorca MVVM, zapewniający czytelność i prostotę obsługi.

## 🖥️ Zrzuty ekranu

### Główny ekran aplikacji
Ekran umożliwiający przeglądanie zapisanych danych geolokalizacyjnych oraz dodawanie nowych lokalizacji:
![Główny ekran](https://github.com/user-attachments/assets/e6991c92-b329-4781-9ea3-37fbd4bfa5cb)

### Wyszukiwanie geolokalizacji
Ekran umożliwia wyszukiwanie informacji geolokalizacyjnych na podstawie wprowadzonego adresu IP / URL. Pobierana jest cząstka informacji.
![Wyszukiwanie_geolokalizacji](https://github.com/user-attachments/assets/c99154cc-bea5-4dd9-bd84-38d8c0c2f442)
![image](https://github.com/user-attachments/assets/ed8d66cf-1f22-4fbb-aa8f-607366704ecf)

## 🛠 Wymagania

1. **.NET 9**: Upewnij się, że masz zainstalowane środowisko .NET 9.
2. **Konto na ipstack**: 
   - Zarejestruj się na stronie [ipstack.com](https://ipstack.com).
   - Uzyskaj własny klucz API, który będzie wymagany do działania aplikacji.
3. **SQLite**: Aplikacja korzysta z lokalnej bazy danych SQLite, która zostanie automatycznie utworzona przy pierwszym uruchomieniu.

## 📦 Użyte biblioteki

TrackMyIP wykorzystuje następujące biblioteki:

- [MahApps.Metro](https://github.com/MahApps/MahApps.Metro) - Stylizacja i ulepszenie interfejsu użytkownika.
- [Microsoft.EntityFrameworkCore](https://learn.microsoft.com/en-us/ef/core/) - Dostęp do bazy danych w modelu ORM.
- [Microsoft.EntityFrameworkCore.Sqlite](https://learn.microsoft.com/en-us/ef/core/providers/sqlite/?tabs=dotnet-cli) - Obsługa bazy SQLite.
- [Microsoft.EntityFrameworkCore.Tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) - Narzędzia CLI dla Entity Framework.
- [Microsoft.Xaml.Behaviors.Wpf](https://www.nuget.org/packages/Microsoft.Xaml.Behaviors.Wpf) - Obsługa zachowań w WPF.
- [Newtonsoft.Json](https://www.newtonsoft.com/json) - Przetwarzanie i parsowanie danych JSON.

## 🖥️ Instalacja i konfiguracja

1. **Pobierz projekt**:
   - Sklonuj repozytorium lub pobierz jako plik ZIP.
   ```bash
   git clone https://github.com/your-repo-name/TrackMyIP.git
