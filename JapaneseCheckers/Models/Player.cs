using System;
using System.Windows.Media;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Models;

internal class Player : MvvmBase
{
    private Color color;
    private Guid id;

    public Player(Guid id, Color color)
    {
        Id = id;
        Color = color;
    }

    public Guid Id
    {
        get => id;
        set => Set(ref id, value);
    }

    public Color Color
    {
        get => color;
        set => Set(ref color, value);
    }
}