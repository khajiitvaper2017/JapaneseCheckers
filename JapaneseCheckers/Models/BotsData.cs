using System;

namespace JapaneseCheckers.Models;

public class BotsData : Data<Bot>
{
    public void AddBot(string botname, Func<CellBoard, (int, int)> func)
    {
        Collection.Add(new Bot(botname, func));
    }
}