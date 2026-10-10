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
}
