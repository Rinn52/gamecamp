using UnityEngine;

// シーン開始時に BGM を流す。シーンに空のオブジェクトを作ってアタッチする
public class SceneBgm : MonoBehaviour
{
    [SerializeField] private string bgmName = "game";   // Resources/Music/Bgm/<名前>

    private void Start()
    {
        MusicManager.Instance.PlayBgm(bgmName);
    }
}
