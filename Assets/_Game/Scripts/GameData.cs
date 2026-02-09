using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    // We save the Name of the level the player should start in
    public string levelToLoad;
    public int chests;     
    public int deaths;     
    public float timePlayed;
    public List<string> collectedItemsID;



    // Constructor to set default values (New Game)
    public GameData()
    {
        levelToLoad = "Level1";
        chests = 0;
        deaths = 0;
        timePlayed = 0;
        collectedItemsID = new List<string>();
    }
}