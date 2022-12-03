using System.Windows;
using JapaneseCheckers.Models;

namespace JapaneseCheckers.ViewModels;

internal class CheckersViewModel : MvvmBase
{
    private JapaneseCheckersGame game;
    private uint turn;

    public uint Turn
    {
        get => game.Turn;
        set => Set(ref turn, value);
    }
    public CheckersViewModel()
    {
        ClickCommand = new RelayCommand(SetCell);

        SetupNewGame();
    }

    private void SetupNewGame()
    {
        Game = new JapaneseCheckersGame();
        Game.OnWin += Game_OnWin;
        Game.OnDraw += Game_OnDraw;
    }

    public JapaneseCheckersGame Game
    {
        get => game;
        set => Set(ref game, value);
    }

    public RelayCommand ClickCommand { get; }

    private void SetCell(object param)
    {
        var cell = param as Cell;
        Game.SetCell(cell);
    }

    private void Game_OnDraw()
    {
        MessageBox.Show("Ничья!");
    }

    private void Game_OnWin()
    {
        MessageBox.Show($"{Game.WhoseTurn()} победил!");
    }
}