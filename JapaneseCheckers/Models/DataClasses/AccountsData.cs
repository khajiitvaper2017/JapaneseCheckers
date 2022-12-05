using System.Linq;
using JapaneseCheckers.Models.Services;

namespace JapaneseCheckers.Models.DataClasses;

public record Account(string Username, string Email, string PasswordHash);

public class AccountsData : Data<Account>
{
    public Account? GetAccount(string login)
    {
        return Collection.Where(a => a.Email == login || a.Username == login).ToList().FirstOrDefault();
    }

    public bool AddAccount(string username, string email, string password)
    {
        if (Collection.Any(a => a.Email == email || a.Username == username)) return false;
        Collection.Add(new Account(username, email, Encryption.Sha256String(password)));
        return true;
    }

    public bool Login(string login, string password)
    {
        if (GetAccount(login) is not Account acc) return false;
        return acc.PasswordHash == Encryption.Sha256String(password);
    }
}