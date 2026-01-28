[System.Serializable]
public class GameData
{
    // We save the Name of the level the player should start in
    public string levelToLoad;

    // You can add more later: e.g., public int coins;

    // Constructor to set default values (New Game)
    public GameData()
    {
        levelToLoad = "Level1";
    }
}