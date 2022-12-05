namespace JapaneseCheckers.Models;

public record Game(Player FirstPlayer, Player SecondPlayer, Color Result, bool PlayedOnRating);