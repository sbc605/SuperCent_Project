
using UnityEngine;

public static class Sfx
{
    public static void Play(string clipName, float volume = 1f)
    {
        if (string.IsNullOrEmpty(clipName))
            return;

        if (SoundManager.Instance == null)
        {
            Debug.LogWarning("[Sfx] SoundManager가 씬에 없습니다.");
            return;
        }

        SoundManager.Instance.EventSoundPlay(clipName, volume);
    }
}
