using System.Linq;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Models;

public delegate void Message();

internal class JapaneseCheckers : MvvmBase
{
    private CellBoard board;
    private bool isGameEnded;
    private uint turn;

    public JapaneseCheckers()
    {
        Board = new CellBoard();
        for (var i = 0; i < CellBoard.BoardSize; i++)
        for (var j = 0; j < CellBoard.BoardSize; j++)
            Board.Add(new Cell(Color.None, i, j));
    }

    public bool IsGameEnded
    {
        get => isGameEnded;
        set => Set(ref isGameEnded, value);
    }

    public Color Result { get; private set; }

    public uint Turn
    {
        get => turn;
        set => Set(ref turn, value);
    }

    public CellBoard Board
    {
        get => board;
        set => Set(ref board, value);
    }

    public void Surrender()
    {
        isGameEnded = true;
        Result = WhoseTurn() == Color.White ? Color.Black : Color.White;
        GameEnded?.Invoke();
    }

    public Color WhoseTurn()
    {
        return (Color)(Turn % 2);
    }

    internal void SetCell(Cell cell)
    {
        if (isGameEnded) return;
        Board[cell.Row, cell.Col].Color = WhoseTurn();
        Board[cell.Row, cell.Col].IsFree = false;
        if (CheckForWin())
        {
            isGameEnded = true;
            Result = WhoseTurn();
            GameEnded?.Invoke();
            return;
        }

        if (!Board.Any(a => a.IsFree))
        {
            isGameEnded = true;
            Result = Color.None;
            GameEnded?.Invoke();
            return;
        }

        Turn++;
    }

    public event Message? GameEnded;

    private bool CheckForWin()
    {
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
            for (var y = 0; y < CellBoard.BoardSize; y++)
            {
                var lastx = x + 4 * dx;
                var lasty = y + 4 * dy;
                if (lastx is < 0 or >= CellBoard.BoardSize ||
                    lasty is < 0 or >= CellBoard.BoardSize) continue;

                var w = Board[x, y].Color;
                if (w == Color.None) continue;
                if (w == Board[x + dx, y + dy].Color
                    && w == Board[x + 2 * dx, y + 2 * dy].Color
                    && w == Board[x + 3 * dx, y + 3 * dy].Color
                    && w == Board[lastx, lasty].Color)
                    return true;
            }
        }

        return false;
    }
}