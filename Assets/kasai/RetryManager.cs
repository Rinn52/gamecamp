using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    [Header("Scene Destinations")]
    [SerializeField, Tooltip("Retryボタンを押したときに開くシーン名")]
    private string retryDestinationScene = "Geme";
    [SerializeField, Tooltip("あきらめるボタンを押したときに開くシーン名")]
    private string giveUpDestinationScene = "GameOver";

    public void RetryGame()
    {
        LoadDestinationScene(retryDestinationScene);
    }

    public static void LoadRetryScene()
    {
        SceneManager.LoadScene("Retry", LoadSceneMode.Single);
    }

    public void GiveUp()
    {
        LoadDestinationScene(giveUpDestinationScene);
    }

    public void LoadScene(string sceneName)
    {
        LoadDestinationScene(sceneName);
    }

    private void LoadDestinationScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Set a destination scene name in the RetryManager Inspector.", this);
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
