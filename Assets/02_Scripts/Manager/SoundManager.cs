using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : GenericSingleton<SoundManager>
{
    public AudioSource BgmSource => bgmAudio;
    [SerializeField] AudioSource bgmAudio;    // 루프 / 페이드 / 상태관리
    [SerializeField] AudioSource eventAudio;  // 버튼, 효과음
    public string CurrentBgm { get; private set; }
    private Dictionary<string, AudioClip> clipDatabase = new Dictionary<string, AudioClip>();

    private const string BGM_PATH = "Audio/BGM";
    private const string CLIP_PATH = "Audio/Clip";

    protected override void Awake()
    {
        base.Awake();

        LoadClipsFromResources(BGM_PATH);
        LoadClipsFromResources(CLIP_PATH);
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

        CurrentBgm = name;

        float volume = PlayerPrefs.GetFloat("SoundSetting_BGMVolume", 1f);

        bgmAudio.volume = volume;
        bgmAudio.loop = loop;
        bgmAudio.clip = clip;
        bgmAudio.Play();
    }

    public void EventSoundPlay(string clipName, float volume = 1f)
    {
        if (eventAudio == null)
        {
            return;
        }

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
}
