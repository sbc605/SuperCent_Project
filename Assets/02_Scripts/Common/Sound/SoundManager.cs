using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 실제 오디오 클립 로드
/// AudioSource로 사운드 재생
/// BGM / SFX 관리
/// </summary>
public class SoundManager : GenericSingleton<SoundManager>
{
    public AudioSource BgmSource => bgmAudio;
    [SerializeField] AudioSource bgmAudio;    // 루프 / 페이드 / 상태관리
    [SerializeField] AudioSource eventAudio;  // 효과음
    [SerializeField] private AudioSource uiAudio; // 버튼 클릭음
    public string CurrentBgm { get; private set; }
    private Dictionary<string, AudioClip> clipDatabase = new Dictionary<string, AudioClip>();

    private const string BGM_PATH = "Audio/BGM";
    private const string CLIP_PATH = "Audio/Clip";

    #region Button 연결
    public bool IsMuted { get; private set; }

    private const string SOUND_MUTED_KEY = "SoundSetting_IsMuted";
    #endregion

    protected override void Awake()
    {
        base.Awake();

        LoadClipsFromResources(BGM_PATH);
        LoadClipsFromResources(CLIP_PATH);

        IsMuted = PlayerPrefs.GetInt(SOUND_MUTED_KEY, 0) == 1;
        ApplyMuteState();
    }

    private void LoadClipsFromResources(string path)
    {
        AudioClip[] clipsInFolder = Resources.LoadAll<AudioClip>(path);

        Debug.Log($"[SoundManager] {path} 폴더에서 {clipsInFolder.Length}개의 오디오 클립을 로드합니다.");

        foreach (var clip in clipsInFolder)
        {
            if (!clipDatabase.TryAdd(clip.name, clip))
            {
                Debug.LogWarning($"[SoundManager] 이미 딕셔너리에 '{clip.name}' 이름의 클립이 존재합니다. ({path} 경로)");
            }
        }
    }

    #region Play
    public void BgmSoundPlay(string name, bool loop = true)
    {
        if (!clipDatabase.TryGetValue(name, out var clip))
            return;

        if (bgmAudio == null)
            return;

        CurrentBgm = name;

        float volume = PlayerPrefs.GetFloat("SoundSetting_BGMVolume", 0.5f);

        bgmAudio.volume = volume;
        bgmAudio.loop = loop;
        bgmAudio.clip = clip;
        bgmAudio.mute = IsMuted;
        bgmAudio.Play();
    }

    public void EventSoundPlay(string clipName, float volume = 1f)
    {
        if (eventAudio == null)
            return;

        if (IsMuted)
            return;

        if (clipDatabase.TryGetValue(clipName, out AudioClip clip))
        {
            eventAudio.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] 이벤트 클립을 찾을 수 없습니다: {clipName}");
        }
    }
    #endregion

    #region 코루틴
    public void PlayBgmRoutine(string name)
    {
        StartCoroutine(PlayDelayed(name));
    }

    private IEnumerator PlayDelayed(string name)
    {
        yield return null;
        BgmSoundPlay(name);
    }
    #endregion

    #region Fade용
    public IEnumerator FadeOutBGM(float time)
    {
        float start = bgmAudio.volume;
        float t = 0f;

        while (t < time)
        {
            t += Time.deltaTime;
            bgmAudio.volume = Mathf.Lerp(start, 0f, t / time);
            yield return null;
        }

        bgmAudio.Stop();
        bgmAudio.volume = 1f;
    }

    public IEnumerator FadeInBGM(string name, float time)
    {
        if (!clipDatabase.TryGetValue(name, out var clip)) yield break;

        bgmAudio.clip = clip;
        bgmAudio.volume = 0f;
        bgmAudio.Play();

        float t = 0f;
        while (t < time)
        {
            t += Time.deltaTime;
            bgmAudio.volume = Mathf.Lerp(0f, 1f, t / time);
            yield return null;
        }
    }
    #endregion

    #region 버튼 연결
    public void ToggleMute()
    {
        SetMuted(!IsMuted);
    }

    public void SetMuted(bool muted)
    {
        IsMuted = muted;

        PlayerPrefs.SetInt(SOUND_MUTED_KEY, IsMuted ? 1 : 0);
        PlayerPrefs.Save();

        ApplyMuteState();
    }

    private void ApplyMuteState()
    {
        if (bgmAudio != null)
            bgmAudio.mute = IsMuted;

        if (eventAudio != null)
            eventAudio.mute = IsMuted;
    }

    /// <summary>
    /// UI 클릭 전용 함수
    /// </summary>
    public void UISoundPlay(string clipName, float volume = 1f)
    {
        if (uiAudio == null)
        {
            Debug.LogWarning("[SoundManager] uiAudio가 연결되지 않았습니다.");
            return;
        }

        if (clipDatabase.TryGetValue(clipName, out AudioClip clip))
        {
            uiAudio.PlayOneShot(clip, volume);
        }
        else
        {
            Debug.LogWarning($"[SoundManager] UI 클립을 찾을 수 없습니다: {clipName}");
        }
    }
    #endregion
}
