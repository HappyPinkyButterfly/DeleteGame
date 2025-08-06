using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public void LoadScene(string loadToScene)
    {
        SceneManager.LoadScene(loadToScene);
    }
}
