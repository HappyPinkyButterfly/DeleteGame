using UnityEngine;

public class ScreenShoter : MonoBehaviour
{
    int count = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ScreenCapture.CaptureScreenshot($"/Users///screenshot-{count++}.png");
        }
    }
    
}
