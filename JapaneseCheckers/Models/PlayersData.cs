namespace JapaneseCheckers.Models;

public class PlayersData : Data<Player>
{
    public void AddPlayer(string username)
    {
        Collection.Add(new Player(username));
    }
}