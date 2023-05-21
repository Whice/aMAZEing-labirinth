using TMPro;
using UnityEngine;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] private float updateInterval = 0.1f;
    [SerializeField] private int targetFrameRate = 300;
    [SerializeField] private TextMeshProUGUI fpsText = null;

    private float accumDeltaTime = 0f;
    private int frames = 0;
    private float fps = 0f;
    private float averageFPS = 0f;

    private void Start()
    {
        // Set the target frame rate
        Application.targetFrameRate = targetFrameRate;

        // Start the FPS calculation
        StartCoroutine(UpdateFPS());
    }

    private System.Collections.IEnumerator UpdateFPS()
    {
        float totalTime = 0f;
        int totalFrames = 0;

        while (true)
        {
            // Calculate FPS over the specified interval
            accumDeltaTime += Time.deltaTime;
            frames++;

            if (accumDeltaTime >= updateInterval)
            {

                totalFrames += frames;
                averageFPS = totalFrames / Time.time;
                ShowFps();
                fps = frames / accumDeltaTime;
                accumDeltaTime = 0f;
                frames = 0;

            }
            yield return null;
        }
    }

    private void ShowFps()
    {
        this.fpsText.text = $"FPS: {fps.ToString("F2")}\n" +
            $"Average FPS: {averageFPS.ToString("F2")}";
    }
}
