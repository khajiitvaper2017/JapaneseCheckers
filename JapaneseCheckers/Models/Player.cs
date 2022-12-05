using System;
using System.Collections.ObjectModel;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Models;

public class Player : MvvmBase
{
    private readonly string username;
    public ObservableCollection<Game> PlayedGames = new();
    private int rating = 1500;

    public Player(string username, bool isBot = false)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty or whitespace", nameof(username));

        Username = username;
        IsBot = isBot;
    }

    public bool IsBot;

    public string Username
    {
        get => username;
        private init => Set(ref username, value);
    }

    public int Rating
    {
        get => rating;
        set => Set(ref rating, value);
    }
}