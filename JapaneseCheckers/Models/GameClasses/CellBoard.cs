using System;
using System.Collections.ObjectModel;

namespace JapaneseCheckers.Models.GameClasses;

public class CellBoard : ObservableCollection<Cell>, IDisposable
{
    public const int BoardSize = 10;

    public Cell this[int row, int column]
    {
        get => this[row * BoardSize + column];
        set => this[row * BoardSize + column] = value;
    }

    public void Dispose()
    {
        Clear();
    }
}