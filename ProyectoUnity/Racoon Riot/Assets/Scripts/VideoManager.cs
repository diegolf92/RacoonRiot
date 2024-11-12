using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;  // Reference to the VideoPlayer component
    public string nextSceneName;     // Name of the next scene to load

    void Start()
    {
        // Ensure the video starts playing as soon as the scene loads
        videoPlayer.Play();

        // Register a callback to detect when the video finishes
        videoPlayer.loopPointReached += OnVideoFinished;
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        // Load the next scene when the video stops
        SceneManager.LoadScene(nextSceneName);
    }
}

