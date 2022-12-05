using System;

namespace JapaneseCheckers.Models;

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
        const int cofficient = 50;
        double firstRating = game.FirstPlayer.Rating;
        double secondRating = game.SecondPlayer.Rating;

        var firstProbability = Probability(firstRating, secondRating);
        var secondProbability = Probability(secondRating, firstRating);

        switch (game.Result)
        {
            case Color.White:
                firstRating += cofficient * (1 - firstProbability);
                secondRating += cofficient * (0 - secondProbability);
                break;
            case Color.Black:
                firstRating += cofficient * (0 - firstProbability);
                secondRating += cofficient * (1 - secondProbability);
                break;
            case Color.None:
                firstRating += cofficient * (0.5 - firstProbability);
                secondRating += cofficient * (0.5 - secondProbability);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        if (firstRating < 100) firstRating = 100;
        if (secondRating < 100) secondRating = 100;
        game.FirstPlayer.Rating = Convert.ToInt32(Math.Round(firstRating * 1000000.0) / 1000000.0);
        game.SecondPlayer.Rating = Convert.ToInt32(Math.Round(secondRating * 1000000.0) / 1000000.0);
    }
}