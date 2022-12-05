using System;
using System.Text.Json.Serialization;

namespace JapaneseCheckers.Models;

public class Bot : Player
{
    public Bot(string username, Func<CellBoard, (int, int)>? calculate = null) : base(username, true)
    {
        if(calculate is not null)
            CalculateMove = calculate;
    }
    [JsonIgnore]
    public Func<CellBoard, (int, int)>? CalculateMove;
}