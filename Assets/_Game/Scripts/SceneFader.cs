using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneFader : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private CanvasGroup uiGroup;
    [SerializeField] private float fadeSpeed = 1f;

    private void Start()
    {
        // Whenever a level starts, we assume the screen is black, so we fade IN.
        // Ensure the panel starts black
        uiGroup.alpha = 1;
        StartCoroutine(FadeIn());
    }

    private void OnEnable()
    {
        // Nos suscribimos al evento de carga de escenas
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        // Nos desuscribimos para evitar errores de memoria
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // Este método se ejecuta AUTOMÁTICAMENTE cada vez que cambia la escena
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StopAllCoroutines(); // Detenemos cualquier fade previo
        uiGroup.alpha = 1;   // Forzamos negro
        uiGroup.blocksRaycasts = true;
        StartCoroutine(FadeIn());
    }

    public void FadeToLevel(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    }

    private IEnumerator FadeIn()
    {
        while (uiGroup.alpha > 0)
        {
            uiGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        uiGroup.alpha = 0;
        uiGroup.blocksRaycasts = false; // Let player click things again
    }

    private IEnumerator FadeOut(string sceneName)
    {
        uiGroup.blocksRaycasts = true; // Block clicks while fading

        // Fade from 0 (Clear) to 1 (Black)
        while (uiGroup.alpha < 1)
        {
            uiGroup.alpha += Time.deltaTime * fadeSpeed;
            yield return null;
        }

        // Wait a tiny bit for drama
        yield return new WaitForSeconds(0.5f);

        // Load the next level
        SceneManager.LoadScene(sceneName);
    }
}