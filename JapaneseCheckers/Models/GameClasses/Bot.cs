using System;
using System.Text.Json.Serialization;

namespace JapaneseCheckers.Models.GameClasses;

public class Bot : Player
{
    [JsonIgnore] public Func<CellBoard, (int, int)>? CalculateMove;

    public Bot(string username, int rating = 1500) : base(username, true)
    {
        Rating = rating;
    }
}