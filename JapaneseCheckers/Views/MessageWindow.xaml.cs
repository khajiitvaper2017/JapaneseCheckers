using System.Windows;

namespace JapaneseCheckers.Views;

/// <summary>
///     Interaction logic for MessageInstance.xaml
/// </summary>
public partial class MessageWindow : Window
{
    public MessageWindow(string header, string message, string okButtonText = "OK", string cancelButtonText = "Cancel")
    {
        Header = header;
        Message = message;
        OkButtonText = okButtonText;
        CancelButtonText = cancelButtonText;

        InitializeComponent();
    }

    public string Message { get; set; }
    public string Header { get; set; }
    public string OkButtonText { get; set; }
    public string CancelButtonText { get; set; }

    private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        DialogResult = true;
    }
}