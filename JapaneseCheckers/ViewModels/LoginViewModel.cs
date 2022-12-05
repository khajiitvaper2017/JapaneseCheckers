using System.ComponentModel;
using System.Linq;
using System.Windows.Controls;
using JapaneseCheckers.Models;
using JapaneseCheckers.Views;

namespace JapaneseCheckers.ViewModels;

internal class LoginViewModel : MvvmBase
{
    private string username = "";

    public LoginViewModel()
    {
        Login = new RelayCommand(LoginButtonClick);
        GoogleLogin = new RelayCommand(GoogleLoginClick);
    }

    public string Username
    {
        get => username;
        set => Set(ref username, value);
    }


    public RelayCommand Login { get; }
    public RelayCommand GoogleLogin { get; }

    private void LoginButtonClick(object sender)
    {
        var password = (sender as PasswordBox)?.Password;
        (string, string) message;
        var success = false;
        if (password is null or "")
        {
            message = ("Wrong password", "Password cannot be empty");
        }
        else if (Username.Length < 3)
        {
            message = ("Wrong username", "Username must be bigger than 3 symbols");
        }
        else if (password.Length < 8)
        {
            message = ("Wrong password", "Password must be longer than 8 symbols");
        }
        else if (MainViewModel.LoggedAccounts.Any(a => a.Username == Username))
        {
            message = ("Login", $"You already logged on, {Username}");
        }
        else if (MainViewModel.AccountsData.Login(Username, password))
        {
            success = true;
            message = ("Successful login", $"Hello, {Username}");
        }
        else
        {
            message = ("Error", "Account cannot be found");
        }

        new MessageWindow(message.Item1, message.Item2).ShowDialog();
        if (success) MainViewModel.LoggedAccounts.Add(MainViewModel.AccountsData.GetAccount(username)!);
    }

    private void GoogleLoginClick(object parameter)
    {
        var googleAuth = new GoogleAuth();
        googleAuth.PropertyChanged += GoogleAuthOnResponse;
        googleAuth.Auth();
    }

    private static void GoogleAuthOnResponse(object? sender, PropertyChangedEventArgs e)
    {
        var googleAuth = (GoogleAuth)sender;
        if (googleAuth is null)
        {
            new MessageWindow("Error", "Something went wrong").ShowDialog();
            return;
        }

        var response = googleAuth.UserinfoResponseText;

        var responseText = response.Split(',');

        var login = responseText.First(a => a.Contains("name")).Split(':')[1][2..^1];
        var email = responseText.First(a => a.Contains("email")).Split(':')[1][2..^1];
        var id = responseText.First(a => a.Contains("sub")).Split(':')[1][2..^1];

        var acc = MainViewModel.AccountsData.Login(email, id);
        if (MainViewModel.LoggedAccounts.Any(a => a.Username == login))
        {
            new MessageWindow("Login", $"You already logged on, {login}").ShowDialog();
            return;
        }

        if (acc)
        {
            MainViewModel.LoggedAccounts.Add(MainViewModel.AccountsData.GetAccount(login)!);
            new MessageWindow("Successful login", $"Hello, {login}").ShowDialog();
        }
        else
        {
            if (MainViewModel.AccountsData.AddAccount(login, email, id))
            {
                MainViewModel.LoggedAccounts.Add(MainViewModel.AccountsData.GetAccount(login)!);
                MainViewModel.PlayersData.AddPlayer(login);
                new MessageWindow("Successful registration", $"Hello, {login}").ShowDialog();
            }
            else
            {
                new MessageWindow("Unsuccessful registration", $"This account already registered, {login}")
                    .ShowDialog();
            }
        }
    }
}