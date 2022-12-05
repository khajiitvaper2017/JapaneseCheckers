using System.Collections.ObjectModel;
using System.Linq;
using JapaneseCheckers.Models;
using JapaneseCheckers.Views;

namespace JapaneseCheckers.ViewModels;

internal class SelectPlayersViewModel : MvvmBase
{
    private Player? firstPlayer;
    private bool onRating;
    private Player? secondPlayer;

    public SelectPlayersViewModel()
    {
        var logged = MainViewModel.LoggedAccounts;
        var players = MainViewModel.PlayersData.Collection.Where(p => logged.Any(a => a.Username == p.Username));
        AvailablePlayers = new ObservableCollection<Player>(players);
        foreach (var bot in MainViewModel.BotsData.Collection) AvailablePlayers.Add(bot);
        SelectCommand = new RelayCommand(Select,
            _ =>
            {
                if (FirstPlayer == null || SecondPlayer == null || FirstPlayer == SecondPlayer ||
                    (FirstPlayer.IsBot && SecondPlayer.IsBot)) return false;
                return true;
            });
    }

    public bool OnRating
    {
        get => onRating;
        set => Set(ref onRating, value);
    }

    public Player? FirstPlayer
    {
        get => firstPlayer;
        set => Set(ref firstPlayer, value);
    }

    public Player? SecondPlayer
    {
        get => secondPlayer;
        set => Set(ref secondPlayer, value);
    }

    public ObservableCollection<Player> AvailablePlayers { get; set; }

    public RelayCommand SelectCommand { get; }

    private void Select(object obj)
    {
        if (obj is SelectPlayers win) win.DialogResult = true;
    }
}