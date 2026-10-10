using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
#region Unity Function
    private void Start()
    {
        Initialize();
    }

    private void Update()
    {
    }
#endregion

    /// <summary>
    /// 初期化
    /// </summary>
    private static void Initialize()
    {
        SetupInstance();
    }

    /// <summary>
    /// タイトルシーンに移動
    /// </summary>
    public void LoadTitleScene()
    {
        SceneManager.UnloadSceneAsync("Geme");
        SceneManager.LoadScene("Title", LoadSceneMode.Additive);
    }

    /// <summary>
    /// ゲームシーンに移動
    /// </summary>
    public void LoadGameScene()
    {
        SceneManager.UnloadSceneAsync("Title");
        SceneManager.LoadScene("Geme", LoadSceneMode.Additive);
    }


}
