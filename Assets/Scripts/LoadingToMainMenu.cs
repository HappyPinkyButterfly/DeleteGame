using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingToMainMenu : MonoBehaviour
{
    public Image progressBarFill; // referenca na "ProgressBarFill"
    public float loadTime = 4f; // čas polnjenja v sekundah
    public string nextSceneName = "NextScene"; // ime naslednje scene

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        float progress = Mathf.Clamp01(timer / loadTime);

        // Update fillAmount (0 do 1)
        progressBarFill.fillAmount = progress;

        if (timer >= loadTime)
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
