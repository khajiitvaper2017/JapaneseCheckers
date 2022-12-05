using JapaneseCheckers.Models.GameClasses;

namespace JapaneseCheckers.Models.DataClasses;

public class PlayersData : Data<Player>
{
    public void AddPlayer(string username)
    {
        Collection.Add(new Player(username));
    }
}