using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class OpeningScene : MonoBehaviour {

    public TMP_Text presentedBy;
    public CanvasGroup logo;
    public TMP_Text ticTacToeText;      // "TIC TAC TOE"
    public TMP_Text deleteText;         // "DELETE"


    public float logoFadeInTime = 1f;  // Čas za pojavljanje logotipa
    public float logoDisplayTime = 2f;   // Čas prikaza logotipa
    public float titleDelay = 0.5f;      // Zamik pred "TIC TAC TOE"
    public float deleteFadeInTime = 3f;  // Počasno pojavljanje "DELETE"

    void Start() {
        // Na začetku skrij vse
        presentedBy.alpha = 0;
        ticTacToeText.alpha = 0;
        deleteText.alpha = 0;
        logo.alpha = 0;

        StartCoroutine(OpeningAnimation());
    }

    IEnumerator OpeningAnimation() {

        yield return StartCoroutine(FadeText(presentedBy, 0f, 1f, logoFadeInTime));
        yield return StartCoroutine(FadeCanvasGroup(logo, 0f, 1f, logoFadeInTime));

        yield return new WaitForSeconds(logoFadeInTime);
        yield return StartCoroutine(FadeText(ticTacToeText, 0f, 1f, logoFadeInTime));

        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(FadeText(deleteText, 0f, 1f, 3.5f));

        // 5. Naloži glavni meni
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("LoadToMainMenu");
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