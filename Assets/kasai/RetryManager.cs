using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryManager : MonoBehaviour
{
    [Header("Scene Destinations")]
    [SerializeField, Tooltip("Retryボタンを押したときに開くシーン名")]
    private string retryDestinationScene = "Geme";
    [SerializeField, Tooltip("あきらめるボタンを押したときに開くシーン名")]
    private string giveUpDestinationScene = "GameOver";

    private GUIStyle titleStyle;
    private GUIStyle buttonStyle;

    private void OnGUI()
    {
        InitializeStyles();
        GUI.color = new Color(0.08f, 0.08f, 0.12f);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "GameOver")
        {
            GUI.Label(
                new Rect(0f, Screen.height * 0.24f, Screen.width, Screen.height * 0.18f),
                "GAME OVER",
                titleStyle);
            return;
        }

        if (currentScene != "Retry")
        {
            return;
        }

        float centerX = Screen.width * 0.5f;
        float buttonWidth = Mathf.Min(Screen.width * 0.6f, 360f);
        float buttonHeight = Mathf.Clamp(Screen.height * 0.09f, 48f, 72f);
        float buttonX = centerX - buttonWidth * 0.5f;

        if (GUI.Button(
            new Rect(buttonX, Screen.height * 0.5f, buttonWidth, buttonHeight),
            "Retry",
            buttonStyle))
        {
            RetryGame();
        }

        if (GUI.Button(
            new Rect(buttonX, Screen.height * 0.5f + buttonHeight + 16f, buttonWidth, buttonHeight),
            "Give Up",
            buttonStyle))
        {
            GiveUp();
        }
    }

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

    private void LoadDestinationScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Set a destination scene name in the RetryManager Inspector.", this);
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    private void InitializeStyles()
    {
        if (titleStyle != null)
        {
            return;
        }

        titleStyle = new GUIStyle(GUI.skin.label)
        {
            alignment = TextAnchor.MiddleCenter,
            fontSize = Mathf.Clamp(Screen.height / 12, 28, 64),
            fontStyle = FontStyle.Bold
        };
        buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = Mathf.Clamp(Screen.height / 30, 18, 32)
        };
    }
}
