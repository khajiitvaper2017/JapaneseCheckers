using System.Windows;
using System.Windows.Controls;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Views;

/// <summary>
///     Логика взаимодействия для PlayersListWindow.xaml
/// </summary>
public partial class PlayersListWindow : Window
{
    public PlayersListWindow()
    {
        InitializeComponent();
    }

    public PlayersListWindow(MainViewModel mvm) : this()
    {
        DataContext = mvm;
    }

    private void DataGrid_OnAutoGeneratingColumn(object? sender, DataGridAutoGeneratingColumnEventArgs e)
    {
        if (e.PropertyName == "IsBot") e.Cancel = true;
    }
}