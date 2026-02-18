using UnityEngine;

public class PersistentUI : MonoBehaviour
{
    public static PersistentUI Instance;

    private void Awake()
    {
        // SINGLETON PATTERN:
        // 1. Is there already a Master UI?
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        // 2. No?
        Instance = this;

        // 3. Keep me alive across scenes.
        DontDestroyOnLoad(gameObject);
    }


}