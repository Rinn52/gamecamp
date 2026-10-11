using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearScreen : MonoBehaviour
{
    [SerializeField, Tooltip("タイトルボタンで開くシーン名")]
    private string titleSceneName = "Title";
    [SerializeField, Tooltip("リトライボタンで開くゲームシーン名")]
    private string retrySceneName = "Geme";

    private GUIStyle messageStyle;
    private GUIStyle buttonStyle;

    private void OnGUI()
    {
        if (messageStyle == null)
        {
            messageStyle = new GUIStyle(GUI.skin.label)
            {
                alignment = TextAnchor.MiddleCenter,
                fontSize = Mathf.Clamp(Screen.height / 12, 28, 64),
                fontStyle = FontStyle.Bold
            };
        }

        GUI.color = new Color(0.08f, 0.08f, 0.12f);
        GUI.DrawTexture(new Rect(0f, 0f, Screen.width, Screen.height), Texture2D.whiteTexture);
        GUI.color = Color.white;
        GUI.Label(new Rect(0f, Screen.height * 0.35f, Screen.width, Screen.height * 0.3f), "GAME CLEAR!", messageStyle);

        if (buttonStyle == null)
        {
            buttonStyle = new GUIStyle(GUI.skin.button)
            {
                fontSize = Mathf.Clamp(Screen.height / 30, 18, 32)
            };
        }

        float buttonWidth = Mathf.Min(Screen.width * 0.6f, 360f);
        float buttonHeight = Mathf.Clamp(Screen.height * 0.09f, 48f, 72f);
        float buttonX = (Screen.width - buttonWidth) * 0.5f;
        float buttonY = Screen.height * 0.62f;

        if (GUI.Button(new Rect(buttonX, buttonY, buttonWidth, buttonHeight), "タイトルへ", buttonStyle))
        {
            LoadScene(titleSceneName);
        }

        if (GUI.Button(new Rect(buttonX, buttonY + buttonHeight + 16f, buttonWidth, buttonHeight), "リトライ", buttonStyle))
        {
            LoadScene(retrySceneName);
        }
    }

    private void LoadScene(string sceneName)
    {
        if (string.IsNullOrWhiteSpace(sceneName))
        {
            Debug.LogError("Set the destination scene name in the ClearScreen Inspector.", this);
            return;
        }

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
