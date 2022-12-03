
using JapaneseCheckers.Models;

namespace JapaneseCheckers.ViewModels;

internal class Cell : MvvmBase
{
    private int row;
    private int col;
    private Color color;
    private bool isFree = true;
    private string content;

    public string Content
    {
        get => content;
        set => Set(ref content, value);
    }

    public bool IsFree
    {
        get => isFree;
        set => Set(ref isFree, value);
    }
    public Color Color
    {
        get => color;
        set
        {
            Set(ref color, value);
            Content = $"Resources/{Color}piece.png";
        }
    }

    public int Row
    {
        get => row;
        set => Set(ref row, value);
    }
    public int Col
    {
        get => col;
        set => Set(ref col, value);
    }

    public Cell(Color color, int row, int col)
    {
        Color = color;
        Row = row;
        Col = col;
    }
}