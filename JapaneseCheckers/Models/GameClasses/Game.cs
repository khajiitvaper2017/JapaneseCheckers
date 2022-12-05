namespace JapaneseCheckers.Models.GameClasses;

public class Game
{
    public Game(Player FirstPlayer, Player SecondPlayer, Color Result, bool PlayedOnRating)
    {
        this.FirstPlayer = FirstPlayer;
        this.SecondPlayer = SecondPlayer;
        this.Result = Result;
        this.PlayedOnRating = PlayedOnRating;
    }

    public Player FirstPlayer { get; set; }
    public Player SecondPlayer { get; set; }
    public Color Result { get; set; }
    public bool PlayedOnRating { get; set; }
    public int FirstRatingChange { get; set; } = 0;
    public int SecondRatingChange { get; set; } = 0;
}