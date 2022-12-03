using System.Collections.ObjectModel;
using JapaneseCheckers.ViewModels;

namespace JapaneseCheckers.Models;

internal class CellBoard : ObservableCollection<Cell>
{
    public const int BoardSize = 10;

    public Cell this[int row, int column]
    {
        get => this[row * BoardSize + column];
        set => this[row * BoardSize + column] = value;
    }
}