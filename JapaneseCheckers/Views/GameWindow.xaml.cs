using System.ComponentModel;
using System.Windows;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Views;

public partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();
    }

    private void GameWindow_OnClosing(object? sender, CancelEventArgs e)
    {
        var data = DataContext as CheckersViewModel;
        if (data?.ExitBtnText == "Exit")
            return;
        e.Cancel = true;
        data.ExitCommand.Execute(this);
    }
}