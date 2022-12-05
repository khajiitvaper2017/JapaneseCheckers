using System;
using System.ComponentModel;
using System.Windows;

namespace JapaneseCheckers.Views;

/// <summary>
///     Логика взаимодействия для Main.xaml
/// </summary>
public partial class Main : Window
{
    public Main()
    {
        InitializeComponent();
    }

    private void Main_OnClosing(object? sender, CancelEventArgs e)
    {
        var data = DataContext as IDisposable;
        data?.Dispose();
    }
}