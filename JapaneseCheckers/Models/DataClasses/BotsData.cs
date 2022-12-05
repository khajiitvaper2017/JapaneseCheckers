using System;
using System.Linq;
using JapaneseCheckers.Models.GameClasses;

namespace JapaneseCheckers.Models.DataClasses;

public class BotsData : Data<Bot>
{
    public BotsData() : base()
    {
        if (Collection.Count == 0)
        {
            AddBot("Random Bot");
            AddBot("Smarter Bot");
        }

        AddFuncs();
    }

    public void AddBot(string botname)
    {
        Collection.Add(new Bot(botname));
    }

    private void AddFuncs()
    {
        Cell GetRandomCell(CellBoard cellBoard)
        {
            var cells = cellBoard.Where(cell => cell.IsFree);
            var cell1 = cells.ElementAt(new Random().Next(cells.Count()));
            return cell1;
        }

        (Collection[0] as Bot).CalculateMove = (board) =>
        {
            var cell = GetRandomCell(board);
            return (cell.Row, cell.Col);
        };
        var bot = Collection[1] as Bot;
        bot.CalculateMove = (board) =>
        {
            var move = (0, 0);
            var qual = 0;
            int[][] directions =
            {
                new[] { 1, 0 },
                new[] { 1, -1 },
                new[] { 1, 1 },
                new[] { 0, 1 }
            };
            foreach (var direction in directions)
            {
                var dx = direction[0];
                var dy = direction[1];

                for (var x = 0; x < CellBoard.BoardSize; x++)
                {
                    for (var y = 0; y < CellBoard.BoardSize; y++)
                    {
                        try
                        {
                            var w = board[x, y].Color;
                            if (w == Color.None) continue;
                            if (w == board[x + dx, y + dy].Color)
                            {
                                if (w == board[x + 2 * dx, y + 2 * dy].Color)
                                {
                                    if (w == board[x + 3 * dx, y + 3 * dy].Color)
                                    {
                                        if (board[x + 4 * dx, y + 4 * dy].IsFree)
                                        {
                                            if (qual < 4)
                                            {
                                                qual = 4;
                                                move = (x + 4 * dx, y + 4 * dy);
                                            }
                                        }

                                        if (board[x - dx, y - dy].IsFree)
                                        {
                                            if (qual < 4)
                                            {
                                                qual = 4;
                                                move = (x - dx, y - dy);
                                            }
                                        }
                                    }

                                    if (board[x + 3 * dx, y + 3 * dy].IsFree)
                                    {
                                        if (qual < 3)
                                        {
                                            qual = 3;
                                            move = (x + 3 * dx, y + 3 * dy);
                                        }
                                    }

                                    if (board[x - dx, y - dy].IsFree)
                                    {
                                        if (qual < 3)
                                        {
                                            qual = 3;
                                            move = (x - dx, y - dy);
                                        }
                                    }
                                }

                                if (board[x + 2 * dx, y + 2 * dy].IsFree)
                                {
                                    if (qual < 2)
                                    {
                                        qual = 2;
                                        move = (x + 2 * dx, y + 2 * dy);
                                    }
                                }

                                if (board[x - dx, y - dy].IsFree)
                                {
                                    if (qual < 2)
                                    {
                                        qual = 2;
                                        move = (x - dx, y - dy);
                                    }
                                }
                            }

                            if (board[x + 1 * dx, y + 1 * dy].IsFree)
                            {
                                if (qual < 1)
                                {
                                    qual = 1;
                                    move = (x + 1 * dx, y + 1 * dy);
                                }
                            }

                            if (board[x - dx, y - dy].IsFree)
                            {
                                if (qual < 1)
                                {
                                    qual = 1;
                                    move = (x - dx, y - dy);
                                }
                            }
                        }
                        catch (Exception)
                        {
                            //
                        }
                    }
                }
            }

            if (qual != 0) return move;
            var cell = GetRandomCell(board);
            return (cell.Row, cell.Col);
        };
    }
}