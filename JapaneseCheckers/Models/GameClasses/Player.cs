using System;
using System.Text.Json.Serialization;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Models.GameClasses;

public class Player : MvvmBase
{
    [JsonIgnore] protected readonly string username;

    [JsonIgnore] protected int rating = 1500;

    public Player(string username, bool isBot = false)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be empty or whitespace", nameof(username));

        Username = username;
        IsBot = isBot;
    }

    [JsonIgnore] public bool IsBot { get; set; }

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