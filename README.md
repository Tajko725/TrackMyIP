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
![Główny ekran](https://github.com/user-attachments/assets/e8aee286-3b55-4f4f-ad62-5295159a59ac)


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
- [Microsoft.EntityFrameworkCore.InMemory](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.InMemory/9.0.1) - Dostawca bazy danych in-memory dla Entity Framework Core (do wykorzystania w celach testowych).
- [CommunityToolkit.Mvvm](https://learn.microsoft.com/pl-pl/dotnet/communitytoolkit/mvvm/) - Szybka, modułowa, platformowa biblioteka MVVM, która jest oficjalnym następcą MVVMlight. Posiada Dependency Injection (Ioc), klasy RelayCommand, ObservableProperty, ObservableObject itp.
- [Newtonsoft.Json](https://www.newtonsoft.com/json) - Przetwarzanie i parsowanie danych JSON.
- [FluentAssertions](https://xceed.com/products/unit-testing/fluent-assertions/) - Fluent Assertions oferuje kompleksowy zestaw metod rozszerzeń, które umożliwiają naturalne wyrażanie oczekiwanych wyników testów jednostkowych TDD (programowanie sterowane testami) lub BDD (rozwój sterowany zachowaniem).
- [FluentAssertions.Analyzers](https://github.com/fluentassertions/fluentassertions.analyzers) - Analizatory pomagające w pisaniu płynnych twierdzeń we właściwy sposób.
- [Moq](https://github.com/devlooped/moq) - Moq to najpopularniejszy i najbardziej przyjazny framework mockingowy dla .NET.
- [xunit](https://www.nuget.org/packages/xunit/2.9.3) - xUnit.net jest frameworkiem testowania deweloperskiego, zbudowanym w celu wsparcia Test Driven Development, z celem projektowym ekstremalnej prostoty i dostosowania do funkcji frameworka.

## 🖥️ Instalacja i konfiguracja

1. **Pobierz projekt**:
   - Sklonuj repozytorium lub pobierz jako plik ZIP.
   ```bash
   git clone https://github.com/Tajko725/TrackMyIP
