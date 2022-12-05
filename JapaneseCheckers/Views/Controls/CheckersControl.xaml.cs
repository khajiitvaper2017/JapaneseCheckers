using System.Windows.Controls;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Views;

/// <summary>
///     Логика взаимодействия для CheckersControl.xaml
/// </summary>
public partial class CheckersControl : UserControl
{
    public CheckersControl()
    {
        InitializeComponent();
        DataContext = new CheckersViewModel();
    }
}