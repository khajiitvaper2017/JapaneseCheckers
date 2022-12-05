using System.Text.RegularExpressions;
using System.Windows.Controls;
using JapaneseCheckers.Views;

namespace JapaneseCheckers.ViewModels;

internal class RegistrationViewModel : MvvmBase
{
    private string email;
    public Regex EmailRegex = new("^[\\w-\\.]+@([\\w-]+\\.)+[\\w-]{2,4}$");
    private string username;


    public RegistrationViewModel()
    {
        Register = new RelayCommand(RegisterClick);
    }

    public string Username
    {
        get => username;
        set => Set(ref username, value);
    }

    public string Email
    {
        get => email;
        set => Set(ref email, value);
    }

    public RelayCommand Register { get; }

    private void RegisterClick(object parameters)
    {
        var values = (object[])parameters;
        var password = (values[0] as PasswordBox).Password;
        var passwordConfirmation = (values[1] as PasswordBox).Password;

        var message = ("", "");

        if (password is null or "" || passwordConfirmation is null or "")
            message = ("Wrong password", "Password cannot be empty");
        else if (Username.Length is < 3 or > 25)
            message = ("Wrong username", "Username must be bigger than 3 symbols and less than 25");

        else if (!EmailRegex.IsMatch(Email))
            message = ("Wrong email", "Please, enter correct email");

        else if (password?.Length < 8)
            message = ("Wrong password", "Password must be longer than 8 symbols");
        else if (password != passwordConfirmation) message = ("Wrong password", "Passwords are not equal");

        if (message != ("", ""))
        {
            new MessageWindow(message.Item1, message.Item2).ShowDialog();
            return;
        }

        var success = MainViewModel.AccountsData.AddAccount(Username, Email, password);
        if (success)
        {
            new MessageWindow("Successful registration", $"Thank you for registration, {Username}").ShowDialog();

            MainViewModel.LoggedAccounts.Add(MainViewModel.AccountsData.GetAccount(Username)!);
            MainViewModel.PlayersData.AddPlayer(Username);
        }
        else
        {
            new MessageWindow("Wrong data", "Account with that data already exists").ShowDialog();
        }
    }
}