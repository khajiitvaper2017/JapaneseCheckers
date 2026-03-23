# Japanese Checkers

Japanese Checkers is a Windows desktop game built with WPF and the MVVM pattern. It combines a simple turn-based board game with local account management, player history, bots, and optional rating-based matches.

## Game Description

This project implements a 10x10 two-player board game where players alternate placing pieces on empty cells. A player wins by creating a line of five matching pieces in a row, column, or diagonal. If the board fills before either side wins, the game ends in a draw.

The game supports:

- Human vs human matches
- Human vs bot matches
- Bot vs bot matches
- Rating-enabled games, with Elo-style rating adjustments

## Tech Stack

- .NET 6 (`net6.0-windows`)
- WPF
- MVVM architecture
- MaterialDesignInXAML `4.6.1`
- `System.Text.Json` for local persistence
- Google OAuth 2.0 for external login

## Architecture

The codebase is split into the usual WPF layers:

- `Views/` contains XAML windows and controls for the UI.
- `ViewModels/` contains presentation logic, commands, and state binding.
- `Models/` contains the game domain, persistence, and service classes.

### View Layer

The main windows are:

- `MainWindow` - landing page with entry points to login, registration, player list, game list, and new game flow.
- `LoginWindow` - username/password login and Google login.
- `RegistrationWindow` - local account registration.
- `SelectPlayersWindow` - chooses the white and black players and whether the match affects rating.
- `GameWindow` - the actual board and in-game status display.
- `PlayersListWindow` - displays bots and registered players.
- `GamesListWindow` - displays played game history.

The board itself is rendered by `Views/Controls/CheckersControl.xaml` as a 10x10 `ItemsControl` backed by the game board model.

### ViewModel Layer

The project follows MVVM with a shared base class:

- `MvvmBase` implements `INotifyPropertyChanged`.
- `RelayCommand` is the command wrapper used by buttons and board cells.
- `MainViewModel` coordinates the global app state and opens the secondary windows.
- `LoginViewModel`, `RegistrationViewModel`, `SelectPlayersViewModel`, and `CheckersViewModel` handle the corresponding flows.

### Model Layer

The domain model includes:

- `JapaneseCheckers` - core game logic, turn tracking, win detection, and surrender handling.
- `CellBoard` and `Cell` - the 10x10 board and individual cells.
- `Player` and `Bot` - player identities, ratings, and bot move calculation.
- `Game` - a finished match record.

Persistence is handled by the generic `Data<T>` base class, which loads and saves JSON files under `Data/`:

- `AccountsData`
- `PlayersData`
- `BotsData`
- `PlayedGamesData`

The service layer includes:

- `Encryption` - SHA-256 password hashing and helper encoding functions.
- `GoogleAuth` - loopback OAuth flow for signing in with Google.

## Functionality

### Authentication

- Register local accounts with username, email, and password.
- Log in with username or email.
- Log in with Google.
- Track logged-in accounts in memory during the app session.

### Players and Bots

- Registered users are added to the player list.
- Two built-in bots are included:
  - `Random Bot`
  - `Smarter Bot`
- Player selection prevents invalid matchups such as selecting the same player twice or bot vs bot.

### Gameplay

- Start a new match from the main window.
- Select white and black players before the game starts.
- Optionally mark a match as rating-based.
- Play by clicking empty cells on the board.
- The status bar shows the current turn and active player.
- Surrender is supported during a match.
- When a match ends, it is stored in the played games history.

### Ratings and History

- Rating-based games update player ratings when the match ends.
- Played matches are stored with winner, rating mode, and rating change information.
- Separate screens show player and game history in tabular form.

## Project Structure

```text
JapaneseCheckers/
  Models/
    DataClasses/
    GameClasses/
    Services/
  ViewModels/
  Views/
    Controls/
    Converters/
    Resources/
```

## Running the App

Build and run the solution in Visual Studio, or use the CLI:

```bash
dotnet restore
dotnet build JapaneseCheckers/JapaneseCheckers.csproj
dotnet run --project JapaneseCheckers/JapaneseCheckers.csproj
```

## Notes

- The app targets Windows only because it uses WPF.
- Local application data is stored as JSON files in the `Data/` directory.
- The UI uses Material Design styling and bundled image resources for the board and pieces.
