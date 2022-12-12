using System;
using System.Collections.ObjectModel;
using JapaneseCheckers.Models.DataClasses;
using JapaneseCheckers.Views;

namespace JapaneseCheckers.ViewModels;

public class MainViewModel : MvvmBase, IDisposable
{
    public MainViewModel()
    {
        Login = new RelayCommand(LoginClick);
        Registration = new RelayCommand(RegistrationClick);
        DisplayGamesList = new RelayCommand(DisplayGamesListClick);
        DisplayPlayersList = new RelayCommand(DisplayPlayersListClick);
        StartNewCommand = new RelayCommand(StartNewClick, _ => LoggedAccounts.Count > 0);
    }

    public static AccountsData AccountsData { get; set; } = new();
    public static PlayersData PlayersData { get; set; } = new();
    public static BotsData BotsData { get; set; } = new();
    public static PlayedGamesData PlayedGamesData { get; set; } = new();
    public static ObservableCollection<Account> LoggedAccounts { get; set; } = new();

    public RelayCommand Login { get; }
    public RelayCommand Registration { get; }
    public RelayCommand StartNewCommand { get; }
    public RelayCommand DisplayPlayersList { get; }
    public RelayCommand DisplayGamesList { get; }

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }

    private void DisplayPlayersListClick(object obj)
    {
        new PlayersListWindow(this).ShowDialog();
    }

    private void DisplayGamesListClick(object obj)
    {
        new GamesListWindow(this).ShowDialog();
    }


    private void StartNewClick(object obj)
    {
        var result = new SelectPlayersWindow();
        if (result.ShowDialog() == false) return;

        var data = result.DataContext as SelectPlayersViewModel;
        var first = data.FirstPlayer;
        var second = data.SecondPlayer;
        var onRating = data.OnRating;
        var game = new GameWindow();
        var gameData = game.DataContext as CheckersViewModel;
        gameData.FirstPlayer = first;
        gameData.SecondPlayer = second;
        gameData.PlayedOnRating = onRating;
        gameData.Update();
        game.ShowDialog();
    }

    private void LoginClick(object sender)
    {
        new LoginWindow().ShowDialog();
    }

    private void RegistrationClick(object sender)
    {
        new RegistrationWindow().ShowDialog();
    }

    private void ReleaseUnmanagedResources()
    {
        AccountsData.Dispose();
        PlayersData.Dispose();
        PlayedGamesData.Dispose();
        BotsData.Dispose();
    }

    ~MainViewModel()
    {
        ReleaseUnmanagedResources();
    }
}