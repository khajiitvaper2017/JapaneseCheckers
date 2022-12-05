using System;
using System.ComponentModel;
using JapaneseCheckers.Models;
using JapaneseCheckers.Views;

namespace JapaneseCheckers.ViewModels;

internal class CheckersViewModel : MvvmBase
{
    private Player currentPlayer;
    private string exitBtnText;
    private Models.JapaneseCheckers game;

    private string status;

    public CheckersViewModel()
    {
        ClickCommand = new RelayCommand(SetCell, obj => !game.IsGameEnded);
        ExitCommand = new RelayCommand(Exit);
        SetupNewGame();
        game.PropertyChanged += Game_PropertyChanged;
        CurrentPlayer = FirstPlayer;
        ExitBtnText = "Surrender";
    }

    public bool PlayedOnRating { get; set; }
    public Player FirstPlayer { get; set; }
    public Player SecondPlayer { get; set; }

    public Player CurrentPlayer
    {
        get => currentPlayer;
        set => Set(ref currentPlayer, value);
    }

    public string Status
    {
        get => status;
        set => Set(ref status, value);
    }

    public string ExitBtnText
    {
        get => exitBtnText;
        set => Set(ref exitBtnText, value);
    }

    public Models.JapaneseCheckers Game
    {
        get => game;
        set => Set(ref game, value);
    }

    public RelayCommand ClickCommand { get; }
    public RelayCommand ExitCommand { get; }

    private void Exit(object obj)
    {
        var win = obj as GameWindow;
        switch (ExitBtnText)
        {
            case "Surrender":
            {
                var result = new MessageWindow("Surrender", "Are you sure you want to surrender?",
                        "Yes", "No")
                    .ShowDialog();
                if (result != true) return;
                game.Surrender();
                win.DialogResult = true;
                break;
            }
            case "Exit":
                win.DialogResult = false;

                break;
        }
    }

    public void Update()
    {
        Game_PropertyChanged(this, new PropertyChangedEventArgs("Update"));
        if (!CurrentPlayer.IsBot) return;

        var bot = CurrentPlayer as Bot;
        var move = bot.CalculateMove(Game.Board);
        SetCell(game.Board[move.Item1, move.Item2]);
    }

    private void Game_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName is not "Turn" and not "Update") return;
        var turn = game.WhoseTurn();
        CurrentPlayer = turn == Color.White ? FirstPlayer : SecondPlayer;
        if (game.Turn == 0)
        {
            Status = $"Game started. First move makes {CurrentPlayer.Username}";
            return;
        }

        Status = $"Turn: {game.Turn + 1}. Next move makes: {CurrentPlayer.Username}";
    }

    private void SetupNewGame()
    {
        Game = new Models.JapaneseCheckers();
        Game.GameEnded += Game_GameEnded;
    }

    private void Game_GameEnded()
    {
        var prevRating = game.Result == Color.White ? FirstPlayer.Rating : SecondPlayer.Rating;
        var endedGame = new Game(FirstPlayer, SecondPlayer, game.Result, PlayedOnRating);
        MainViewModel.PlayedGamesData.AddGame(endedGame);
        Status = game.Result switch
        {
            Color.White => $"{FirstPlayer.Username} won! His rating increased by {FirstPlayer.Rating - prevRating}",
            Color.Black => $"{SecondPlayer.Username} won! His rating increased by {SecondPlayer.Rating - prevRating}",
            Color.None => "Draw!",
            _ => throw new ArgumentOutOfRangeException()
        };
        ExitBtnText = "Exit";
    }

    private void SetCell(object param)
    {
        if (game.IsGameEnded) return;
        var cell = param as Cell;
        Game.SetCell(cell);

        if (!CurrentPlayer.IsBot) return;

        var bot = CurrentPlayer as Bot;
        var move = bot.CalculateMove(game.Board);
        SetCell(game.Board[move.Item1, move.Item2]);
    }
}