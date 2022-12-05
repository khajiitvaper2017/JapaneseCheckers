using System.Windows;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Views;

/// <summary>
///     Логика взаимодействия для GamesListWindow.xaml
/// </summary>
public partial class GamesListWindow : Window
{
    public GamesListWindow()
    {
        InitializeComponent();
    }

    public GamesListWindow(MainViewModel mvm) : this()
    {
        DataContext = mvm;
    }
}