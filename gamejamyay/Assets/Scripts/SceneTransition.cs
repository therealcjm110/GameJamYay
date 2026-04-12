using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransition : MonoBehaviour
{
    public static SceneTransition Instance;

    [Header("Fade Settings")]
    public CanvasGroup fadeCanvasGroup;  // A full-screen black Image with CanvasGroup
    public float fadeDuration = 1f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // Fade in when scene loads
        StartCoroutine(FadeIn());
    }

    public void GoToNextScene()
    {
        StartCoroutine(FadeAndLoad());
    }

    IEnumerator FadeIn()
    {
        fadeCanvasGroup.alpha = 1f;
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = 1f - (t / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;
    }

    IEnumerator FadeAndLoad()
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            fadeCanvasGroup.alpha = t / fadeDuration;
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;

        int nextScene = SceneManager.GetActiveScene().buildIndex + 1;
        SceneManager.LoadScene(nextScene);
    }
}
