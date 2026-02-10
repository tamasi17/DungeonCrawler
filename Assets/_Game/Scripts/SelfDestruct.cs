using UnityEngine;
public class SelfDestruct : MonoBehaviour
{
    void Start()
    {
        // Destroy this object after the animation finishes (e.g., 1 second)
        Destroy(gameObject, 1.0f);
    }
}