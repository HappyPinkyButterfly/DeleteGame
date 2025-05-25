using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour {

    public CanvasGroup logo;

    void Start() {
        // Na začetku skrij vse
 
        logo.alpha = 0;

        StartCoroutine(OpeningAnimation());
    }

    IEnumerator OpeningAnimation() {

        yield return StartCoroutine(FadeCanvasGroup(logo, 0f, 1f, 2f));

        // 5. Naloži glavni meni
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("MainMenu");
    }

    // Pomožna funkcija za fade CanvasGroup (za logo + presented by)
    IEnumerator FadeCanvasGroup(CanvasGroup group, float startAlpha, float endAlpha, float duration) {
        float elapsed = 0f;
        while (elapsed < duration) {
            group.alpha = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        group.alpha = endAlpha;
    }

    // Pomožna funkcija za fade TMP_Text (za "TIC TAC TOE" in "DELETE")
    IEnumerator FadeText(TMP_Text text, float startAlpha, float endAlpha, float duration) {
        float elapsed = 0f;
        Color color = text.color;
        while (elapsed < duration) {
            color.a = Mathf.Lerp(startAlpha, endAlpha, elapsed / duration);
            text.color = color;
            elapsed += Time.deltaTime;
            yield return null;
        }
        color.a = endAlpha;
        text.color = color;
    }
}