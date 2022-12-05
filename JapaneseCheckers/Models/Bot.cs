using System;

namespace JapaneseCheckers.Models;

public class Bot : Player
{
    public Bot(string username, Func<CellBoard, (int, int)> calculate) : base(username, true)
    {
        CalculateMove = calculate;
    }

    public Func<CellBoard, (int, int)> CalculateMove;
}