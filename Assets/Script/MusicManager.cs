using System.Collections.Generic;
using UnityEngine;

public class MusicManager : Singleton<MusicManager>
{
    // ファイル名は enum 名を小文字にしたもの(Jump → Resources/Music/Se/jump)
    public enum SeType
    {
        Jump,
        GameOver,
        ItemGet,
    }

    private const string BgmPath = "Music/Bgm/";
    private const string SePath = "Music/Se/";

    [Header("音量")]
    [SerializeField, Range(0f, 1f)] private float bgmVolume = 0.6f;
    [SerializeField, Range(0f, 1f)] private float seVolume = 1f;

    private AudioSource bgmSource;
    private AudioSource seSource;
    private readonly Dictionary<string, AudioClip> clipCache = new Dictionary<string, AudioClip>();

    public override void Awake()
    {
        base.Awake();                       // 重複していたらここで破棄される
        if (Instance != this) return;

        DontDestroyOnLoad(gameObject);      // シーンを読み込み直しても残す

        bgmSource = gameObject.AddComponent<AudioSource>();
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume;

        seSource = gameObject.AddComponent<AudioSource>();
        seSource.playOnAwake = false;
    }

    // ---------- SE ----------
    public void PlaySe(SeType type)
    {
        AudioClip clip = Load(SePath + type.ToString().ToLowerInvariant());
        if (clip != null) seSource.PlayOneShot(clip, seVolume);
    }

    // ---------- BGM ----------
    // 例: PlayBgm("game") → Resources/Music/Bgm/game
    public void PlayBgm(string bgmName)
    {
        AudioClip clip = Load(BgmPath + bgmName);
        if (clip == null) return;
        if (bgmSource.clip == clip && bgmSource.isPlaying) return;   // 同じ曲なら継続

        bgmSource.clip = clip;
        bgmSource.volume = bgmVolume;
        bgmSource.Play();
    }

    public void StopBgm()
    {
        bgmSource.Stop();
    }

    // ---------- 音量 ----------
    public void SetBgmVolume(float v)
    {
        bgmVolume = Mathf.Clamp01(v);
        bgmSource.volume = bgmVolume;
    }

    public void SetSeVolume(float v)
    {
        seVolume = Mathf.Clamp01(v);
    }

    // ---------- 内部 ----------
    private AudioClip Load(string path)
    {
        if (clipCache.TryGetValue(path, out AudioClip cached)) return cached;

        AudioClip clip = Resources.Load<AudioClip>(path);
        if (clip == null)
            Debug.LogWarning("MusicManager: 音源が見つかりません → Resources/" + path);

        clipCache[path] = clip;
        return clip;
    }
}
