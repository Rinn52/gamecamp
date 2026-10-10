using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot
{
    /// <summary>
    /// ゲーム起動時に実行される
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Init()
    {
        // Managaer用のシーンを開く
        SceneManager.LoadScene("Manager", LoadSceneMode.Additive);
    }
}