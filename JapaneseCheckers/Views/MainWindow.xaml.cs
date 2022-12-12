using System;
using System.ComponentModel;
using System.Windows;

namespace JapaneseCheckers.Views;

/// <summary>
///     Логика взаимодействия для MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void Main_OnClosing(object? sender, CancelEventArgs e)
    {
        var data = DataContext as IDisposable;
        data?.Dispose();
    }
}