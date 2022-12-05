using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Models.GameClasses;

public class Cell : MvvmBase
{
    private int col;
    private Color color;
    private string content;
    private bool isFree = true;
    private int row;

    public Cell(Color color, int row, int col)
    {
        Color = color;
        Row = row;
        Col = col;
    }

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
            Content = $"../Resources/{Color}piece.png";
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
}