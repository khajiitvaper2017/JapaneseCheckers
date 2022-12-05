using System;
using JapaneseCheckers.Models.GameClasses;

namespace JapaneseCheckers.Models.DataClasses;

public class PlayedGamesData : Data<Game>
{
    public void AddGame(Game game)
    {
        if (game.PlayedOnRating) CalculateRating(game);
        Collection.Add(game);
    }

    private static double Probability(double firstRating, double secondRating)
    {
        return 1.0f * 1.0f / (1 + 1.0f * Math.Pow(10, 1.0f * (secondRating - firstRating) / 400));
    }

    private static void CalculateRating(Game game)
    {
        double firstRating = game.FirstPlayer.Rating;
        double secondRating = game.SecondPlayer.Rating;

        var firstCofficient = (int)(Math.Sqrt(firstRating) / firstRating * 3000);
        var secondCofficient = (int)(Math.Sqrt(secondRating) / secondRating * 3000);

        var firstProbability = Probability(firstRating, secondRating);
        var secondProbability = Probability(secondRating, firstRating);

        switch (game.Result)
        {
            case Color.White:
                firstRating += firstCofficient * (1 - firstProbability);
                secondRating += secondCofficient * (0 - secondProbability);
                break;
            case Color.Black:
                firstRating += firstCofficient * (0 - firstProbability);
                secondRating += secondCofficient * (1 - secondProbability);
                break;
            case Color.None:
                firstRating += firstCofficient * (0.5 - firstProbability);
                secondRating += secondCofficient * (0.5 - secondProbability);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (firstRating < 100) firstRating = 100;
        if (secondRating < 100) secondRating = 100;
        var firstChange = (int)firstRating - game.FirstPlayer.Rating;
        var secondChange = (int)secondRating - game.SecondPlayer.Rating;
        game.FirstPlayer.Rating = Convert.ToInt32(Math.Round(firstRating * 1000000.0) / 1000000.0);
        game.SecondPlayer.Rating = Convert.ToInt32(Math.Round(secondRating * 1000000.0) / 1000000.0);
        game.FirstRatingChange = firstChange;
        game.SecondRatingChange = secondChange;
    }
}